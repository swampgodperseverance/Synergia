using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using System.Collections.Generic;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class BurningAura : ModProjectile
	{
		public override void SetStaticDefaults() => Terraria.ID.ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2048;
		public override void SetDefaults() {
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Main.netMode != 2) {
				Projectile.Center = Main.LocalPlayer.Center;
				int l = Dust.NewDust(Main.screenPosition + new Vector2(Main.rand.Next(Main.screenWidth + 1), Main.screenHeight), 0, 0, 6);
				Main.dust[l].noGravity = true;
				Main.dust[l].scale *= 2.1f;
				Main.dust[l].velocity = Vector2.UnitY * -Main.rand.Next(16, 65);
			}
			if(Projectile.ai[0] < 15f) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
			if(--Projectile.ai[0] <= 0f) Projectile.Kill();
			else Projectile.timeLeft = 2;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.DarkOrange * MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f) * 0.2f * Projectile.Opacity;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Vector2.Zero, null, lightColor, 0f, Vector2.Zero, Main.ScreenSize.ToVector2(), SpriteEffects.None, 0);
			if(!Filters.Scene["HeatDistortion"].IsActive() && Main.UseHeatDistortion && Projectile.alpha == 0) Filters.Scene.Activate("HeatDistortion", default(Vector2));
			if(Filters.Scene["HeatDistortion"].IsActive()) {
				Filters.Scene["HeatDistortion"].GetShader().UseIntensity(Projectile.Opacity);
				Filters.Scene["HeatDistortion"].IsHidden = false;
			}
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overWiresUI.Add(index);
	}
}