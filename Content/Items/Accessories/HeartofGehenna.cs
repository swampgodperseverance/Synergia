using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Items.Accessories
{
    public class HeartofGehenna : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.20f;
            
            player.endurance -= 0.20f;

            player.lifeRegen += 3;
            
            player.statDefense -= 30;
            
            player.GetModPlayer<HeartofGehennaPlayer>().effectActive = true;
        }
    }

    public class HeartofGehennaPlayer : ModPlayer
    {
        public bool effectActive;
        public int timer;
        public bool critReady;
        
        public override void ResetEffects()
        {
            effectActive = false;
        }
        
        public override void PostUpdate()
        {
            if (effectActive)
            {
                timer++;
                
                if (timer >= 1800)
                {
                    timer = 0;
                    critReady = true;
                    

                    CombatText.NewText(Player.getRect(), Color.Red, "Gehenna's Wrath Ready!", true);
                }
            }
            else
            {
                timer = 0;
                critReady = false;
            }
        }
        
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (critReady && effectActive)
            {
                modifiers.SetCrit();
                modifiers.FlatBonusDamage += Player.GetWeaponDamage(Player.HeldItem); // +100% 
                critReady = false;
                timer = 0;
                
                // 
                CombatText.NewText(Player.getRect(), Color.OrangeRed, "Gehenna's Wrath!", true);
            }
        }
        
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (critReady && effectActive && proj.DamageType != DamageClass.Summon)
            {
                modifiers.SetCrit();
                modifiers.FlatBonusDamage += proj.damage; //
                critReady = false;
                timer = 0;
                
      
                CombatText.NewText(Player.getRect(), Color.OrangeRed, "Gehenna's Wrath!", true);
            }
        }
    }
}