using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.NPCs
{
    public class CogwormBody : ModNPC
    {
        private int segmentIndex = 0;
        private Vector2[] oldPositions = new Vector2[5];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cogworm Body");
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 3000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;

        }

        public override void AI()
        {
            if (segmentIndex == 0)
            {
                segmentIndex = (int)NPC.ai[2] + 1;
            }

            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                oldPositions[i] = oldPositions[i - 1];
            }
            oldPositions[0] = NPC.Center;

            NPC.realLife = (int)NPC.ai[3];
            int prev = (int)NPC.ai[1];

            if (prev >= 0 && Main.npc[prev].active)
            {
                Vector2 center = Main.npc[prev].Center;
                float spacing = NPC.width * 0.85f;

                Vector2 direction = center - NPC.Center;
                float distance = direction.Length();

                if (distance > spacing)
                {
                    direction.Normalize();
                    direction *= (distance - spacing);
                    NPC.position += direction;
                }

                NPC.velocity = Vector2.Zero;
                NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;


                if (Main.rand.NextBool(10))
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 0f, 100, default, 0.8f);
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }
                if (segmentIndex > 5 && segmentIndex < 10 && Main.rand.NextBool(60))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int headIndex = (int)NPC.ai[3];
                        if (Main.npc.IndexInRange(headIndex) && Main.npc[headIndex].ModNPC is Cogworm headNPC)
                        {
                            if (headNPC.attackPhase == 0) 
                            {
                                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                                Vector2 dir = angle.ToRotationVector2();

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dir * 5f,
                                    ProjectileID.Fireball, 20, 1f, Main.myPlayer);
                            }
                        }
                    }
                }
            }
            else
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                float alpha = 0.4f * (1f - (float)i / oldPositions.Length);
                spriteBatch.Draw(texture, oldPositions[i] - screenPos, null, drawColor * alpha, 
                    NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }

            return true;
        }
    }
}