using Microsoft.Xna.Framework.Audio;
using Synergia.Common;
using Synergia.Content.Dusts;
using Synergia.Content.Projectiles.Hostile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using ValhallaMod.Items.Material;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Dungeon
{
    public class Sparky : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);
        public enum SparkyAIState
        {
            Aim,
            Prepare,
            Dash
        }
        public ref float StateTimer => ref NPC.ai[0];
        public ref float CurrentState => ref NPC.ai[1];
        public ref float PreviousIdentity => ref NPC.ai[2];
        public ref float CanHitTimer => ref NPC.ai[3];

        private const float AimDuration = 30f;
        private const float PrepareDuration = 15f;
        private const float DashDistance = 250f;
        private const float DashDuration = 8f;

        private bool spawned = false;
        private readonly List<int> sparkyTrail = [];

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 70;
            NPC.defense = 25;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.aiStyle = -1;
            NPC.HitSound = null;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = Item.buyPrice(0, 0, 8, 0);
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            NPC.frameCounter++;

            if (NPC.frameCounter >= 8)
            {
                NPC.frameCounter = 0;

                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if(!target.active || target.dead)
            {
                NPC.TargetClosest();
                target = Main.player[NPC.target];
                if (!target.active || target.dead)
                    NPC.active = false;
            }

            CanHitTimer++;

            if (!spawned && Main.netMode != NetmodeID.MultiplayerClient)
            {
                spawned = true;

                float rotationToDash = Main.rand.NextFloat() * MathHelper.TwoPi;
                Vector2 directionToDash = Vector2.UnitX.RotatedBy(rotationToDash) * (DashDistance / DashDuration);
                NPC.rotation = rotationToDash;
                NPC.velocity = directionToDash;
                CurrentState = (float)SparkyAIState.Dash;
                StateTimer = DashDuration;
                PreviousIdentity = -1;

                NPC.netUpdate = true;
            }
            switch (CurrentState)
            {
                case (float)SparkyAIState.Aim:
                    Aim(target);
                    break;
                case (float)SparkyAIState.Prepare:
                    Prepare();
                    break;
                case (float)SparkyAIState.Dash:
                    Dash();
                    break;
            }
        }

        public void Aim(Player target)
        {
            Vector2 direction = target.Center - NPC.Center;
            direction.Normalize();
            NPC.rotation = direction.ToRotation();

            StateTimer--;
            if (StateTimer <= 0)
            {
                CurrentState = (float)SparkyAIState.Prepare;
                StateTimer = PrepareDuration;
            }
        }
        public void Prepare()
        {
            StateTimer--;
            if (StateTimer <= 0)
            {
                NPC.velocity = Vector2.UnitX.RotatedBy(NPC.rotation) * (DashDistance / DashDuration);
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, NPC.Center);
                CurrentState = (float)SparkyAIState.Dash;
                StateTimer = DashDuration;
            }
        }
        public void Dash()
        {
            if(Main.netMode != NetmodeID.Server)
                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ElectricityDust>(), 0, 0);
            StateTimer--;
            if (StateTimer <= 0)
            {
                NPC.velocity = Vector2.Zero;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int previousTrailPoint = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<SparkyTrail>(),
                        Damage: 40,
                        KnockBack: 0,
                        Owner: Main.myPlayer,
                        ai0: PreviousIdentity,
                        ai1: -1,
                        ai2: 1f
                    );
                    if(PreviousIdentity != -1)
                    {
                        for(int i = 0; i < Main.maxProjectiles; i++)
                            if (Main.projectile[i].identity == PreviousIdentity)
                            {
                                Main.projectile[i].ai[1] = Main.projectile[previousTrailPoint].identity;
                                break;
                            }
                    }
                    PreviousIdentity = Main.projectile[previousTrailPoint].identity;
                    foreach (int i in sparkyTrail)
                    {
                        Main.projectile[i].ai[2] -= 0.175f;
                        Main.projectile[i].timeLeft = 300;
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, i);
                    }
                    sparkyTrail.Add((int)previousTrailPoint);
                    if (sparkyTrail.Count > 5)
                    {
                        Main.projectile[sparkyTrail[0]].timeLeft = 30;
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, sparkyTrail[0]);
                        sparkyTrail.RemoveAt(0);
                    }
                }
                CurrentState = (float)SparkyAIState.Aim;
                StateTimer = AimDuration;
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return CanHitTimer > DashDuration + AimDuration && base.CanHitPlayer(target, ref cooldownSlot);
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (NPC.life <= 0)
                return;
            SoundEngine.PlaySound(SoundID.NPCHit36, NPC.Center);
            base.OnHitByItem(player, item, hit, damageDone);
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (NPC.life <= 0)
                return;
            SoundEngine.PlaySound(SoundID.NPCHit36, NPC.Center);
            base.OnHitByProjectile(projectile, hit, damageDone);
        }
        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i = 0; i < sparkyTrail.Count; i++)
                {
                    Main.projectile[sparkyTrail[i]].timeLeft = 300 + (i + 1) * 30;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderShard>(), 1, 1, 2));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = NPC.frame.Size() / 2f;

            Color color = Color.White;
            Vector2 offset = Vector2.Zero;

            switch (CurrentState)
            {
                case (float)SparkyAIState.Aim:
                    color = new Color(1f, 1f, 1f, MathHelper.Lerp(0.15f, 0.35f, 1f - StateTimer / AimDuration));
                    break;
                case (float)SparkyAIState.Prepare:
                    color = new Color(1f, 1f, 1f, 0.35f);
                    float cloneScale = MathHelper.Lerp(2.125f, 1f, 1f - StateTimer / PrepareDuration);
                    Color cloneColor = new Color(1f, 1f, 1f, 0.35f) * (1f - StateTimer / PrepareDuration);
                    spriteBatch.Draw(
                        texture,
                        NPC.Center - screenPos,
                        NPC.frame,
                        cloneColor,
                        NPC.rotation - MathHelper.PiOver2,
                        origin,
                        cloneScale,
                        SpriteEffects.None,
                        0f
                    );
                    break;
                case (float)SparkyAIState.Dash:
                    color = new Color(1f, 1f, 1f, 0.75f);
                    offset = Main.rand.NextVector2Unit() * 2.5f;
                    break;
            }

            spriteBatch.Draw(
                texture,
                NPC.Center + offset - screenPos,
                NPC.frame,
                color,
                NPC.rotation - MathHelper.PiOver2,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );
            return false;
        }
    }
}
