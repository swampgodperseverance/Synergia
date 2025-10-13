using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic; // ✅ нужно для List и Dictionary
using Terraria.Audio;
using Synergia.Content.Projectiles.Hostile;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems;
using Synergia.Common;
using Synergia.Content.Items;
using Synergia.Content.NPCs;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.Weapons.Cogworm;
using ValhallaMod.Items.Placeable.Blocks;
using Terraria;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Synergia.Content
{
    public class SynergiaSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            DoBossChecklistIntegration();
        }

        private void DoBossChecklistIntegration()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
                return;

            if (bossChecklistMod.Version < new Version(1, 6))
                return;

            string internalName = nameof(Cogworm);
            float weight = 12.5f; // прогрессия босса

            Func<bool> downed = () => DownedBossSystem.DownedSinlordBoss; 

            int spawnItem = ModContent.ItemType<Content.Items.Misc.HellwormScale>();
            int bossType = ModContent.NPCType<Cogworm>();

            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<CogwormRelicItem>(),
                ModContent.ItemType<CogwormTrophy>()
            };

            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("Synergia/Content/NPCs/CogwormDash").Value;
                Vector2 centered = new Vector2(
                    rect.X + (rect.Width / 2) - (texture.Width / 2),
                    rect.Y + (rect.Height / 2) - (texture.Height / 2)
                );
                sb.Draw(texture, centered, color);
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod, // текущий мод
                internalName,
                weight,

                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = new List<int> { spawnItem },
                    ["collectibles"] = collectibles,
                    ["spawnInfo"] = Language.GetOrRegister("Use Hellworm Scale in the underworld"),
                    ["customPortrait"] = customPortrait
                }
            );
        }
    }
}