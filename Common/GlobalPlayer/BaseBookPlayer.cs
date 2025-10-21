using Avalon.Items.Consumables;
using Avalon.Items.Material;
using Avalon.Items.Placeable.Crafting;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Items.Histories;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using ValhallaMod.Items.Consumable;
using static Terraria.ID.ItemID;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalPlayer
{
    public class BaseBookPlayer : ModPlayer
    {
        static readonly int IckyAltar = ItemType<IckyAltar>();
        static readonly int NaquadahAnvil = ItemType<NaquadahAnvil>();

        public static Rectangle BookRectangle(float X, float Y, int Widht, int Height) { return _ = new Rectangle((int)(X), (int)(Y), Widht, Height); }
        public static void BaseDrawBook(SpriteBatch spriteBatch, string name, float BookOpacity)
        {
            Texture2D texture = Request<Texture2D>($"Synergia/Assets/UIs/{name}").Value;
            Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;
            spriteBatch.Draw(texture, position, null, Color.White * BookOpacity, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
        public static bool DrawBookButtons(SpriteBatch spriteBatch, bool Right, string TextureName = "Mana2")
        {
            string NameRightButton = Language.GetTextValue("Mods.Synergia.Book.Button");
            string NameAltButton = Language.GetTextValue("Mods.Synergia.Book.Button2");

            Rectangle nextPageButton;
            Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Texture2D buttonTex = Request<Texture2D>(Right ? $"Synergia/Assets/UIs/{TextureName}" + "_Alt" : $"Synergia/Assets/UIs/{TextureName}").Value;

            nextPageButton = BookRectangle(Right ? position.X + 375 : position.X - 405, position.Y - -285, 25, 25);

            bool hovering = nextPageButton.Contains(Main.MouseScreen.ToPoint());

            spriteBatch.Draw(buttonTex, nextPageButton, hovering ? Color.White : Color.Gray);

            if(hovering) Main.instance.MouseText(Right ? NameRightButton : NameAltButton);

            if (hovering && Main.mouseLeft && Main.mouseLeftRelease)
            {
                SoundEngine.PlaySound(Terraria.ID.SoundID.MenuTick);
                return true;
            }
            return false;
        }
        public static bool WhenHovering(int itemType, Rectangle rectangle)
        {
            Point mousePos = Main.MouseScreen.ToPoint();
            if (rectangle.Contains(mousePos))
            {
                Main.HoverItem = new Item(itemType).Clone();
                Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
                return true;
            }
            return false;
        }
        public static void BookStage(Vector2 position, int stage)
        {
            if (stage == 1)
            {
                if (WhenHovering(Gel, BookRectangle(position.X - 220, position.Y - 315, 20, 20))) return;
                if (WhenHovering(GoldCrown, BookRectangle(position.X - 190, position.Y - 315, 20, 20))) return;
                if (WhenHovering(IckyAltar, BookRectangle(position.X - 207, position.Y - 288, 26, 20))) return;
                if (WhenHovering(ItemType<KingSlimeHistory>(), BookRectangle(position.X - 150, position.Y - 300, 140, 60))) return;
                if (WhenHovering(ItemType<KingSlimeHistory>(), BookRectangle(position.X - 150, position.Y - 300, 140, 60))) return;
                if (WhenHovering(SlimeCrown, BookRectangle(position.X - 208, position.Y - 265, 20, 20))) return;
                if (WhenHovering(ItemType<SuspiciousLookingEgg>(), BookRectangle(position.X - 208, position.Y - 210, 20, 25))) return;
                if (WhenHovering(ItemType<LepusHistory>(), BookRectangle(position.X - 150, position.Y - 215, 140, 45))) return;
                if (WhenHovering(Lens, BookRectangle(position.X - 208, position.Y - 155, 20, 25))) return;
                if (WhenHovering(IckyAltar, BookRectangle(position.X - 207, position.Y - 122, 26, 20))) return;
                if (WhenHovering(SuspiciousLookingEye, BookRectangle(position.X - 206, position.Y - 100, 25, 20))) return;
                if (WhenHovering(ItemType<EyeHistory>(), BookRectangle(position.X - 140, position.Y - 145, 115, 70))) return;
                if (WhenHovering(ItemType<VirulentPowder>(), BookRectangle(position.X - 220, position.Y - 65, 20, 20))) return;
                if (WhenHovering(ItemType<YuckyBit>(), BookRectangle(position.X - 190, position.Y - 65, 20, 20))) return;
                if (WhenHovering(IckyAltar, BookRectangle(position.X - 207, position.Y - 40, 26, 20))) return;
                if (WhenHovering(ItemType<InfestedCarcass>(), BookRectangle(position.X - 208, position.Y - 10, 20, 24))) return;
                if (WhenHovering(ItemType<BacteriumHistory>(), BookRectangle(position.X - 135, position.Y - 55, 115, 50))) return;
                if (WhenHovering(Abeemination, BookRectangle(position.X - 208, position.Y + 40, 20, 25))) return;
                if (WhenHovering(ItemType<BeeHistory>(), BookRectangle(position.X - 135, position.Y - -30, 120, 50))) return;
                if (WhenHovering(ItemType<TurkeyFeather>(), BookRectangle(position.X - 220, position.Y - -120, 20, 25))) return;
                if (WhenHovering(ItemType<CursedStuffing>(), BookRectangle(position.X - 190, position.Y - -120, 20, 25))) return;
                if (WhenHovering(ItemType<TurkorHistory>(), BookRectangle(position.X - 135, position.Y - -120, 120, 30))) return;
                if (WhenHovering(ItemType<GoblinHistory>(), BookRectangle(position.X - 275, position.Y - -205, 215, 80))) return;
                if (WhenHovering(ItemType<FlashHistory>(), BookRectangle(position.X - -68, position.Y - -185, 290, 100))) return;
                if (WhenHovering(ItemType<SkeletHistory>(), BookRectangle(position.X - -268, position.Y - 300, 130, 60))) return;
                if (WhenHovering(ItemType<DeerHistory>(), BookRectangle(position.X - -268, position.Y - 225, 140, 60))) return;
                if (WhenHovering(ItemType<BeakHistory>(), BookRectangle(position.X - -268, position.Y - 145, 140, 56))) return;
                if (WhenHovering(ItemType<LuteSummonHistory>(), BookRectangle(position.X - -196, position.Y - 45, 60, 40))) return;
                if (WhenHovering(ItemType<SkeletSummonHistory>(), BookRectangle(position.X - -184, position.Y - 300, 50, 30))) return;
                if (WhenHovering(ItemType<Beak>(), BookRectangle(position.X - -195, position.Y - 155, 20, 20))) return;
                if (WhenHovering(SandBlock, BookRectangle(position.X - -227, position.Y - 153, 10, 10))) return;
                if (WhenHovering(FlinxFur, BookRectangle(position.X - -195, position.Y - 235, 20, 20))) return;
                if (WhenHovering(DemoniteOre, BookRectangle(position.X - -220, position.Y - 242, 20, 20))) return;
                if (WhenHovering(IckyAltar, BookRectangle(position.X - -220, position.Y - 213, 26, 20))) return;
                if (WhenHovering(DeerThing, BookRectangle(position.X - -220, position.Y - 190, 20, 26))) return;
                if (WhenHovering(Lens, BookRectangle(position.X - -245, position.Y - 235, 20, 25))) return;
                if (WhenHovering(Hellforge, BookRectangle(position.X - -225, position.Y - 130, 15, 15))) return;
                if (WhenHovering(ItemType<DesertHorn>(), BookRectangle(position.X - -225, position.Y - 109, 15, 15))) return;
                if (WhenHovering(ItemType<LuteHistory>(), BookRectangle(position.X - -280, position.Y - 60, 157, 60))) return;
            }
            if (stage == 2)
            {
                if (WhenHovering(ItemType<ColdSummonHistory>(), BookRectangle(position.X - 215, position.Y - 288, 26, 20))) return;
                if (WhenHovering(ItemType<ColdHistory>(), BookRectangle(position.X - 150, position.Y - 300, 140, 60))) return;
                if (WhenHovering(ItemType<NautilusSummonHistory>(), BookRectangle(position.X - 213, position.Y - 210, 28, 25))) return;
                if (WhenHovering(ItemType<NautilusHistory>(), BookRectangle(position.X - 150, position.Y - 223, 140, 55))) return;
                if (WhenHovering(FallenStar, BookRectangle(position.X - 211, position.Y - 155, 18, 25))) return;
                if (WhenHovering(SoulofFlight, BookRectangle(position.X - 185, position.Y - 155, 25, 20))) return;
                if (WhenHovering(CobaltBar, BookRectangle(position.X - 246, position.Y - 155, 20, 20))) return;
                if (WhenHovering(NaquadahAnvil, BookRectangle(position.X - 217, position.Y - 125, 26, 20))) return;
                if (WhenHovering(ItemType<HeavensSeal>(), BookRectangle(position.X - 214, position.Y - 94, 22, 22))) return;
                if (WhenHovering(ItemType<EmperorHistory>(), BookRectangle(position.X - 142, position.Y - 145, 135, 70))) return;
                if (WhenHovering(QueenSlimeCrystal, BookRectangle(position.X - 225, position.Y - 40, 26, 26))) return;
                if (WhenHovering(ItemType<QueenHistory>(), BookRectangle(position.X - 135, position.Y - 47, 125, 60))) return;
                if (WhenHovering(MechanicalSkull, BookRectangle(position.X - 208, position.Y - -40, 20, 25))) return;
                if (WhenHovering(MechanicalEye, BookRectangle(position.X - 250, position.Y - -40, 25, 20))) return;
                if (WhenHovering(MechanicalWorm, BookRectangle(position.X - 174, position.Y - -40, 25, 18))) return;
                if (WhenHovering(ItemType<TrioHistory>(), BookRectangle(position.X - 135, position.Y - -24, 120, 50))) return;
                if (WhenHovering(TinkerersWorkshop, BookRectangle(position.X - 213, position.Y - -120, 20, 25))) return;
                if (WhenHovering(ChlorophyteBar, BookRectangle(position.X - 213, position.Y - -93, 25, 20))) return;
                if (WhenHovering(MasterBait, BookRectangle(position.X - 237, position.Y - -93, 22, 22))) return;
                if (WhenHovering(ItemType<ShinyBait>(), BookRectangle(position.X - 213, position.Y - -147, 25, 25))) return;
                if (WhenHovering(ItemType<CarnageStory>(), BookRectangle(position.X - 135, position.Y - -98, 120, 50))) return;
                if (WhenHovering(ItemType<PiratesHistory>(), BookRectangle(position.X - 306, position.Y - -205, 275, 80))) return;
                if (WhenHovering(ItemType<MoonsHistory>(), BookRectangle(position.X - -68, position.Y - -185, 300, 100))) return;
                if (WhenHovering(ItemType<PlanteraHistory>(), BookRectangle(position.X - -268, position.Y - 300, 130, 30))) return;
                if (WhenHovering(ItemType<PlanteraSummonHistory>(), BookRectangle(position.X - -186, position.Y - 295, 55, 35))) return;
                if (WhenHovering(ItemType<BlazewormHistory>(), BookRectangle(position.X - -268, position.Y - 225, 140, 60))) return;
                if (WhenHovering(ItemType<GolemHistory>(), BookRectangle(position.X - -268, position.Y - 137, 140, 56))) return;
                if (WhenHovering(ItemType<GolemSummonHistory>(), BookRectangle(position.X - -186, position.Y - 125, 33, 40))) return;
                if (WhenHovering(ItemType<CaesiumHeavyAnvil>(), BookRectangle(position.X - -186, position.Y - 26, 30, 20))) return;
                if (WhenHovering(LunarTabletFragment, BookRectangle(position.X - -206, position.Y - 49, 20, 20))) return;
                if (WhenHovering(Bone, BookRectangle(position.X - -170, position.Y - 49, 20, 20))) return;
                if (WhenHovering(ItemType<SuspiciousLookingSkull>(), BookRectangle(position.X - -194, position.Y - 3, 20, 20))) return;
                if (WhenHovering(ItemType<ValhallaMod.Items.Placeable.Blocks.Sinstone>(), BookRectangle(position.X - -180, position.Y - 228, 20, 15))) return;
                if (WhenHovering(ItemType<BottledLava>(), BookRectangle(position.X - -205, position.Y - 227, 20, 20))) return;
                if (WhenHovering(Hellforge, BookRectangle(position.X - -200, position.Y - 203, 26, 20))) return;
                if (WhenHovering(ItemType<HellwormScale>(), BookRectangle(position.X - -200, position.Y - 173, 20, 20))) return;
                if (WhenHovering(ItemType<LifeDew>(), BookRectangle(position.X - -230, position.Y - 227, 20, 20))) return;
                if (WhenHovering(ItemType<OcramHistory>(), BookRectangle(position.X - -280, position.Y - 50, 157, 60))) return;
                if (WhenHovering(ItemType<EclipseHistory>(), BookRectangle(position.X - -365, position.Y - -108, 28, 40))) return;
                if (WhenHovering(NaquadahAnvil, BookRectangle(position.X - -238, position.Y - -123, 15, 10))) return;
                if (WhenHovering(NaughtyPresent, BookRectangle(position.X - -238, position.Y - -141, 15, 19))) return;
                if (WhenHovering(Ectoplasm, BookRectangle(position.X - -242, position.Y - -101, 10, 10))) return;
                if (WhenHovering(Silk, BookRectangle(position.X - -222, position.Y - -101, 10, 10))) return;
                if (WhenHovering(ItemType<SoulofIce>(), BookRectangle(position.X - -264, position.Y - -101, 10, 10))) return;
                if (WhenHovering(NaquadahAnvil, BookRectangle(position.X - -110, position.Y - -122, 15, 10))) return;
                if (WhenHovering(Ectoplasm, BookRectangle(position.X - -114, position.Y - -100, 10, 10))) return;
                if (WhenHovering(ItemType<ValhallaMod.Items.Material.Bar.CorrodeBar>(), BookRectangle(position.X - -132, position.Y - -105, 13, 10))) return;
                if (WhenHovering(Pumpkin, BookRectangle(position.X - -92, position.Y - -102, 12, 12))) return;
                if (WhenHovering(PumpkinMoonMedallion, BookRectangle(position.X - -112, position.Y - -147, 19, 22))) return;
            }
        }
    }
}