using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class FireWhirl : ModProjectile
	{
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0f) {
				Projectile.localAI[0] = Projectile.ai[1];
				if(Projectile.velocity.X != 0) Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0 ? -1 : 1);
			}
			if(Projectile.ai[0] > 0f) {
				NPC npc = Main.npc[(int)Projectile.ai[0] - 1];
				if(npc.ModNPC is Content.NPCs.Boss.SinlordWyrm.Sinlord boss) {
					Projectile.spriteDirection = Projectile.direction = (int)-npc.ai[2];
					Projectile.Center = boss.storedPos;
				}
				else Projectile.Center = npc.Center + npc.rotation.ToRotationVector2() * (npc.width * 0.4f + (1f - Projectile.ai[1] / Projectile.localAI[0]) * Projectile.ai[2] * 21f) - Projectile.velocity - Vector2.UnitY * 2;
				if(--Projectile.ai[1] == 0f) {
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
					Projectile.ai[0] = 0f;
				}
				Projectile.timeLeft++;
			}
			else {
				Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Main.rand.NextVector2CircularEdge(16f, 16f), 8f, 10, 30, 2400f, "Sinlord Screenshake"));
				int amount = 18 - Projectile.timeLeft;
				for(int i = 0; i < amount; i++) Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Vector2.UnitY.RotatedBy(i * MathHelper.TwoPi / amount) * (amount - 1) * 30f, Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), Projectile.damage, 0f, Main.myPlayer);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f;
			float scale = Projectile.scale * (1f - Projectile.ai[1] / Projectile.localAI[0]) * Projectile.ai[2];
			for(int k = 0; k < 20; k++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * fade * 0.5f, (Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / (k / 20f + 1) * 7.5f - MathHelper.ToRadians(k * 36f)) * Projectile.spriteDirection, texture.Size() * 0.5f, scale * 0.075f * k * fade, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			float size = (Projectile.width + Projectile.height) / 4 * (float)MathHelper.Min(15, Projectile.timeLeft) / 15f * Projectile.scale * (1f - Projectile.ai[1] / Projectile.localAI[0]) * Projectile.ai[2];
			hitbox = new Rectangle((int)(Projectile.Center.X - size), (int)(Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
		public override bool CanHitPlayer(Player target) => Projectile.Distance(target.MountedCenter) <= (Projectile.width + Projectile.height) / 4 * (float)MathHelper.Min(15, Projectile.timeLeft) / 15f * Projectile.scale * (1f - Projectile.ai[1] / Projectile.localAI[0]) * Projectile.ai[2];
	}
}