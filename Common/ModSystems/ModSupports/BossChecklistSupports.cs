using Synergia.Content.Items.Misc;
using Synergia.Content.NPCs.Boss.SinlordWyrm;
using Synergia.Content.NPCs.Swamp;
using System;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.ModSystems.ModSupports {
    public class BossChecklistSupports : ModSupportsSystem {
        static readonly string[] type = ["LogBoss", "LogMiniBoss", "LogEvent"];

        public override Mod TargetMod() { ModLoader.TryGetMod("BossChecklist", out Mod mod); return mod; }
        public override void PostSetupContent(Mod bossChecklistMod) {
            if (bossChecklistMod.Version < new Version(1, 6)) { return; }
            RegisterInBossChecklist(bossChecklistMod, type[0], nameof(Sinlord), 12.5f, () => DownedBossSystem.DownedSinlordBoss, NPCType<Sinlord>(), ItemType<HellwormScale>(), CustomPortrait("Sinlord", 1f));
            RegisterInBossChecklist(bossChecklistMod, type[1], nameof(MossWitch), 5.4f, () => DownedBossSystem.CompleteNewFrostEvent, NPCType<MossWitch>(), -1, CustomPortrait("Cruor", 1f));
        }
        void RegisterInBossChecklist(Mod bossChecklistMod, string type, string internalName, float weight, Func<bool> downed, int bossType, int spawnItem) {
            if (spawnItem > -1) { bossChecklistMod.Call(type, Mod, internalName, weight, downed, bossType, new Dictionary<string, object>() { ["spawnItems"] = new List<int> { spawnItem } }); }
            else { bossChecklistMod.Call(type, Mod, internalName, weight, downed, bossType); }
        }
        void RegisterInBossChecklist(Mod bossChecklistMod, string type, string internalName, float weight, Func<bool> downed, int bossType, int spawnItem, Action<SpriteBatch, Rectangle, Color> customPortrait) {
            bossChecklistMod.Call(type, Mod, internalName, weight, downed, bossType, new Dictionary<string, object>() { ["spawnItems"] = new List<int> { spawnItem }, ["customPortrait"] = customPortrait});
        }   
        static Action<SpriteBatch, Rectangle, Color> CustomPortrait(string name, float size) {
            return (sb, rect, color) => { Texture2D texture = Request<Texture2D>($"Synergia/Assets/Textures/{name}").Value; 
                Vector2 centered = new(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f); 
                sb.Draw(texture, centered, null, color, 0f, texture.Size() / 2f, size, SpriteEffects.None, 0f); 
            };
        }
    }
}