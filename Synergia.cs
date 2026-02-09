using Avalon.Buffs.Debuffs;
using Synergia.Content.Achievements;
using Synergia.Content.Quests;
using Synergia.GraphicsSetting;
using Terraria;
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

        public override void Load() {
            #region UI
            DwarfChatInterface = new UserInterface();
            DwarfReforgeInterface = new UserInterface();
            LuceatInterface = new UserInterface();
            #endregion
            Instance = this;
            LoadMod();
            Init(Assets);
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
        }
    }
}