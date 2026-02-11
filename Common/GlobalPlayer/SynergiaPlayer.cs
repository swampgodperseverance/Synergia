using Synergia.Common.Biome;
using Synergia.Common.ModConfigs;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Content.Achievements;
using Synergia.Content.Buffs;
using Synergia.Content.Buffs.Debuff;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Synergia.Common.GlobalPlayer {
    public class SynergiaPlayer : ModPlayer {
        bool giveMsg = false;
        public bool IsEquippedUprateLavaCharm; // Надет ли на играка улучшенный амулет лавы

        public int useSulfuricAcid;

        public override void SaveData(TagCompound tag) {
            tag["useSulfuricAcid"] = useSulfuricAcid;
        }
        public override void LoadData(TagCompound tag) {
            useSulfuricAcid = tag.GetInt("useSulfuricAcid");
        }
        public override void ResetEffects() {
            IsEquippedUprateLavaCharm = false;
        }
        public override void Initialize() {
            IsEquippedUprateLavaCharm = false;
        }
        public override void PostUpdate() {
            if (Player.InModBiome<NewHell>()) {
                if (!NPC.downedPlantBoss) {
                    Player.jumpHeight = 5;
                    Player.wingTime = 0;
                    Player.blockExtraJumps = true;
                    Player.waterWalk = false;
                    Player.waterWalk2 = false;
                }
                Player.ClearBuff(BuffID.Featherfall);
                Player.ClearBuff(BuffID.WaterWalking);
                Player.ClearBuff(BuffID.Gravitation);
            }
            if (!NPC.downedBoss3 && Player.ZoneUnderworldHeight) {
                Player.AddBuff(BuffType<HellishAir>(), 2);
            }
        }
        public override bool CanBeTeleportedTo(Vector2 teleportPosition, string context) {
            bool hellStruct = WorldHelper.CheckBiomeTile((int)(teleportPosition.X / 16f), (int)(teleportPosition.Y / 16f), 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 149);
            bool arena = Player.GetModPlayer<BiomePlayer>().arenaBiome;
            if (!arena && hellStruct && context == "TeleportRod") { return false; } else if(arena) { return base.CanBeTeleportedTo(teleportPosition, context); } else { return base.CanBeTeleportedTo(teleportPosition, context); }
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
                        Main.NewText(string.Format(LocUIKey(CHATMSG, "tPacer"), ModList.PackBuilderLoaded.DisplayName, Language.GetTextValue("Mods.Synergia.Config.NewRecipe")), Color.DarkRed);
                    }
                }
                if (!SynergiaGenVars.SnowVillageGen && !SynergiaGenVars.HellVillageGen) {
                    Main.NewText(string.Format(LocUIKey(CHATMSG, "EmptyStruct"), Mod.DisplayName), Color.DarkRed);
                }
                giveMsg = true;
            }
        }
        public static SynergiaPlayer Get() => Get(Main.LocalPlayer);
        public static SynergiaPlayer Get(Player player) => player.GetModPlayer<SynergiaPlayer>();
    }
}