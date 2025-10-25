using Terraria.Localization;

namespace Synergia.Common.SUtils;

// Localization Util
public static class LocUtil
{
    public enum CategoryName {
        NPC, Quest
    }
    public static string Category(CategoryName name)
    {
        return name switch {
            CategoryName.NPC => "NPCs",
            CategoryName.Quest => "Quests",
            _ => "Quest",
        };
    }
    public static string LocKey(CategoryName nameType, string nameKey)
    {
        return nameType switch {
            CategoryName.NPC => Language.GetTextValue($"Mods.Synergia.NPCs.{nameKey}"),
            CategoryName.Quest => Language.GetTextValue($"Mods.Synergia.Quests.{nameKey}"),
            _ => "Quest",
        };
    }
    public static string LocKey(string npcName, string nameKey) => Language.GetTextValue($"Mods.Synergia.Quests.{npcName}.{nameKey}");
}