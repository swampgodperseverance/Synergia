using StramsSurvival.Buffs;
using Synergia.Common.ModConfigs;
using Synergia.Content.Buffs.Debuff;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class SynergiaPlayer : ModPlayer {
        bool giveMsg = false;
        
        public override void PostUpdateBuffs() {
            // нету смысла проверять, если мод не загружен 
            // наш мод и не может быть загружен если нет StramsSurvival
            int dehydrated = ModContent.BuffType<Dehydrated>();
            int starving = ModContent.BuffType<Starving>();

            ReplaceBuff(dehydrated, ModContent.BuffType<SynergiaDehydrated>());
            ReplaceBuff(starving, ModContent.BuffType<SynergiaStarving>());
        }
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
        public override void UpdateBadLifeRegen() {
            if (Player.HasBuff(ModContent.BuffType<SynergiaDehydrated>())) {
                Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
            if (Player.HasBuff(ModContent.BuffType<SynergiaStarving>())) {
                damage *= 0.8f;
            }
        }
        public override void OnEnterWorld() {
            if (!giveMsg) {
                if (ModList.PackBuilderLoaded != null) {
                    if (ModContent.GetInstance<BossConfig>().NewRecipe) {
                        // MR Swamp pls No USE THIS MOD;
                        Main.NewText(string.Format(SUtils.LocUtil.LocUIKey(SUtils.LocUtil.CHATMSG, "tPacer"), ModList.PackBuilderLoaded.DisplayName, Language.GetTextValue("Mods.Synergia.Config.NewRecipe")), Microsoft.Xna.Framework.Color.DarkRed);
                        giveMsg = true;
                    }
                }
            }
        }
    }
}