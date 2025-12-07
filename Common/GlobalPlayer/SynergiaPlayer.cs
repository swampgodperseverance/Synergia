using StramsSurvival.Buffs;
using Terraria;
using Terraria.ModLoader;
using Synergia.Content.Buffs.Debuff;

namespace Synergia.Common.GlobalPlayer {
    public class SynergiaPlayer : ModPlayer {
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
    }
}