using ReLogic.Content;
using ReLogic.Graphics;
using Synergia.Common.ModConfigs;
using Synergia.Common.ModSystems.Netcode;
using Synergia.Content.Achievements;
using Synergia.Content.Quests;
using Synergia.GraphicsSetting;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using static Synergia.GraphicsSetting.SynegiyGraphics;
using static Synergia.ModList;

namespace Synergia
{
    public class Synergia : Mod {
        public static Synergia Instance { get; private set; }
        public UserInterface DwarfReforgeInterface { get; private set; }
        public UserInterface DwarfChatInterface { get; private set; }
        public UserInterface LuceatInterface { get; private set; }
        public static string ModName { get; private set; } = nameof(Synergia);

        public bool needUpdateFont = false;

        public static Asset<DynamicSpriteFont> OrigDeathText { get; private set; }
        public static Asset<DynamicSpriteFont> OrigMouseText { get; private set; }
        public static Asset<DynamicSpriteFont> OrigItemStack { get; private set; }

        public static Asset<DynamicSpriteFont> DantesCursiveDetxText { get; private set; }
        public static Asset<DynamicSpriteFont> DantesCursiveMouseText { get; private set; }
        public static Asset<DynamicSpriteFont> DantesCursiveItemStack { get; private set; }

        public override void Load() {
            #region UI
            DwarfChatInterface = new UserInterface();
            DwarfReforgeInterface = new UserInterface();
            LuceatInterface = new UserInterface();
            #endregion
            Instance = this;
            LoadMod();
            //Fixes a post setup crash caused by TRAE Project
            On_ItemSorting.SetupWhiteLists += (orig) => {
               try { orig(); }
               catch (System.Exception) { }
            };
            Init(Assets);

            OrigDeathText = FontAssets.DeathText;
            OrigMouseText = FontAssets.MouseText;
            OrigItemStack = FontAssets.ItemStack;

            DantesCursiveDetxText = Request<DynamicSpriteFont>("Synergia/Assets/Font/DantesCursive_DetxText", AssetRequestMode.ImmediateLoad);
            DantesCursiveMouseText = Request<DynamicSpriteFont>("Synergia/Assets/Font/DantesCursive_MouseText", AssetRequestMode.ImmediateLoad);
            DantesCursiveItemStack = Request<DynamicSpriteFont>("Synergia/Assets/Font/DantesCursive_ItemStack", AssetRequestMode.ImmediateLoad);

            On_Main.DrawMenu += (orig, self, time) => {
                orig(self, time);
                NewFont();
            }; 
        }

        internal static void NewFont() {
            if (GetInstance<Synergia>().needUpdateFont) {
                if (DantesCursiveDetxText != null && DantesCursiveMouseText != null && DantesCursiveItemStack != null) {
                    Asset<DynamicSpriteFont> Dfont;
                    Asset<DynamicSpriteFont> Mfont;
                    Asset<DynamicSpriteFont> Ifont;

                    if (GetInstance<UIConfig>().NewFont) {
                        Dfont = DantesCursiveDetxText;
                        Mfont = DantesCursiveMouseText;
                        Ifont = DantesCursiveItemStack;
                        GetInstance<Synergia>().needUpdateFont = false;
                    }
                    else {
                        Dfont = OrigDeathText;
                        Mfont = OrigMouseText;
                        Ifont = OrigItemStack;
                        GetInstance<Synergia>().needUpdateFont = false;
                    }

                    FontAssets.DeathText = Dfont;
                    FontAssets.MouseText = Mfont;
                    FontAssets.ItemStack = Ifont;
                }
            }
        }

        public override void PostSetupContent() {
            base.PostSetupContent();

            if (!Main.dedServ) {
                NewTexture.Load();
            }
            SaveAchieveIfCompleted.Load();
            foreach (BaseQuestLogic content in GetContent<BaseQuestLogic>()) {
                if (content is IPostSetup ps) {
                    ps.PostSetup(this);
                }
            }
        }
        public override void Unload() {
            NewTexture.Unload();
            if (OrigDeathText != null) { FontAssets.DeathText = OrigDeathText; }
            if (OrigMouseText != null) { FontAssets.MouseText = OrigMouseText; }
            if (OrigItemStack != null) { FontAssets.ItemStack = OrigItemStack; }
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            MultiplayerSystem.HandlePacket(reader, whoAmI);
    }
}
