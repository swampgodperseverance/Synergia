using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Synergia.Content.Projectiles.Hostile  ;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class CacodemonAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        
        private int dashTimer = 0;
        private bool isDashing = false;
        private Vector2 dashDirection = Vector2.Zero;
        private int attackCooldown = 180;
        private int bloodFireCooldown = 0;

        public override void SetDefaults(NPC npc)
        {

            if (npc.type == ModLoader.GetMod("ValhallaMod").Find<ModNPC>("Cacodemon").Type)
            {

            }
        }

        public override void AI(NPC npc)
        {

            if (npc.type != ModLoader.GetMod("ValhallaMod").Find<ModNPC>("Cacodemon").Type)
                return;


            if (attackCooldown > 0)
                attackCooldown--;

            if (bloodFireCooldown > 0)
                bloodFireCooldown--;


            Player target = Main.player[npc.target];
            if (target.active && !target.dead && attackCooldown <= 0 && bloodFireCooldown <= 0)
            {
                float distanceToTarget = Vector2.Distance(npc.Center, target.Center);
                

                if (distanceToTarget < 400f)
                {
                    StartDashAttack(npc, target);
                    attackCooldown = 180;
                    bloodFireCooldown = 60; 
                }
            }

            if (isDashing)
            {
                dashTimer++;

                if (dashTimer >= 30)
                {
                    EndDash(npc);
                }
                else
                {

                    npc.velocity = dashDirection * 12f;

                    if (dashTimer == 15)
                    {
                        ShootBloodFireProjectiles(npc);
                    }
                }
            }
        }

        private void StartDashAttack(NPC npc, Player target)
        {
            isDashing = true;
            dashTimer = 0;
            dashDirection = Vector2.Normalize(target.Center - npc.Center);

            SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.8f, Pitch = 0.5f }, npc.Center);
            
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 
                    DustID.Blood, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = dashDirection.RotatedByRandom(MathHelper.PiOver4) * 5f;
            }
        }

        private void EndDash(NPC npc)
        {
            isDashing = false;
            dashTimer = 0;
            dashDirection = Vector2.Zero;

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 
                    DustID.Blood, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
        }

        private void ShootBloodFireProjectiles(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * (i / 6f);
                Vector2 direction = angle.ToRotationVector2();

                Vector2 spawnPosition = npc.Center + direction * 48f;
                

                int projectile = Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    spawnPosition,
                    direction * 3f, 
                    ModContent.ProjectileType<BloodFire>(), 
                    40, 
                    2f, 
                    Main.myPlayer);

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile);
                }
            }

            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.7f }, npc.Center);

            for (int i = 0; i < 20; i++)
            {
                float angle = MathHelper.TwoPi * (i / 20f);
                Vector2 direction = angle.ToRotationVector2();
                
                Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, 
                    DustID.Blood, direction.X * 4f, direction.Y * 4f, 100, default, 2f);
                dust.noGravity = true;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == ModLoader.GetMod("ValhallaMod").Find<ModNPC>("Cacodemon").Type && isDashing)
            {
                modifiers.FinalDamage *= 0.7f;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (isDashing && dashDirection != Vector2.Zero)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ProjectileID.RubyBolt].Value;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                Vector2 drawPos = npc.Center - screenPos;
                
                spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    Color.Red * 0.5f,
                    dashDirection.ToRotation(),
                    origin,
                    0.5f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}