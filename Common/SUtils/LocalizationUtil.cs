using Terraria.Localization;

namespace Synergia.Common.SUtils;

// Localization Util
public static class LocUtil {
    public const string ACC = "Accessories";
    public const string ARM = "Armors";
    public const string WEP = "Weapons";
    public const string CHATMSG = "ChatsMsg";

    public enum CategoryName {
        NPC, Quest
    }
    public static string Category(CategoryName name) {
        return name switch {
            CategoryName.NPC => "NPCs",
            CategoryName.Quest => "Quests",
            _ => "Quest",
        };
    }
    public static string LocKey(CategoryName nameType, string nameKey) {
        return nameType switch {
            CategoryName.NPC => Language.GetTextValue($"Mods.Synergia.NPCs.{nameKey}"),
            CategoryName.Quest => Language.GetTextValue($"Mods.Synergia.Quests.{nameKey}"),
            _ => "Quest",
        };
    }
    public static string LocQuestKey(string npcName, string nameKey) => Language.GetTextValue($"Mods.Synergia.Quests.{npcName}.{nameKey}");
    public static string LocUIKey(string nameCategory, string nameKey) => Language.GetTextValue($"Mods.Synergia.UI.{nameCategory}.{nameKey}");
    public static string ItemTooltip(string category, string tooltipKey) => Language.GetTextValue($"Mods.Synergia.Tooltips.{category}.{tooltipKey}");
    public static string EventLocKey(string eventName) => Language.GetTextValue($"Mods.Synergia.Events.{eventName}");
    public static string SynergiaLocKey(string name) => Language.GetTextValue($"Mods.Synergia.{name}");
    public static string AddBaseTooltips(string name) => Language.GetTextValue($"Mods.Synergia.BaseLoc.{name}");
    public static string DamageClassName(string className) => $"Mods.Synergia.BaseLoc.DamageClass.{className}";
    public static string AddAttackSpeed(string damageTypeKey, int speed) {
        string damageType = Language.GetTextValue(damageTypeKey);
        string attackSpead = Language.GetTextValue("Mods.Synergia.BaseLoc.AttackSpead");
        return string.Format(attackSpead, damageType, speed);
    }
}