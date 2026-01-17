using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Synergia.Common
{
    public partial class QuestSystem
    {
        public class QuestBoolean : ModPlayer
        {
            public bool DwarfQuest = false, HunterQuest = false, ArtistQuest = false, NinjaQuest = false, FarmerQuest = false, LibrarianQuest = false, HellDwarfQuest = false;

            public override void Initialize() {
                DwarfQuest = false;
                HunterQuest = false;
                ArtistQuest = false;
                NinjaQuest = false;
                FarmerQuest = false;
                LibrarianQuest = false;
                HellDwarfQuest = false;
            }
            public override void SaveData(TagCompound tag) {
                MySaveData(tag, nameof(DwarfQuest),     ref DwarfQuest);
                MySaveData(tag, nameof(HunterQuest),    ref HunterQuest);
                MySaveData(tag, nameof(ArtistQuest),    ref ArtistQuest);
                MySaveData(tag, nameof(NinjaQuest),     ref NinjaQuest);
                MySaveData(tag, nameof(FarmerQuest),    ref FarmerQuest);
                MySaveData(tag, nameof(LibrarianQuest), ref LibrarianQuest);
                MySaveData(tag, nameof(HellDwarfQuest), ref HellDwarfQuest);
            }
            public override void LoadData(TagCompound tag) {
                MyLoadData(tag, nameof(DwarfQuest),     ref DwarfQuest);
                MyLoadData(tag, nameof(HunterQuest),    ref HunterQuest);
                MyLoadData(tag, nameof(ArtistQuest),    ref ArtistQuest);
                MyLoadData(tag, nameof(NinjaQuest),     ref NinjaQuest);
                MyLoadData(tag, nameof(FarmerQuest),    ref FarmerQuest);
                MyLoadData(tag, nameof(LibrarianQuest), ref LibrarianQuest);
                MyLoadData(tag, nameof(HellDwarfQuest), ref HellDwarfQuest);
            }
            void MySaveData(TagCompound tag, string saveName, ref bool save) => tag[saveName] = save;
            void MyLoadData(TagCompound tag, string saveName, ref bool save) => save = tag.GetBool(saveName);
        }
    }
}
