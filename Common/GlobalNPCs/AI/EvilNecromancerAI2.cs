using System;
using System.Collections.Generic;
using System.Reflection;
using Bismuth.Content.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Synergia.Common.GlobalNPCs.AIs;
using Synergia.Content.Projectiles.Hostile.Bosses;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.Hooks
{
    public class HookForNecromant2 : ModSystem
    {
        private Hook newAiForNecromant;
        private FieldInfo currentPhaseField;


        private static readonly Dictionary<int, int> attackTimers = new();

        public override void Load()
        {
            Type type = typeof(EvilNecromancer);

            currentPhaseField = type.GetField("currentphase", BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo target = type.GetMethod(nameof(EvilNecromancer.AI), BindingFlags.Instance | BindingFlags.Public);
            newAiForNecromant = new Hook(target, (GetSetAI)NewNecromancerAI);
        }

        public override void Unload()
        {
            newAiForNecromant?.Dispose();
            newAiForNecromant = null;
            currentPhaseField = null;
            attackTimers.Clear();
        }

        private delegate void Orig_SetAI(EvilNecromancer npc);
        private delegate void GetSetAI(Orig_SetAI orig, EvilNecromancer npc);

        private void NewNecromancerAI(Orig_SetAI orig, EvilNecromancer npc)
        {
            orig(npc);
            if (EvilNecromancerAI.Disabled) return;
            if (!npc.NPC.active)
                return;

            int id = npc.NPC.whoAmI;
            int currentphase = (int)currentPhaseField.GetValue(npc);

            if (!attackTimers.ContainsKey(id))
                attackTimers[id] = 0;

            attackTimers[id]++;

            Vector2 spawnPos = npc.NPC.Center;
            int damage = 20;
            float knockBack = 2f;
            int owner = Main.myPlayer;

            float healthPercent = (float)npc.NPC.life / npc.NPC.lifeMax;

            if (currentphase == 2)
            {
                if (attackTimers[id] >= 90)
                {
                    int sphereCount = 1;

                    if (healthPercent <= 0.4f && Main.rand.NextFloat() < 0.5f)
                    {
                        sphereCount = 2;
                    }

                    for (int i = 0; i < sphereCount; i++)
                    {
                        int projType = ModContent.ProjectileType<NecroSphere>();
                        float xOffset = Main.rand.NextFloat(-20f, 20f);
                        float yOffset = Main.rand.NextFloat(-10f, 10f);
                        Vector2 spawnOffset = new Vector2(xOffset, yOffset);

                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-8f, -5f));

                        if (sphereCount == 2 && i == 1)
                        {
                            velocity = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-6f, -3f));
                        }

                        Projectile.NewProjectileDirect(npc.NPC.GetSource_FromAI(), spawnPos + spawnOffset, velocity, projType, damage, knockBack, owner);
                    }
                    attackTimers[id] = 0;
                }
            }
            else if (currentphase == 3)
            {
                if (attackTimers[id] >= 120)
                {
                    int projType = ModContent.ProjectileType<NecroSkull>();
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-7f, -4f));
                    Projectile.NewProjectileDirect(npc.NPC.GetSource_FromAI(), spawnPos, velocity, projType, damage, knockBack, owner);
                    attackTimers[id] = 0;
                }
            }
            else
            {
                attackTimers[id] = 0;
            }
        }
    }
}