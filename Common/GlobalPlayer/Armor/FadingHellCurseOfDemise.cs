using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Common.GlobalPlayer.Armor
{
    public class FadingHellCursedFireEffect : ModPlayer
    {
        // time addition ratio: 180 dmg = 1 sec
        //                      3 dmg = 1 tick
        private const float DamageTimeRatio = 3f;

        public bool IsActive = false;
        public override void ResetEffects()
        {
            IsActive = false;
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsActive) return;
            
            target.AddBuff(BuffType<CurseOfDemise>(), (int)(damageDone / DamageTimeRatio), false);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsActive) return;

            float time = damageDone / DamageTimeRatio;
            if (proj.maxPenetrate > 1)
                time *= (float)(proj.penetrate + 1) / proj.maxPenetrate;
            else
                time /= 10;
            target.AddBuff(BuffType<CurseOfDemise>(), (int)time, false);
        }
    }

    public class CurseOfDemise : ModBuff
    {
        private const int MaxEffectTime = 120;
        public override string Texture => "Synergia/Assets/Textures/Blank";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            Main.pvpBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<CurseOfDemiseNPCEffect>().DebuffStrength = (float)npc.buffTime[buffIndex] / MaxEffectTime;
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.buffTime[buffIndex] += time;
            npc.buffTime[buffIndex] = Math.Clamp(npc.buffTime[buffIndex], 0, MaxEffectTime);
            return true;
        }
    }
    public class CurseOfDemiseNPCEffect : GlobalNPC
    {
        private const int DefenceReduction = 20;
        private const int DamagePerSecond = 100;
        private const float IncomingDamageIncreasion = 0.5f;
        private const float OutcomingDamageReduction = 0.75f;

        public float DebuffStrength = 0f;
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            DebuffStrength = 0f;
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            modifiers.Defense.Base -= DefenceReduction * DebuffStrength;
            modifiers.CritDamage += IncomingDamageIncreasion * DebuffStrength;
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= MathHelper.Lerp(1f, OutcomingDamageReduction, DebuffStrength);
        }
        public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= MathHelper.Lerp(1f, OutcomingDamageReduction, DebuffStrength);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            npc.lifeRegen -= (int)(DamagePerSecond * 2 * DebuffStrength);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            drawColor = Color.Lerp(drawColor, new Color(129, 255, 33), DebuffStrength);
        }
    }
}
