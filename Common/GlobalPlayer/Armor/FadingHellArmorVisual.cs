using Synergia.Content.Items.Armor.Magic.FadingHell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Common.GlobalPlayer.Armor
{
    public class FadingHellArmorVisual : ModPlayer
    {
        public static readonly string[] TextureSuffix = { "Cursed", "Frozen", "Shadow" };
        private void UpdateVisual()
        {
            int fireType = (int)Player.GetModPlayer<FadingHellPlayer>().currentFireType - 2;
            if (fireType < 0)
            {
                int slot = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<FadingHellHat>().Name, EquipType.Head);
                ArmorIDs.Head.Sets.DrawHead[slot] = !Player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn();
                return;
            }

            int headSlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellHat_{TextureSuffix[fireType]}", EquipType.Head);
            int bodySlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellChestplate_{TextureSuffix[fireType]}", EquipType.Body);
            int legsSlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellPants_{TextureSuffix[fireType]}", EquipType.Legs);
            ArmorIDs.Head.Sets.DrawHead[headSlot] = !Player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn();
            Player.head = headSlot;
            Player.body = bodySlot;
            Player.legs = legsSlot;
        }
        //fuck you tml devs, fix FrameEffect
        /*public override void FrameEffects()
        {
            UpdateVisual();
        }*/
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            UpdateVisual();
        }
    }
}
