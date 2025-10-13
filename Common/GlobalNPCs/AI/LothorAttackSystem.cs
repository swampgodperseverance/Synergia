using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Hostile;
using Terraria.ID;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class LothorAttackSystem : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        private int _attackCooldown = 0;
        private bool _warningActive = false;
        private Vector2 _attackOrigin;
        private Vector2 _attackTarget;
        private int? _lothorTypeCache = null;
        private const int ProjectilesCount = 5;
        // DefensiveRing
        private int _defensiveRingID = -1;
        private bool _lowHPPhase = false;
        private int _fastAttackTimer = 0;

        public override void AI(NPC npc)
        {
            if (!IsRoALothor(npc)) return;

            if (npc.life < npc.lifeMax * 0.25f && !_lowHPPhase)
            {
                _lowHPPhase = true;
                SpawnDefensiveRing(npc);
                CreatePhaseTransitionEffect(npc);
            }

            if (_defensiveRingID != -1 && !Main.projectile[_defensiveRingID].active)
            {
                _defensiveRingID = -1;
            }

            if (_lowHPPhase)
            {
                _fastAttackTimer++;
                if (_fastAttackTimer >= 120 && npc.HasValidTarget)
                {
                    StartBloodAttack(npc, isFastPhase: true);
                    _fastAttackTimer = 0;
                }
            }
            else if (npc.life < npc.lifeMax * 0.5f)
            {
                if (_attackCooldown <= 0 && !_warningActive && npc.HasValidTarget)
                {
                    StartBloodAttack(npc);
                    _attackCooldown = Main.rand.Next(500, 600);
                    _warningActive = true;
                }
                _attackCooldown--;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (IsRoALothor(npc) && npc.life < npc.lifeMax * 0.25f)
            {
                modifiers.FinalDamage *= 0.80f; 
            }
        }

        private void SpawnDefensiveRing(NPC boss)
        {
            _defensiveRingID = Projectile.NewProjectile(
                boss.GetSource_FromAI(),
                boss.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DefensiveRing>(),
                boss.damage / 2,
                2f,
                Main.myPlayer,
                boss.whoAmI
            );

            Projectile ring = Main.projectile[_defensiveRingID];
            ring.timeLeft = 99999;
            ring.scale = 1.3f;
            ring.hostile = true;
            ring.friendly = false;
        }

        private void StartBloodAttack(NPC boss, bool isFastPhase = false)
        {
            _attackOrigin = FindSolidTile(boss.Center);
            _attackTarget = Main.player[boss.target].Center;

            CreateWarningLine(_attackOrigin, _attackTarget);

            TimerSystem.DelayAction(60, () => 
            {
                if (!boss.active || boss.life <= 0) return;

                Vector2 direction = (_attackTarget - _attackOrigin).SafeNormalize(Vector2.UnitX);
                float speed = isFastPhase ? 18f : 12f;
                int damage = isFastPhase ? boss.damage / 3 : boss.damage / 4;

                for (int i = 0; i < ProjectilesCount; i++)
                {
                    TimerSystem.DelayAction(i * (isFastPhase ? 3 : 5), () => 
                    {
                        Vector2 spreadDirection = direction.RotatedByRandom(MathHelper.ToRadians(15));
                        Projectile.NewProjectile(
                            boss.GetSource_FromAI(),
                            _attackOrigin,
                            spreadDirection * (speed + Main.rand.NextFloat(-1f, 1f)),
                            ModContent.ProjectileType<PrimordialBlood>(),
                            damage,
                            2f,
                            Main.myPlayer
                        );
                    });
                }
                _warningActive = false;
            });
        }

        private void CreatePhaseTransitionEffect(NPC npc)
        {
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDustPerfect(
                    npc.Center,
                    DustID.LifeDrain,
                    Main.rand.NextVector2Circular(10, 10),
                    0,
                    Color.DarkRed,
                    2.5f
                ).noGravity = true;
            }
        }

        private bool IsRoALothor(NPC npc)
        {
            if (_lothorTypeCache == null)
            {
                Mod mod = ModLoader.GetMod("RoA");
                _lothorTypeCache = mod?.Find<ModNPC>("Lothor")?.Type ?? -1;
            }
            return _lothorTypeCache > 0 && npc.type == _lothorTypeCache;
        }

        private Vector2 FindSolidTile(Vector2 center)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 testPos = center + new Vector2(Main.rand.Next(-400, 400), Main.rand.Next(-200, 200));
                if (WorldGen.SolidTile(Framing.GetTileSafely((int)testPos.X / 16, (int)testPos.Y / 16)))
                {
                    return testPos;
                }
            }
            return center;
        }

        private void CreateWarningLine(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;
            float distance = direction.Length();
            direction.Normalize();

            for (float i = 0; i < distance; i += 16f)
            {
                Vector2 pos = start + direction * i;
                Dust.NewDustPerfect(pos, DustID.LifeDrain, Vector2.Zero, 0, Color.Red, 1.5f).noGravity = true;
            }
        }
    }
}