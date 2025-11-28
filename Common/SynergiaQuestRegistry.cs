using Synergia.Content.Quests;
using static Bismuth.Utilities.ModSupport.QuestRegistry;

namespace Synergia.Common;

public partial class QuestSystem {
    public class SynergiaQuestRegistry {
        public static void RegisterQuests() {
            Register(new DwarfQuest());
            Register(new TaxCollectorQuest());
            Register(new HunterQuest());
            Register(new ArtistQuest());
            Register(new NinjaQuest());
            Register(new FarmerQuest());
            Register(new LibrarianQuest());
        }
    }
}