using Bismuth;
using Bismuth.Utilities;
using log4net.Core;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForRPGSystem : ModSystem {

        Hook book;
        Hook drawLevel;
        Hook levelUP;

        Texture2D texture = null;
        Vector2 vector2 = Vector2.Zero;
        bool flag = false;

        delegate void orig_Quest_DrawBook(Quests quests, SpriteBatch sb);
        delegate void new_Quest_DrawBook(orig_Quest_DrawBook orig, Quests quests, SpriteBatch sb);

        delegate void orig_Levels_Draw(Levels level, SpriteBatch spriteBatch);
        delegate void new_Levels_Draw(orig_Levels_Draw orig, Levels level, SpriteBatch spriteBatch);

        delegate void orig_Levels_LevelUP(Levels level);
        delegate void new_Levels_LevelUP(orig_Levels_LevelUP orig, Levels level);

        public override void Load() {
            MethodInfo info = typeof(Quests).GetMethod(nameof(Quests.DrawBook));
            book = new(info, (new_Quest_DrawBook)NewQuestBook);
            info = typeof(Levels).GetMethod(nameof(Levels.DRAW));
            drawLevel = new(info, (new_Levels_Draw)NewDrawLevel);
            info = typeof(Levels).GetMethod(nameof(Levels.LEVELUP));
            levelUP = new(info, (new_Levels_LevelUP)NewLevelUP);
        }

        void NewQuestBook(orig_Quest_DrawBook orig, Quests quests, SpriteBatch sb) {
            if (quests.Player.GetModPlayer<BismuthPlayer>().PlayerClass == 6) {
                if (texture == null) {
                    texture = quests.ActualPanel;
                    flag = quests.treeflag;
                }
            }
            orig(quests, sb);
            if (Quests.currentpage == 4) {
                if (quests.Player.GetModPlayer<BismuthPlayer>().PlayerClass == 6) {
                    quests.treeflag = false;
                    quests.ActualPanel = Request<Texture2D>("Synergia/Assets/Textures/Blank").Value;
                    Vector2 textPos = new Vector2(Quests.bookcoord.X + 205, Quests.bookcoord.Y) + Request<Texture2D>("Bismuth/UI/AdventurersBookPageEmpty").Value.Size() / 2f;
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Bismuth.Bismuth.Adonais, SynergiaLocKey("UI.BismuthBook.NoSupport"), textPos, Color.White, 0f, FontAssets.MouseText.Value.MeasureString(SynergiaLocKey("UI.BismuthBook.NoSupport")) / 2f, Vector2.One);
                }
                else if (texture != null) {
                    quests.ActualPanel = texture;
                    quests.treeflag = flag;
                }
            }
        }
        void NewDrawLevel(orig_Levels_Draw orig, Levels level, SpriteBatch spriteBatch) {
            //if (level.Player.GetModPlayer<BismuthPlayer>().PlayerClass == 6) { return; }
            //else { orig(level, spriteBatch); }
            return;
        }
        void NewLevelUP(orig_Levels_LevelUP orig, Levels level) {
            //if (level.Player.GetModPlayer<BismuthPlayer>().PlayerClass == 6) { return; }
            //else { orig(level); }
            return;
        }

        public override void Unload() {
            base.Unload();
        }
    }
}