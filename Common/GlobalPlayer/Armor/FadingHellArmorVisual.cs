using Synergia.Content.Items.Armor.Magic.FadingHell;
using Synergia.Trails;
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
            if (Player.merman || Player.wereWolf)
                return;

            int fireType = (int)Player.GetModPlayer<FadingHellPlayer>().currentFireType - 2;
            if (fireType < 0)
            {
                int slot = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<FadingHellHat>().Name, EquipType.Head);
                ArmorIDs.Head.Sets.DrawHead[slot] = !(Player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn() && IsVisible(ItemType<FadingHellHat>(), 0));
                return;
            }

            if(IsVisible(ItemType<FadingHellHat>(), 0))
            {
                int headSlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellHat_{TextureSuffix[fireType]}", EquipType.Head);
                ArmorIDs.Head.Sets.DrawHead[headSlot] = !Player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn();
                Player.head = headSlot;
            }
            if (IsVisible(ItemType<FadingHellChestplate>(), 1))
            {
                int bodySlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellChestplate_{TextureSuffix[fireType]}", EquipType.Body);
                Player.body = bodySlot;
            }
            if(IsVisible(ItemType<FadingHellPants>(), 2))
            {
                int legsSlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellPants_{TextureSuffix[fireType]}", EquipType.Legs);
                Player.legs = legsSlot;
            }
        }
        private bool IsVisible(int itemType, int slot)
        {
            return Player.armor[slot].type == itemType && Player.armor[slot + 10].type == ItemID.None
            || Player.armor[slot + 10].type == itemType;
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
