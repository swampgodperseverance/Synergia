using Bismuth.Content.Items.Other;
using Microsoft.Xna.Framework;
using Synergia.Common.WorldGenSystem;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalTiles
{
    public class BiomeBlock : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (GenerationSnowVillage.VilageTiles.Contains(new Vector2(i, j)) && !Main.LocalPlayer.HasItem(ModContent.ItemType<MasterToolBox>())) return false;
            else return base.CanKillTile(i, j, type, ref blockDamaged);
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if (GenerationSnowVillage.VilageTiles.Contains(new Vector2(i, j)) && !Main.LocalPlayer.HasItem(ModContent.ItemType<MasterToolBox>())) return false;
            else return base.CanExplode(i, j, type);
        }
    }
}