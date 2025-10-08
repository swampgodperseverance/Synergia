using Terraria;
using Terraria.ModLoader;
using StramsSurvival.Buffs;
using Terraria.ID; // Added missing using directive

namespace Synergia.Common
{
    // 🔸 Наши версии бафов
    public class SynergiaDehydrated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dehydrated");
            // Description.SetDefault("You can't regenerate life");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }

    public class SynergiaStarving : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starving");
            // Description.SetDefault("Damage reduced by 20%");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }

                // 🔸 Логика бафов
          public class SynergiaPlayer : ModPlayer
        {
            public override void PostUpdateBuffs()
            {
                Mod otherMod = ModLoader.GetMod("StramsSurvival");
                if (otherMod != null)
                {
                    int dehydrated = otherMod.Find<ModBuff>("Dehydrated").Type;
                    int starving = otherMod.Find<ModBuff>("Starving").Type;

                    ReplaceBuff(dehydrated, ModContent.BuffType<SynergiaDehydrated>());
                    ReplaceBuff(starving, ModContent.BuffType<SynergiaStarving>());
                }
            }

            private void ReplaceBuff(int originalBuffType, int newBuffType)
            {
                if (Player.HasBuff(originalBuffType))
                {
                    int buffIndex = Player.FindBuffIndex(originalBuffType);
                    if (buffIndex != -1)
                    {
                        int time = Player.buffTime[buffIndex];
                        Player.DelBuff(buffIndex);
                        Player.AddBuff(newBuffType, time);
                    }
                }
            }

            public override void UpdateBadLifeRegen()
            {
                if (Player.HasBuff(ModContent.BuffType<SynergiaDehydrated>()))
                {
                    Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                }
            }

            // ✅ Новый метод для снижения урона
            public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
            {
                if (Player.HasBuff(ModContent.BuffType<SynergiaStarving>()))
                {
                    damage *= 0.8f;
                }
            }
        }

}