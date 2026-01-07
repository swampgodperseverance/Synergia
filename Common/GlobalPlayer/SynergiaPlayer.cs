using StramsSurvival.Buffs;
using Synergia.Common.ModConfigs;
using Synergia.Content.Achievements;
using Synergia.Content.Buffs.Debuff;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Synergia.Common.GlobalPlayer {
    public class SynergiaPlayer : ModPlayer {
        #region Flag
        bool giveMsg = false;

        public int useSulfuricAcid;
        #endregion;
        #region Save
        public override void SaveData(TagCompound tag) {
            tag["useSulfuricAcid"] = useSulfuricAcid;
        }
        public override void LoadData(TagCompound tag) {
            useSulfuricAcid = tag.GetInt("useSulfuricAcid");
        }
        #endregion;
        #region Vanilla
        public override void PostUpdateBuffs() {
            ReplaceBuff(BuffType<Dehydrated>(), BuffType<SynergiaDehydrated>());
            ReplaceBuff(BuffType<Starving>(), BuffType<SynergiaStarving>());
        }
        public override void UpdateBadLifeRegen() {
            if (Player.HasBuff(BuffType<SynergiaDehydrated>())) {
                Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
            if (Player.HasBuff(BuffType<SynergiaStarving>())) {
                damage *= 0.8f;
            }
        }
        public override void OnEnterWorld() {
            if (!giveMsg) {
                SaveAchieveIfCompleted.Restore();
                ModEvent.Instance.SettingEvent();
                if (ModList.PackBuilderLoaded != null) {
                    if (GetInstance<BossConfig>().NewRecipe) {
                        Main.NewText(string.Format(SUtils.LocUtil.LocUIKey(SUtils.LocUtil.CHATMSG, "tPacer"), ModList.PackBuilderLoaded.DisplayName, Language.GetTextValue("Mods.Synergia.Config.NewRecipe")), Color.DarkRed);
                    }
                }
                giveMsg = true;
            }
        }
        #endregion;
        #region NewNethod
        void ReplaceBuff(int originalBuffType, int newBuffType) {
            if (Player.HasBuff(originalBuffType)) {
                int buffIndex = Player.FindBuffIndex(originalBuffType);
                if (buffIndex != -1) {
                    int time = Player.buffTime[buffIndex];
                    Player.DelBuff(buffIndex);
                    Player.AddBuff(newBuffType, time);
                }
            }
        }
        #endregion;
    }
}