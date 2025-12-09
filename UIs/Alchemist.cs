using Synergia.GraphicsSetting;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.UIs {
    [AutoloadHead]
    public class Alchemist : ModNPC
    {        
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoTownNPCHappiness[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 32;
            NPC.height = 42;
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.defense = 20;
            NPC.lifeMax = 1000;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                ModContent.GetInstance<Synergia>().DwarfUserInterface.SetState(new DwarfUI());
            }
        }
        public void UpdatePosition()
        {
            if (Main.player[Main.myPlayer].position.X >= NPC.position.X)
                NPC.spriteDirection = -1;
            else
                NPC.spriteDirection = 1;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return false;
        }
        public override void AI() {
            //string name = SynegiyGraphics.ARMORRADAR;

            //Filters.Scene[ARMORRADAR].Activate(NPC.position);
            if (NPC.homeTileX == -1 || NPC.homeTileY == -1)
            {
                NPC.homeTileX = NPC.Center.ToTileCoordinates().X;
                NPC.homeTileY = NPC.Center.ToTileCoordinates().Y;
            }
            NPC.dontTakeDamage = true;
            NPC.breath = 100;
            NPC.life = NPC.lifeMax;
            if (NPC.oldVelocity.X != 0f)
            {
                NPC.velocity.X = 0f;
            }
            if (Main.LocalPlayer.talkNPC != -1)
            {
                if (Main.npc[Main.LocalPlayer.talkNPC].whoAmI == NPC.whoAmI)
                {
                    UpdatePosition();
                }
            }
        }
    }
}