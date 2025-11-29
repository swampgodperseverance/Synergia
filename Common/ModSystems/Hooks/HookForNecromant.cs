using Bismuth.Content.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Synergia.Content.Projectiles.Hostile.Bosses;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.Hooks
{
    public class HookForNecromant : ModSystem
    {
        Hook newAiForNecromant;
        FieldInfo currentPhaseField;
        static readonly Dictionary<int, int> attackTimers = [];

        public override void Load() {
            Type type = typeof(EvilNecromancer);

            currentPhaseField = type.GetField("currentphase", BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo target = type.GetMethod(nameof(EvilNecromancer.AI), BindingFlags.Instance | BindingFlags.Public);
            newAiForNecromant = new Hook(target, (GetSetAI)NewNecromancerAI);
        }
        public override void Unload() {
            newAiForNecromant?.Dispose();
            newAiForNecromant = null;
            currentPhaseField = null;
            attackTimers.Clear();
        }

        delegate void Orig_SetAI(EvilNecromancer npc);
        delegate void GetSetAI(Orig_SetAI orig, EvilNecromancer npc);

        void NewNecromancerAI(Orig_SetAI orig, EvilNecromancer npc)
        {
            orig(npc);

            if (!npc.NPC.active) {
                return;
            }

            int id = npc.NPC.whoAmI;
            int currentphase = (int)currentPhaseField.GetValue(npc);

            if (!attackTimers.TryGetValue(id, out int value)) {
                value = 0;
                attackTimers[id] = value;
            }

            attackTimers[id] = ++value;

            Vector2 spawnPos = npc.NPC.Center;
            int damage = 40;
            float knockBack = 2f;
            int owner = Main.myPlayer;

            if (currentphase == 2) {

                if (attackTimers[id] >= 90) {
                    for (int i = 0; i < 2; i++) {
                        int projType = ModContent.ProjectileType<NecroSphere>();
                        Vector2 velocity = new(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-8f, -5f));
                        Projectile.NewProjectileDirect(npc.NPC.GetSource_FromAI(), spawnPos, velocity, projType, damage, knockBack, owner);
                    }
                    attackTimers[id] = 0;
                }
            }
            else if (currentphase == 3) {

                if (attackTimers[id] >= 120) {
                    int projType = ModContent.ProjectileType<NecroSkull>();
                    Vector2 velocity = new(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-7f, -4f));
                    Projectile.NewProjectileDirect(npc.NPC.GetSource_FromAI(), spawnPos, velocity, projType, damage, knockBack, owner);
                    attackTimers[id] = 0;
                }
            }
            else {
                attackTimers[id] = 0;
            }
        }
    }
}