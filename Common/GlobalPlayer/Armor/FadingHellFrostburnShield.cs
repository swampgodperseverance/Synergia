
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Common.GlobalPlayer.Armor
{
    public class FadingHellFrostburnShield : ModPlayer
    {
        public bool IsActive = false;
        public bool WasHitInThisFrame = false;

        public readonly int ShieldMaxProgress = 12 * 60;
        public int ShieldProgress = 0;

        public readonly float DashSpeed = 35f;
        public readonly int DashCooldown = 45;
        public readonly int DashDuration = 50;
        public int DashDirection = 0;
        public int DashTimer = 0;
        public int DashDelay = 0;
        public override void Initialize()
        {
            IsActive = false;
        }
        public override void ResetEffects()
        {
            if (!IsActive)
            {
                ShieldProgress = 0;
                return;
            }
            IsActive = false;
            DashDirection = 0;

            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15 && Player.doubleTapCardinalTimer[3] == 0)
                DashDirection = 1;
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15 && Player.doubleTapCardinalTimer[2] == 0)
                DashDirection = -1;
        }
        public override void PreUpdateMovement()
        {
            if (!(IsActive && ShieldProgress == ShieldMaxProgress))
            {
                DashDelay = 0;
                DashTimer = 0;
                return;
            }

            Player.dash = 0;
            Player.dashTime = -1;
            if (DashDirection != 0 && DashDelay == 0 && !Player.mount.Active)
            {
                Vector2 newVelocity = Player.velocity;
                if (Player.velocity.X > -DashSpeed || Player.velocity.X < DashSpeed)
                {
                    newVelocity.X = DashSpeed * DashDirection;
                }
                else
                    return;

                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;
            }
            if (DashDelay > 0)
                DashDelay--;
            if (DashTimer > 0)
            {
                Player.armorEffectDrawShadowEOCShield = true;
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.IceTorch, 0, 0, 180, default, 1.25f);
                DashTimer--;
            }
        }
        public override void PreUpdate()
        {
            if (!(IsActive && DashTimer > 0)) return;

            // this code was taken from shield of cthulhu
            Rectangle rectangle = new Rectangle((int)(Player.position.X + Player.velocity.X * 0.5 - 4.0), (int)(Player.position.Y + Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.dontTakeDamage || npc.friendly || (npc.aiStyle == NPCAIStyleID.Fairy && !(npc.ai[2] <= 1f)) || !Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc))
                    continue;
                Rectangle rect = npc.getRect();
                if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                {
                    Player.immune = true;
                    Player.immuneTime += 120;
                    Player.statMana = Player.statManaMax2;
                    DashTimer = 0;
                    ShieldProgress = 0;
                    BreakingShieldVFX(1f);
                    SoundEngine.PlaySound(SoundID.Item62, Player.Center);
                    if (Main.myPlayer == Player.whoAmI)
                        Projectile.NewProjectile(
                            Player.GetSource_FromAI(),
                            Player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<FadingHellFrostburnShieldExplosion>(),
                            5000,
                            15f,
                            Main.myPlayer
                        );
                    break;
                }
            }
        }
        public override void PostUpdate()
        {
            if (!IsActive) return;

            if (ShieldProgress >= ShieldMaxProgress) return;
            ShieldProgress++;
            if(ShieldProgress == ShieldMaxProgress)
            {
                if (Main.myPlayer != Player.whoAmI) return;
                SoundEngine.PlaySound(SoundID.MaxMana);
                for(int i = 0; i < 3; i++)
                    Dust.NewDust(Player.position, Player.width, Player.height, DustID.IceTorch, 0, 0, 200, default, 1.25f);
            }
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (!IsActive) return;
            ModifyHit(ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (!IsActive) return;
            ModifyHit(ref modifiers);
        }
        private void ModifyHit(ref Player.HurtModifiers modifiers)
        {
            float progress = (float)ShieldProgress / ShieldMaxProgress;
            modifiers.FinalDamage *= 1f - progress;
            modifiers.Knockback *= 1f - progress;
            BreakingShieldVFX(progress);
            WasHitInThisFrame = ShieldProgress > 0;
            ShieldProgress = 0;
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (info.Damage == 1 && !info.PvP && WasHitInThisFrame)
                Player.immuneTime *= 2;
            WasHitInThisFrame = false;
        }
        private void BreakingShieldVFX(float progress)
        {
            SoundEngine.PlaySound(SoundID.Item27, Player.Center);
            int maxDust = (int)(20 * progress);
            for (int i = 0; i < maxDust; i++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.IceTorch, 0, 0, 180, default, 2f);
            }
        }
    }
    public class FadingHellFrostburnShieldExplosion : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Glow";
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 15;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = Projectile.timeLeft + 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Default;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.BrokenArmor, 20 * 60, false);
            target.AddBuff(BuffID.Frostburn2, 10 * 60, false);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;
            // it's not correct code but it looks cool so i'll stay it
            float progress = 1f - Projectile.timeLeft / 10f;
            Color color = Color.Lerp(new Color(5, 217, 250, 127), Color.Transparent, progress);
            Main.EntitySpriteDraw(
                texture,
                position,
                null,
                color,
                0f,
                origin,
                1.5f * progress,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
    public class FadingHellFrostburnShieldDraw : PlayerDrawLayer
    {
        Asset<Texture2D> frozenAsset;
        public override void Load()
        {
            frozenAsset = ModContent.Request<Texture2D>("Synergia/Assets/Textures/FrostburnShield");
        }
        public override void Unload()
        {
            frozenAsset = null;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.hideEntirePlayer)
                return;

            Player player = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || player.dead || drawInfo.headOnlyRender || Main.gameMenu)
                return;

            Texture2D frozenTexture = frozenAsset.Value;

            FadingHellFrostburnShield data = player.GetModPlayer<FadingHellFrostburnShield>();
            float progress = (float)data.ShieldProgress / data.ShieldMaxProgress;

            int frameHeight = frozenTexture.Height / 6;
            Rectangle frame = new Rectangle(0, frameHeight * (int)(5 * progress), frozenTexture.Width, frameHeight);

            float x = drawInfo.Position.X + player.width / 2;
            float y = drawInfo.Position.Y + player.height / 2;
            Vector2 position = new Vector2(x, y) - Main.screenPosition;
            Vector2 origin = frame.Size() / 2f;

            float scale = 0.75f + progress * 0.25f;
            Color color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 40), progress);

            DrawData drawData = new DrawData(
                frozenTexture,
                position,
                new Rectangle?(frame),
                color,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0
            );
            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}
