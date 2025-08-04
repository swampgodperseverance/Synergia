using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vanilla.Content.Projectiles.Hostile;
using Vanilla.Content.NPCs;
using Vanilla;
using Vanilla.Common.GlobalPlayer;

namespace Vanilla.Content.NPCs
{
    public class CogwormTail : ModNPC
    {
        private Vector2[] oldPositions = new Vector2[5];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WyvernTail];
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 20;
            NPC.defense = 8;
            NPC.lifeMax = 2000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.HitSound = null;
            NPC.rotation = MathHelper.PiOver2; 
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            PlayRandomHitSound();
            base.OnHitByItem(player, item, hit, damageDone);
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            PlayRandomHitSound();
            base.OnHitByProjectile(projectile, hit, damageDone);
        }

        private void PlayRandomHitSound()
        {
            if (Main.rand.NextBool())
            {
                SoundEngine.PlaySound(Sounds.CragwormHit with { Volume = 0.8f, Pitch = -0.1f }, NPC.Center);
            }
            else
            {
                SoundEngine.PlaySound(Sounds.CragwormHit2 with { Volume = 0.8f, Pitch = -0.1f }, NPC.Center);
            }
        }

        public override void AI()
        {
 
            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                oldPositions[i] = oldPositions[i - 1];
            }
            oldPositions[0] = NPC.Center;

            NPC.realLife = (int)NPC.ai[3];
            int prev = (int)NPC.ai[1];

            if (prev >= 0 && Main.npc[prev].active)
            {
                Vector2 direction = Main.npc[prev].Center - NPC.Center;
                float distance = direction.Length();
                float spacing = NPC.width * 0.8f;

                if (distance > spacing)
                {
                    direction.Normalize();
                    NPC.position += direction * (distance - spacing);
                }

                NPC.velocity = Vector2.Zero;
                
                Vector2 dirToPrev = Main.npc[prev].Center - NPC.Center;
                NPC.rotation = dirToPrev.ToRotation() + MathHelper.PiOver2; 

                if (Main.rand.NextBool(8))
                {
                    CreateDustEffect();
                }
            }
            else
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
            }
        }

        private void CreateDustEffect()
        {
            Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 
                0f, 0f, 100, default, 0.6f);
            dust.noGravity = true;
            dust.velocity *= 0.2f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                float alpha = 0.3f * (1f - (float)i / oldPositions.Length);
                spriteBatch.Draw(texture, oldPositions[i] - screenPos, null, 
                    drawColor * alpha, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, null, 
                drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}