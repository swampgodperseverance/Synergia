using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Synergia.Common
{
    public partial class QuestSystem
    {
        public class QuestBoolean : ModPlayer
        {
            public bool DwarfQuest = false;

            public override void Initialize()
            {
                DwarfQuest = false;
            }
            public override void SaveData(TagCompound tag)
            {
                tag["DwarfQuest"] = DwarfQuest;
            }
            public override void LoadData(TagCompound tag)
            {
                DwarfQuest = tag.GetBool("DwarfQuest");
            }
        }
    }
}
