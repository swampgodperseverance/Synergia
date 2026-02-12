using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.NPCs;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Synergia.Common.Rarities;
using Synergia.Content.NPCs.Boss.SinlordWyrm;

namespace Synergia.Content.Items.Misc
{
    public class HellwormScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hellworm Scale");
            // Tooltip.SetDefault("Summons the Cogworm in the Underworld");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = ModContent.RarityType<LavaGradientRarity>();
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            bool inUnderworld = player.ZoneUnderworldHeight;
            bool bossExists = NPC.AnyNPCs(ModContent.NPCType<Sinlord>());
            
            return inUnderworld && !bossExists;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
                int type = ModContent.NPCType<Sinlord>();

 
                SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/WormRoar"), player.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), 
                        (int)player.Center.X, (int)player.Center.Y - 200, type);
                    
                    Main.npc[npcID].target = player.whoAmI;
                    
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcID);
                    }
                    
                    string typeName = Main.npc[npcID].TypeName;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
                    }
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(
                            NetworkText.FromKey("Announcement.HasAwoken", Main.npc[npcID].GetTypeNetName()), 
                            new Color(175, 75, 255));
                    }
                }
                else
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }

                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            // Recipe recipe = CreateRecipe();
            // recipe.AddIngredient(ItemID.HellstoneBar, 10);
            // recipe.AddIngredient(ItemID.Obsidian, 20);
            // recipe.AddTile(TileID.Hellforge);
            // recipe.Register();
        }
    }
}