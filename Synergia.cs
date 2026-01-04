using Synergia.Content.Achievements;
using Synergia.GraphicsSetting;
using Terraria;
using Terraria.UI;
using static Synergia.Common.QuestSystem.SynergiaQuestRegistry;
using static Synergia.GraphicsSetting.SynegiyGraphics;
using static Synergia.ModList;

namespace Synergia
{
    public class Synergia : Mod
    {
        public static Synergia Instance { get; private set; }
        public UserInterface DwarfUserInterface;
        public const string ModName = nameof(Synergia);

        public override void Load() {
            #region UI
            DwarfUserInterface = new UserInterface();
            #endregion
            Instance = this;
            LoadMod();
            RegisterQuests();
            Init(Assets);
        }
        public override void PostSetupContent() {
            base.PostSetupContent();

            if (!Main.dedServ) {
                NewTexture.Load();
            }
            SaveAchieveIfCompleted.Load();
        }
    }
}