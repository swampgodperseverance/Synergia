// Code by 𝒜𝑒𝓇𝒾𝓈
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;

namespace Synergia.Content.NPCs {
    [AutoloadHead]
    public class HellDwarf : ModNPC {
        public override string LocalizationCategory => Category(CategoryName.NPC);
        public override List<string> SetNPCNameList() => [this.GetLocalizedValue("Name.Skyzephire"), this.GetLocalizedValue("Name.Thorin"), this.GetLocalizedValue("Name.Belegar"), this.GetLocalizedValue("Name.Dragan"), this.GetLocalizedValue("Name.Wulfrik")];
        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 550;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
        }
        public override void SetDefaults() {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 30;
            NPC.height = 44;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 20;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.GoblinTinkerer;
        }
        public override string GetChat() {
            return Main.rand.Next(maxValue: 9) switch {
                0 => GetText("Text"),
                1 => GetText("Text1"),
                2 => GetText("Text2"),
                3 => GetText("Text3"),
                4 => GetText("Text4"),
                5 => GetText("Text5"),
                6 => GetText("Text6"),
                7 => GetText("Text7"),
                8 => GetText("Text8"),
                _ => Language.GetTextValue("tModLoader.DefaultTownNPCChat"),
            };
        }
        public override void AddShops() {
            NPCShop shop = new(Type, Name);
            shop.Add(1);
            shop.Register();
        }
        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) {
            itemWidth = 32;
            itemHeight = 32;
        }
        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset) {
            Main.GetItemDrawFrame(ItemID.GoldHammer, out item, out itemFrame);
            if (NPC.ai[1] > (float)NPCID.Sets.AttackTime[NPC.type] * 0.66f) {
                offset = new Vector2(offset.X - 7, offset.Y + 24);
            }
            else {
                offset = new Vector2(offset.X, offset.Y + 7);
            }
        }
        string GetText(string key) => LocNPCKey(Name, "Chat." + key);
    }
    public class DwarfEmote : ModEmoteBubble {
        public override string Texture => (GetType().Namespace + "." + "Emote").Replace('.', '/');
        public override void SetStaticDefaults() {
            AddToCategory(EmoteID.Category.Town);
        }
    }
}