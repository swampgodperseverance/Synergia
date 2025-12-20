using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Armor.Melee;

[AutoloadEquip(EquipType.Head)]
public sealed class ThunderHead : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 20;
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 17;
        Item.value = Item.sellPrice(0, 3, 4, 50);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
        player.GetCritChance(DamageClass.Melee) += 17f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<ThunderBody>()
            && legs.type == ModContent.ItemType<ThunderLegs>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus =
            "Melee attacks release lightning on hit\n" +
            "You leave a trail of energy around your body\n" +
            "Taking damage grants additional 0.5s immunity";
        player.GetDamage(DamageClass.Melee) += 0.10f;
        player.GetModPlayer<ThunderArmorPlayer>().thunderSet = true;
    }

    public class ThunderArmorPlayer : ModPlayer
    {
        public bool thunderSet = false;

        public override void ResetEffects()
        {
            thunderSet = false;
        }

        public override void PostUpdateRunSpeeds()
        {
            if (!thunderSet || Player.velocity.Length() <= 2f) return;

            if (Main.rand.NextBool(2)) 
            {
                Vector2[] bodyPositions = {
                    Player.Top + Main.rand.NextVector2Circular(8f, 8f),      
                    Player.Center + Main.rand.NextVector2Circular(12f, 12f), 
                    Player.Bottom + Main.rand.NextVector2Circular(10f, 10f) 
                };

                foreach (Vector2 pos in bodyPositions)
                {
                    Dust d = SpawnTopazDust(pos);
                    d.velocity = Player.velocity * 0.15f + Main.rand.NextVector2Circular(1.5f, 1.5f);
                }
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (thunderSet && item.DamageType == DamageClass.Melee)
            {
                SpawnTopazBurst(target.Center);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (thunderSet && proj.CountsAsClass(DamageClass.Melee))
            {
                SpawnTopazBurst(target.Center);
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (thunderSet)
            {
                Player.immune = true;
                Player.immuneTime += 30;
            }
        }


        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (thunderSet)
            {
                Player.immune = true;
                Player.immuneTime += 30; 
            }
        }

        private void SpawnTopazBurst(Vector2 position)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(8f, 8f);
                Dust d = SpawnTopazDust(position + vel * 1.5f);
                d.velocity = vel;
                d.scale *= 1.3f;
            }
        }

        private Dust SpawnTopazDust(Vector2 position)
        {
            Dust dust = Dust.NewDustPerfect(position, DustID.GemTopaz);
            dust.noGravity = true;
            dust.scale = Main.rand.NextFloat(0.9f, 1.4f);
            dust.fadeIn = 1.2f;
            dust.alpha = 80;
            return dust;
        }
    }
}