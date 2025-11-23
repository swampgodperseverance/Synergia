using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO; // Добавь эту строку для TagCompound
using Synergia.Content.Projectiles.Reworks.Reworks2;
using System.Collections.Generic;
namespace Synergia.Common.Players
{
    public class EverIcePlayer : ModPlayer
    {

        public int everIceHitCount = 0;
        public bool everIceEmpowered = false;

        public override void ResetEffects()
        {

        }


        public override void SaveData(TagCompound tag)
        {
            tag["everIceHitCount"] = everIceHitCount;
            tag["everIceEmpowered"] = everIceEmpowered;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("everIceHitCount")) everIceHitCount = tag.GetInt("everIceHitCount");
            if (tag.ContainsKey("everIceEmpowered")) everIceEmpowered = tag.GetBool("everIceEmpowered");
        }
    }
}
