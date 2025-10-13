using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Accessories
{
    public class PreciousSkull : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
         
            player.GetModPlayer<PreciousSkullPlayer>().hasPreciousSkull = true;

         
            player.GetDamage(DamageClass.Throwing) += 0.08f;
            player.GetCritChance(DamageClass.Throwing) += 10;

            int minuteTimer = (int)(Main.GameUpdateCount / 3600 % 2); 
         
            if (minuteTimer == 0)
                player.lifeRegen += 3;
            else
                player.statDefense += 10;

         
            bool alreadySpawned = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<PreciousSkullAura>())
                {
                    alreadySpawned = true;
                    proj.ai[0] = minuteTimer;
                    break;
                }
            }

            if (!alreadySpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<PreciousSkullAura>(),
                    0,
                    0,
                    player.whoAmI,
                    minuteTimer 
                );
            }

          
            if (player.dead)
            {
                player.GetModPlayer<PreciousSkullPlayer>().deathTimer = 3600;
            }
        }
    }

    public class PreciousSkullPlayer : ModPlayer
    {
        public int deathTimer;
        public bool hasPreciousSkull;

        public override void ResetEffects()
        {
            hasPreciousSkull = false;

            if (deathTimer > 0)
            {
                deathTimer--;
                Player.GetAttackSpeed(DamageClass.Throwing) += 0.10f;
            }
        }
    }

    public class PreciousSkullAura : ModProjectile
    {
        private const int FadeSpeed = 5;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.GetModPlayer<PreciousSkullPlayer>().hasPreciousSkull)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Center + new Vector2(0, -40);

            int targetPhase = (int)Projectile.ai[0]; 

       
            if (Projectile.localAI[0] != targetPhase)
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += FadeSpeed;
                    if (Projectile.alpha > 255) Projectile.alpha = 255;
                }
                else
                {
      
                    Projectile.localAI[0] = targetPhase;
                }
            }
            else
            {
       
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= FadeSpeed;
                    if (Projectile.alpha < 0) Projectile.alpha = 0;
                }
            }

            Projectile.timeLeft = 60; 
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            if ((int)Projectile.localAI[0] == 0)
                texture = ModContent.Request<Texture2D>("Synergia/Content/Items/Accessories/Tourmaline").Value;
            else
                texture = ModContent.Request<Texture2D>("Synergia/Content/Items/Accessories/Zircon").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, drawPos, null,
                Color.White * (1f - Projectile.alpha / 255f),
                0f, texture.Size() / 2, 1f, SpriteEffects.None, 0);

            return false;
        }
    }
}
