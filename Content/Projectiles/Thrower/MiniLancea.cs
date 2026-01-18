﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class MiniLancea : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
        }

        const int dust_count = 5;

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int counter = 0; counter < dust_count; counter++)
            {
                Vector2 velocity = Projectile.velocity * ((float)Main.rand.NextFloat(0.2f, 0.8f));

                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.Iron, 8, 8, 100, default(Color), 0.8f);
                Main.dust[dust].noLight = true;
                Main.dust[dust].velocity = velocity;
            }
        }
    }
}