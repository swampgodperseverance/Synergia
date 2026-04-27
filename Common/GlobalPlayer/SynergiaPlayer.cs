using System;
using RoA.Common.NPCs;
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

namespace Synergia.Common.GlobalPlayer
{
    public class SynergiaPlayer : ModPlayer
    {
        bool giveMsg = false;
        public bool IsEquippedUprateLavaCharm;
        public int useSulfuricAcid;
        public bool downedLothorBossAddon = false;
        public bool desertSandstormImmunity = false;
        public float omnipotenceDodgeChance = 0f;
        public bool omnipotenceThornsOnDodge = false;
        public float omnipotenceThornsMultiplier = 1f;

        public float herosAirDashChance = 0f;
        public int herosRunningRegen = 0;
        public int dashLeftTimer = 0;
        public int dashRightTimer = 0;
        public bool isAirDashing = false;

        public float potionHealMultiplier = 1f;
        public int potionCooldownReduction = 0;
        public float projectileReflectChance = 0f;

        public override void SaveData(TagCompound tag)
        {
            tag["useSulfuricAcid"] = useSulfuricAcid;
            tag["downedLothorBossAddon"] = downedLothorBossAddon;
        }

        public override void LoadData(TagCompound tag)
        {
            useSulfuricAcid = tag.GetInt("useSulfuricAcid");
            downedLothorBossAddon = tag.GetBool("downedLothorBossAddon");
        }

        public override void ResetEffects()
        {
            IsEquippedUprateLavaCharm = false;
            potionHealMultiplier = 1f;
            potionCooldownReduction = 0;
            projectileReflectChance = 0f;
            desertSandstormImmunity = false;
            omnipotenceDodgeChance = 0f;
            omnipotenceThornsOnDodge = false;
            omnipotenceThornsMultiplier = 1f;
            herosAirDashChance = 0f;
            herosRunningRegen = 0;
            dashLeftTimer = 0;
            dashRightTimer = 0;
            isAirDashing = false;
        }

        public override void Initialize()
        {
            IsEquippedUprateLavaCharm = false;
            desertSandstormImmunity = false;
        }

        public override void PreUpdate()
        {
            if (desertSandstormImmunity && Player.ZoneDesert && Player.ZoneSandstorm)
            {
                Player.sandStorm = false;
            }
            base.PreUpdate();
        }

        public override void PreUpdateMovement()
        {
            if (herosAirDashChance > 0f && !isAirDashing && !Player.mount.Active && Player.velocity.Y != 0)
            {
                if (Player.controlLeft)
                {
                    if (dashLeftTimer > 0)
                    {
                        Player.velocity.X = -12f;
                        isAirDashing = true;
                        if (Main.rand.NextFloat() < herosAirDashChance && Player.whoAmI == Main.myPlayer)
                        {
                            Player.NinjaDodge();
                        }
                        dashLeftTimer = 0;
                    }
                    else
                    {
                        dashLeftTimer = 12;
                    }
                }
                if (Player.controlRight)
                {
                    if (dashRightTimer > 0)
                    {
                        Player.velocity.X = 12f;
                        isAirDashing = true;
                        if (Main.rand.NextFloat() < herosAirDashChance && Player.whoAmI == Main.myPlayer)
                        {
                            Player.NinjaDodge();
                        }
                        dashRightTimer = 0;
                    }
                    else
                    {
                        dashRightTimer = 12;
                    }
                }
            }
            if (Player.velocity.Y == 0) isAirDashing = false;
            base.PreUpdateMovement();
        }

        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(BuffType<SynergiaDehydrated>()))
            {
                Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
            }
            if (desertSandstormImmunity && Player.ZoneDesert && Player.ZoneSandstorm)
            {
                Player.lifeRegenTime = 0;
            }
        }

        public override void PostUpdate()
        {
            if (herosRunningRegen > 0 && Math.Abs(Player.velocity.X) > 5f)
            {
                Player.lifeRegen += herosRunningRegen;
            }
            if (Player.InModBiome<NewHell>())
            {
                if (!NPC.downedPlantBoss)
                {
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
        }

        public override bool CanBeTeleportedTo(Vector2 teleportPosition, string context)
        {
            bool hellStruct = WorldHelper.CheckBiomeTile((int)(teleportPosition.X / 16f), (int)(teleportPosition.Y / 16f), 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 149);
            bool arena = Player.GetModPlayer<BiomePlayer>().arenaBiome;
            if (!arena && hellStruct && context == "TeleportRod") { return false; }
            else if (arena) { return base.CanBeTeleportedTo(teleportPosition, context); }
            else { return base.CanBeTeleportedTo(teleportPosition, context); }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (omnipotenceDodgeChance > 0f && Main.rand.NextFloat() < omnipotenceDodgeChance)
            {
                if (Player.whoAmI == Main.myPlayer)
                {
                    Player.NinjaDodge();
                }
                modifiers.FinalDamage *= 0f;
                if (omnipotenceThornsOnDodge)
                {
                    if (modifiers.DamageSource.TryGetCausingEntity(out Entity attacker) && attacker is NPC npc && npc.active)
                    {
                        int returnDamage = (int)(modifiers.GetDamage(1f, 0f, 1f) * omnipotenceThornsMultiplier);
                        if (returnDamage > 0)
                        {
                            var hit = new NPC.HitInfo
                            {
                                Damage = returnDamage,
                                HitDirection = 0,
                                Crit = false
                            };
                            npc.StrikeNPC(hit);
                        }
                    }
                }
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Player.HasBuff(BuffType<SynergiaStarving>()))
            {
                damage *= 0.8f;
            }
        }

        public override void OnEnterWorld()
        {
            if (!giveMsg)
            {
                SaveAchieveIfCompleted.Restore();
                ModEvent.Instance.SettingEvent();
                if (ModList.PackBuilderLoaded != null)
                {
                    if (GetInstance<BossConfig>().NewRecipe)
                    {
                        Main.NewText(string.Format(LocUIKey(CHATMSG, "tPacer"), ModList.PackBuilderLoaded.DisplayName, Language.GetTextValue("Mods.Synergia.Config.NewRecipe")), Color.DarkRed);
                    }
                }
                if (!SynergiaGenVars.SnowVillageGen && !SynergiaGenVars.HellVillageGen)
                {
                    Main.NewText(string.Format(LocUIKey(CHATMSG, "EmptyStruct"), Mod.DisplayName), Color.DarkRed);
                }
                giveMsg = true;
            }
        }

        public static SynergiaPlayer Get() => Get(Main.LocalPlayer);
        public static SynergiaPlayer Get(Player player) => player.GetModPlayer<SynergiaPlayer>();
    }
}