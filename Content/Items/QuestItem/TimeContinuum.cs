
using Avalon.Items.Material.Shards;
using Consolaria.Content.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;

namespace Synergia.Content.Items.QuestItem
{
    public class TimeContinuum : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.questItem = true;
            Item.rare = ItemRarityID.Quest;
            Item.width = 40;
            Item.height = 25;
        }

        public override void HoldItem(Player player)
        {
            TimeStopSystem.TimeStopped = true;
        }
   
        public override void AddRecipes()
        {
            var recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<TatteredBook>(), 1)
                .AddIngredient(ModContent.ItemType<FragmentsOfTime>(), 6)
                .AddIngredient(ModContent.ItemType<SoulofBlight>(), 5)
                .AddTile(ModContent.TileType<Avalon.Tiles.TomeForge>());

            if (ModLoader.TryGetMod("PrimeRework", out Mod primeReworkMod))
            {
                if (primeReworkMod.TryFind<ModItem>("SoulofFreight", out var freight) &&
                    primeReworkMod.TryFind<ModItem>("SoulofPlight", out var plight) &&
                    primeReworkMod.TryFind<ModItem>("SoulofDight", out var dight))
                {
                    recipe
                        .AddIngredient(freight.Type, 3)
                        .AddIngredient(dight.Type, 3);
                }
            }

            recipe.Register();
        }
    
    }
    public class TimeStopSystem : ModSystem
    {
        public static bool TimeStopped;

        public override void PreUpdateWorld()
        {
            if (!TimeStopped)
                return;
            if (Main.dayTime)
                Main.time = 27000;
            else
                Main.time = 16200;

            // NPC
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.type == NPCID.TargetDummy)
                    continue;

                npc.velocity = Vector2.Zero;
                for (int i = 0; i < npc.ai.Length; i++)
                    npc.ai[i] = 0;
            }
            foreach (Projectile proj in Main.projectile)
            {
                if (!proj.active)
                    continue;

                if (proj.hostile)
                {
                    proj.velocity = Vector2.Zero;
                    proj.timeLeft++; 
                }
            }
        }

        public override void PostUpdateEverything()
        {
            TimeStopped = false;
        }
    }
}