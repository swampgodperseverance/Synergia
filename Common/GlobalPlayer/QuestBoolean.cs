using Terraria.ModLoader.IO;

namespace Synergia.Common {
    public partial class QuestSystem {
        public class QuestBoolean : ModPlayer {
            public bool DwarfQuest = false, HunterQuest = false, ArtistQuest = false, NinjaQuest = false, FarmerQuest = false, LibrarianQuest = false, HellDwarfQuest = false, HunterQuest1 = false, DwarfQuest1 = false, ArtistQuest1 = false, NinjaQuest1 = false, FarmerQuest1 = false, LibrarianQuest1 = false, HellDwarfQuest1 = false, DwarfQuest2 = false;

            public override void Initialize() {
                DwarfQuest = false;
                DwarfQuest1 = false;
                DwarfQuest2 = false;
                HunterQuest = false;
                HunterQuest1 = false;
                ArtistQuest = false;
                ArtistQuest1 = false;
                NinjaQuest = false;
                NinjaQuest1 = false;
                FarmerQuest = false;
                FarmerQuest1 = false;
                LibrarianQuest = false;
                LibrarianQuest1 = false;
                HellDwarfQuest = false;
                HellDwarfQuest = false;
            }
            public override void SaveData(TagCompound tag) {
                MySaveData(tag, nameof(DwarfQuest),         ref DwarfQuest);
                MySaveData(tag, nameof(DwarfQuest1),        ref DwarfQuest1);
                MySaveData(tag, nameof(DwarfQuest2),        ref DwarfQuest2);
                MySaveData(tag, nameof(HunterQuest),        ref HunterQuest);
                MySaveData(tag, nameof(HunterQuest1),       ref HunterQuest1);
                MySaveData(tag, nameof(ArtistQuest),        ref ArtistQuest);
                MySaveData(tag, nameof(ArtistQuest1),       ref ArtistQuest1);
                MySaveData(tag, nameof(NinjaQuest),         ref NinjaQuest);
                MySaveData(tag, nameof(NinjaQuest1),        ref NinjaQuest1);
                MySaveData(tag, nameof(FarmerQuest),        ref FarmerQuest);
                MySaveData(tag, nameof(FarmerQuest1),       ref FarmerQuest1);
                MySaveData(tag, nameof(LibrarianQuest),     ref LibrarianQuest);
                MySaveData(tag, nameof(LibrarianQuest1),    ref LibrarianQuest1);
                MySaveData(tag, nameof(HellDwarfQuest),     ref HellDwarfQuest);
                MySaveData(tag, nameof(HellDwarfQuest1),    ref HellDwarfQuest1);
            }
            public override void LoadData(TagCompound tag) {
                MyLoadData(tag, nameof(DwarfQuest),         ref DwarfQuest);
                MyLoadData(tag, nameof(DwarfQuest1),        ref DwarfQuest1);
                MyLoadData(tag, nameof(DwarfQuest2),        ref DwarfQuest2);
                MyLoadData(tag, nameof(HunterQuest),        ref HunterQuest);
                MyLoadData(tag, nameof(HunterQuest1),       ref HunterQuest1);
                MyLoadData(tag, nameof(ArtistQuest),        ref ArtistQuest);
                MyLoadData(tag, nameof(ArtistQuest1),       ref ArtistQuest1);
                MyLoadData(tag, nameof(NinjaQuest),         ref NinjaQuest);
                MyLoadData(tag, nameof(NinjaQuest1),        ref NinjaQuest1);
                MyLoadData(tag, nameof(FarmerQuest),        ref FarmerQuest);
                MyLoadData(tag, nameof(FarmerQuest1),       ref FarmerQuest1);
                MyLoadData(tag, nameof(LibrarianQuest),     ref LibrarianQuest);
                MyLoadData(tag, nameof(LibrarianQuest1),    ref LibrarianQuest1);
                MyLoadData(tag, nameof(HellDwarfQuest),     ref HellDwarfQuest);
                MyLoadData(tag, nameof(HellDwarfQuest1),    ref HellDwarfQuest1);
            }
            static void MySaveData(TagCompound tag, string saveName, ref bool save) => tag[saveName] = save;
            private void MyLoadData(TagCompound tag, string saveName, ref bool save) => save = tag.GetBool(saveName);
        }
    }
}