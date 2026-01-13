//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using MonoMod.RuntimeDetour;
//using ReLogic.Content;
//using System;
//using System.Linq;
//using System.Reflection;
//using Terraria;
//using Terraria.GameContent.UI.Elements;
//using Terraria.ModLoader;
//using Terraria.ModLoader.UI;
//using Terraria.UI;
//using static Synergia.Reassures.Reassures;

//namespace Synergia.Common.ModSystems.Hooks.Ons {
//    public class On_NewUIHook : ModSystem {
//        // Some code is taken from ModLiquid and CalamityFables
//        Hook iconAchieve;
//        Hook animationIcon;

//        Asset<Texture2D> achieve;
//        Asset<Texture2D> animation;

//        delegate void Orig_OnInitialize(UIModItem self);
//        delegate void OnInitialize_Detour(Orig_OnInitialize orig, UIModItem self);
//        delegate void Orig_Draw(UIModItem self, SpriteBatch sprite);
//        delegate void Draw_Detour(Orig_Draw orig, UIModItem self, SpriteBatch sprite);

//        static readonly Type UIModItemType = typeof(ModItem).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
//        static readonly FieldInfo ModNameElement = UIModItemType?.GetField("_modName", BindingFlags.Instance | BindingFlags.NonPublic);
//        static readonly PropertyInfo ModNameProperty = UIModItemType?.GetProperty("ModName", BindingFlags.Instance | BindingFlags.Public);
//        static readonly FieldInfo ModIconField = UIModItemType?.GetField("_modIcon", BindingFlags.Instance | BindingFlags.NonPublic);

//        public override void Load() {
//            LoadIcon();
//        }
//        void LoadIcon() {
//            MethodInfo ExtraMountCavesGeneratorInfo = typeof(UIModItem).GetMethod(nameof(UIModItem.OnInitialize), BindingFlags.Public | BindingFlags.Instance);
//            iconAchieve = new Hook(ExtraMountCavesGeneratorInfo, (OnInitialize_Detour)AddNewIcon);

//            MethodInfo target = typeof(UIModItem).GetMethod(nameof(UIModItem.Draw), BindingFlags.Public | BindingFlags.Instance);
//            animationIcon = new Hook(target, (Draw_Detour)AddNewAnimationIcon);
//        }
//        public override void PostSetupContent() {
//            achieve = ModContent.Request<Texture2D>(GetUIElementName("AchiveIcon"));
//            animation = ModContent.Request<Texture2D>(GetTexturesElementName("AnimationIcon"));
//        }
//        void AddNewIcon(Orig_OnInitialize orig, UIModItem self) {
//            orig.Invoke(self);
//            Assembly ass = typeof(Mod).Assembly;

//            if (ModLoader.TryGetMod(self.ModName, out var loadedMod)) {
//                int achieveCount = loadedMod.GetContent<ModAchievement>().Count();

//                if (achieveCount > 0) {
//                    int baseOffset = -40;
//                    void ChangeOffset(int modCount) {
//                        if (modCount > 0) {
//                            baseOffset -= 18;
//                        }
//                    }

//                    ChangeOffset(loadedMod.GetContent<ModItem>().Count());
//                    ChangeOffset(loadedMod.GetContent<ModNPC>().Count());
//                    ChangeOffset(loadedMod.GetContent<ModTile>().Count());
//                    ChangeOffset(loadedMod.GetContent<ModWall>().Count());
//                    ChangeOffset(loadedMod.GetContent<ModBuff>().Count());
//                    ChangeOffset(loadedMod.GetContent<ModMount>().Count());

//                    Type type_UIHoverImage = ass.GetType("Terraria.ModLoader.UI.UIHoverImage");

//                    object UIHoverImage = Activator.CreateInstance(type_UIHoverImage, achieve, "Mod Achieve" + " " + achieveCount.ToString());
//                    FieldInfo field_Left = type_UIHoverImage.GetField("Left", BindingFlags.Public | BindingFlags.Instance);
//                    field_Left.SetValue(UIHoverImage, new StyleDimension { Percent = 1f, Pixels = baseOffset });
//                    self.Append((UIElement)UIHoverImage);
//                }
//            }
//            if (self.ModName == "Test") {
//                AddNewColorForName(self);
//            }
//        }
//        static void AddNewColorForName(UIModItem self) {
//            object convertedSelf = Convert.ChangeType(self, UIModItemType);
//            object potentialModName = ModNameProperty.GetValue(convertedSelf);
//            if (potentialModName == null || potentialModName is not string _) {
//                return;
//            }
//            object potentiallyTheIcon = ModIconField.GetValue(convertedSelf);
//            if (potentiallyTheIcon is UIImage modIconImage) {
//                DrawNewTextInUI addedDrawLogic = new((UIText)ModNameElement.GetValue(convertedSelf));
//                modIconImage.Append(addedDrawLogic);
//                modIconImage.Color = Color.Transparent;
//            }
//        }
//        void AddNewAnimationIcon(Orig_Draw orig, UIModItem self, SpriteBatch spriteBatch) {
//            orig(self, spriteBatch);
//            if (self.ModName == "Test") {
//                CalculatedStyle style = self.GetInnerDimensions();
//                Texture2D icon = animation.Value;
//                Vector2 offset = new(style.Width - 112, style.Height - 38);
//                Vector2 pos = style.Position() + offset;
//                Vector2 posIcon = new(pos.X - 428.5f, pos.Y - 42.5f);

//                int frame = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
//                spriteBatch.Draw(icon, posIcon, icon.Frame(1, 4, 0, frame), Color.White);
//            }
//        }
//        public override void Unload() {
//            iconAchieve?.Dispose();
//            iconAchieve = null;
//        }
//    }
//    public class DrawNewTextInUI : UIElement {
//        public UIText ModName;

//        public DrawNewTextInUI(UIText nameUI) {
//            Width.Set(80, 0f);
//            Height.Set(80, 0f);

//            ModName = nameUI;
//        }
//        public override void Update(GameTime gameTime) {
//            if (ModName == null)
//                return;
//            Color deepTeal = new(150, 6, 153);
//            Color glowingGold = new Color(200, 120, 012) * 0.6f;

//            ModName.TextColor = deepTeal;
//            ModName.ShadowColor = glowingGold with { A = (byte)((0.5f + 0.5f * (float)Math.Sin(Main.GlobalTimeWrappedHourly)) * 20) };
//        }
//    }
//}