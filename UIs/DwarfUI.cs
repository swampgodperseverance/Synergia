using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;
using ValhallaMod.Items.Placeable.Blocks;
using static Terraria.ModLoader.ModContent;

namespace Synergia.UIs;
// Some of the code is taken from EAMod
internal class DwarfUI : UIState {

    VanillaItemSlotWrapper itemSlotWeppon;
    VanillaItemSlotWrapper itemSlotPrace;
    readonly DwarfPlayer dwarfPlayer = Main.LocalPlayer.GetModPlayer<DwarfPlayer>();
    Texture2D anvil;
    Texture2D hellsmithBgTexture;
    Texture2D magmaSinstoneTexture;
    bool tickPlayed;
    bool mouseInWepponSlot;
    bool mouseInPraceSlot;
    bool hoveringOverReforgeButton;

    #region Button
    AnimationDate reforgeButtonAnim;
    AnimationDate hellsmithBarTextureAnim;
    Texture2D reforgeButtonTexture;
    Texture2D reforgeButtonTexture_glow;
    Texture2D hellsmithBarTexture;
    Texture2D VanillaItemTexture;
    const int slotX = 50;
    const int slotY = 270;
    readonly int reforgeX = slotX + 70;
    readonly int reforgeY = slotY + 40;
    #endregion
    #region LavaBar
    bool forgingActive;    
    int forgingSpeed = 2;
    int forgingFails = 0;
    bool forgingEnded;
    bool forgingSuccess;
    bool isStartForgoten;
    int lastItemType = -1;
    bool needResetForging;
    bool forgingForward = true;
    #endregion

    public override void OnInitialize() {
        if (dwarfPlayer == null) {
            return;
        }
        itemSlotWeppon = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f) {
            Item = dwarfPlayer.weaponSlotItem,
            Left = { Pixels = 50 },
            Top = { Pixels = 335 },
            ValidItemFunc = item => item.IsAir || !item.IsAir && GetItem().Contains(item.type),
        };
        itemSlotPrace = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f) {
            Item = dwarfPlayer.praceSlotItem,
            Left = { Pixels = 100 },
            Top = { Pixels = 335 },
            ValidItemFunc = item => item.IsAir || !item.IsAir && item.type == ItemType<SinstoneMagma>()
        };

        Append(itemSlotWeppon);
        Append(itemSlotPrace);
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        if (Main.LocalPlayer.talkNPC == -1 || Main.npc[Main.LocalPlayer.talkNPC].type != NPCType<Alchemist>()) {
            CloseUI_DropItems();
            GetInstance<Synergia>().DwarfUserInterface.SetState(null);
        }
    }
    #region Util
    void ResetForgingState() {
        forgingActive = false;
        forgingEnded = false;
        forgingSuccess = false;
        forgingFails = 0;
        forgingSpeed = 2;
        isStartForgoten = false;
        hellsmithBarTextureAnim.Reset();
        hellsmithBarTextureAnim.Init(1, 25);
        needResetForging = false;
    }
    void Textures() {
        anvil = Request<Texture2D>(Synergia.GetUIElementName("Anvil")).Value;
        reforgeButtonTexture = Request<Texture2D>(Synergia.GetUIElementName("NakovalnyaBoga")).Value;
        reforgeButtonTexture_glow = Request<Texture2D>(Synergia.GetUIElementName("NakovalnyaBoga_glow")).Value;
        hellsmithBgTexture = Request<Texture2D>(Synergia.GetUIElementName("HellsmithBg")).Value;
        hellsmithBarTexture = Request<Texture2D>(Synergia.GetUIElementName("HellsmithBar")).Value;
        VanillaItemTexture = Request<Texture2D>($"Terraria/Images/Item_{Helpers.UIHelper.GetNextItemType(GetItem())}").Value;
        magmaSinstoneTexture = Request<Texture2D>("ValhallaMod/Items/Placeable/Blocks/SinstoneMagma").Value;

        itemSlotWeppon.ItemTypeTextyre = VanillaItemTexture;
        itemSlotPrace.ItemTypeTextyre = magmaSinstoneTexture;
    }
    static bool MousePositionInUI(int startX, int endX, int statrtY, int endY) => Main.mouseX > startX && Main.mouseX < endX && Main.mouseY > statrtY && Main.mouseY < endY && !PlayerInput.IgnoreMouseInterface;
    #endregion
    #region SaveItem
    void CloseUI_SaveOnly() {
        dwarfPlayer.weaponSlotItem = itemSlotWeppon.Item.Clone(); 
        dwarfPlayer.praceSlotItem = itemSlotPrace.Item.Clone();
    }
    void CloseUI_DropItems() {
        if (!itemSlotWeppon.Item.IsAir) {
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), itemSlotWeppon.Item, itemSlotWeppon.Item.stack);
            itemSlotWeppon.Item.TurnToAir();
            dwarfPlayer.weaponSlotItem.TurnToAir();
        }
        if (!itemSlotPrace.Item.IsAir) {
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), itemSlotPrace.Item, itemSlotPrace.Item.stack);
            itemSlotPrace.Item.TurnToAir();
            dwarfPlayer.praceSlotItem.TurnToAir();
        }
    }
    #endregion
    #region ItemType
    static List<int> GetItem() {
        List<int> idList = [
            ItemID.CopperOre,
            ItemID.TinOre,
            2,
            11,
            3507,
            4956,
            3509,
        ];
        return idList;
    }
    #endregion
    protected override void DrawSelf(SpriteBatch spriteBatch) {
        base.DrawSelf(spriteBatch);
        Textures();
        CloseUI_SaveOnly();
        if (!itemSlotWeppon.Item.IsAir) {
            if (itemSlotWeppon.Item.type != lastItemType) {
                lastItemType = itemSlotWeppon.Item.type;
                needResetForging = true;
            }
        }
        else if (lastItemType != -1) {
            lastItemType = -1;
            needResetForging = true;
        }
        if (needResetForging) { ResetForgingState(); }
        DynamicSpriteFont curfont = FontAssets.ItemStack.Value;
        Main.hidePlayerCraftingMenu = true;
        mouseInWepponSlot = MousePositionInUI(slotX, slotX + 40, slotY + 60, slotY + 110);
        mouseInPraceSlot = MousePositionInUI(slotX + 50, slotX + 90, slotY + 60, slotY + 110);
        reforgeButtonAnim.Init(1, 4);
        reforgeButtonAnim.Update();
        hellsmithBarTextureAnim.Init(1, 25);
        spriteBatch.Draw(hellsmithBgTexture, new Vector2(slotX - 28, slotY - 17), Color.White);
        spriteBatch.Draw(anvil, new Vector2(reforgeX - 50, reforgeY + 70), Color.White);
        if (!itemSlotWeppon.Item.IsAir && (!itemSlotPrace.Item.IsAir && itemSlotPrace.Item.stack >= 10)) {
            Vector2 reforgeButtonOrigin = reforgeButtonAnim.GetSource(reforgeButtonTexture).Size() / 2f;
            spriteBatch.Draw(reforgeButtonTexture, new Vector2(reforgeX + 75, reforgeY + 85), reforgeButtonAnim.GetSource(reforgeButtonTexture), Color.White, 0f, reforgeButtonOrigin, 1f, reforgeButtonAnim.Effects, 0f); // layer: 0 -> 1;
            Vector2 barOrigin = hellsmithBarTextureAnim.GetSource(hellsmithBarTexture).Size() / 2f;
            spriteBatch.Draw(hellsmithBarTexture, new Vector2(reforgeX + 85, reforgeY + 45), hellsmithBarTextureAnim.GetSource(hellsmithBarTexture), Color.White, 0f, barOrigin, 1f, hellsmithBarTextureAnim.Effects, 0f);
            hoveringOverReforgeButton = MousePositionInUI(reforgeX + 55, reforgeX + 90, reforgeY + 75, reforgeY + 105);
            if (hoveringOverReforgeButton) {
                if (needResetForging) {
                    ResetForgingState();
                    hellsmithBarTextureAnim.StartAnimation = true; 
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
                Vector2 reforgeButtonTexture_glowOrigin = reforgeButtonAnim.GetSource(reforgeButtonTexture_glow).Size() / 2f;
                spriteBatch.Draw(reforgeButtonTexture_glow, new Vector2(reforgeX + 75, reforgeY + 85), reforgeButtonAnim.GetSource(reforgeButtonTexture_glow), Color.Gold, 0f, reforgeButtonTexture_glowOrigin, 1f, reforgeButtonAnim.Effects, 1f);
                Main.hoverItemName = Language.GetTextValue("LegacyInterface.19");
                if (!tickPlayed) { SoundEngine.PlaySound(SoundID.MenuTick); }
                tickPlayed = true;
                Main.LocalPlayer.mouseInterface = true;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    reforgeButtonAnim.StartAnimation = true;
                    hellsmithBarTextureAnim.StartAnimation = true;

                    ItemLoader.PreReforge(itemSlotPrace.Item);
                    bool valueFavorited = itemSlotPrace.Item.favorited;
                    int valueStack, a;
                    if (isStartForgoten == false) { a = 10; }
                    else { a = 0; }
                    valueStack = itemSlotPrace.Item.stack - a;

                    itemSlotPrace.Item.favorited = valueFavorited;
                    itemSlotPrace.Item.stack = valueStack;
                    SoundEngine.PlaySound(SoundID.Item37);
                }
                else { 
                    tickPlayed = false; 
                }
            }
            if (hellsmithBarTextureAnim.StartAnimation) {
                if (hellsmithBarTextureAnim.Frame.CurrentRow < 5) {
                    hellsmithBarTextureAnim.Update();
                }
                else {
                    forgingActive = true;
                    hellsmithBarTextureAnim.StartAnimation = false;
                }
                isStartForgoten = true;
            }
            if (forgingActive && !forgingEnded) {
                if (Main.GameUpdateCount % forgingSpeed == 0) {
                    if (forgingForward) { hellsmithBarTextureAnim.Frame.CurrentRow++; }
                    else { hellsmithBarTextureAnim.Frame.CurrentRow--; }
                    if (hellsmithBarTextureAnim.Frame.CurrentRow >= 24) {
                        hellsmithBarTextureAnim.Frame.CurrentRow = 24;
                        forgingForward = false;
                    }
                    else if (hellsmithBarTextureAnim.Frame.CurrentRow <= 5) {
                        hellsmithBarTextureAnim.Frame.CurrentRow = 5;
                        forgingForward = true;
                    }
                }
                if (hoveringOverReforgeButton) {
                    if (Main.mouseLeft && Main.mouseLeftRelease) {
                        int currentFrame = hellsmithBarTextureAnim.Frame.CurrentRow;
                        if (currentFrame >= 14 && currentFrame <= 16) {
                            Main.LocalPlayer.mouseInterface = false;
                            forgingSuccess = true;
                            forgingEnded = true;

                            int itemType = GetItem().IndexOf(itemSlotWeppon.Item.type);
                            int counterPart = GetItem()[itemType + ((GetItem().IndexOf(itemSlotWeppon.Item.type) + 1) % 2 == 0 ? -1 : 1)];

                            ItemLoader.PreReforge(itemSlotWeppon.Item);
                            bool favorited = itemSlotWeppon.Item.favorited;
                            int stack = itemSlotWeppon.Item.stack;

                            Item reforgeItem = new();
                            reforgeItem.SetDefaults(counterPart);

                            itemSlotWeppon.Item = reforgeItem.Clone();
                            itemSlotWeppon.Item.favorited = favorited;
                            itemSlotWeppon.Item.stack = stack;
                            itemSlotWeppon.Item.position = Main.LocalPlayer.Center - new Vector2(itemSlotWeppon.Item.width / 2f, itemSlotWeppon.Item.height / 2f);

                            SoundEngine.PlaySound(SoundID.Item4);
                            CombatText.NewText(Main.LocalPlayer.getRect(), Color.Lime, "Успех!");
                            PopupText.NewText(PopupTextContext.RegularItemPickup, itemSlotWeppon.Item, itemSlotWeppon.Item.stack, noStack: true);
                            Main.LocalPlayer.mouseInterface = false;

                            isStartForgoten = false;
                        }
                        else {
                            forgingFails++;
                            forgingSpeed += 2;
                            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot);
                            if (forgingFails >= 3) {
                                forgingEnded = true;
                                itemSlotWeppon.Item.TurnToAir();

                                SoundEngine.PlaySound(SoundID.Item14);
                                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, "Предмет разрушен!");
                                Main.LocalPlayer.mouseInterface = false;

                                isStartForgoten = true;
                            }
                        }
                    }
                }
            }
        }
        string message;
        if (Language.ActiveCulture.Name == "ru-RU") {
            message = $"Вставте оружие в 1 слот \nВставте Magma Sinstone \nв слот 2";
        }
        else if (Language.ActiveCulture.Name == "en-US") {
            message = $"place weapons in 1 slot, \nput Magma Sinstone in slot 2";
        }
        else {
            // Localizatin key
            message = $"place weapons in 1 slot, \nput Magma Sinstone in slot 2";
        }
        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, curfont, message, new Vector2(slotX, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
        if (mouseInWepponSlot) {
            Main.hoverItemName = "вфывфывфывывфы";
        }
        if (mouseInPraceSlot) {
            Main.hoverItemName = "dasdads";
        }
    }
}
class DwarfPlayer : ModPlayer {
    public Item weaponSlotItem = new();
    public Item praceSlotItem = new();

    public override void Initialize() {
        weaponSlotItem = new Item();
        praceSlotItem = new Item();
        weaponSlotItem.TurnToAir();
        praceSlotItem.TurnToAir();
    }
    public override void SaveData(TagCompound tag) {
        // public const string ModName = "BaseModModName";
        if (weaponSlotItem is not null) {
            tag.Add(Synergia.ModName + "WeaponSlotItem" + nameof(weaponSlotItem), ItemIO.Save(weaponSlotItem));
        }
        if (weaponSlotItem is not null) {
            tag.Add(Synergia.ModName + "PraceSlotItem" + nameof(praceSlotItem), ItemIO.Save(praceSlotItem));
        }
    }
    public override void LoadData(TagCompound tag) {
        if (tag.TryGet(Synergia.ModName + "WeaponSlotItem" + nameof(weaponSlotItem), out TagCompound dye1)) {
            weaponSlotItem = ItemIO.Load(dye1);
        }
        if (tag.TryGet(Synergia.ModName + "PraceSlotItem" + nameof(praceSlotItem), out TagCompound dye2)) {
            praceSlotItem = ItemIO.Load(dye2);
        }
    }
}
struct AnimationDate {
    public SpriteFrame Frame;
    public bool StartAnimation;
    public int FrameTimer;
    public SpriteEffects Effects;
    public const int FrameDuration = 7;

    public void Init(byte columns, byte rows) {
        if (Frame.ColumnCount == 0 || Frame.RowCount == 0) {
            Frame = new SpriteFrame(columns, rows) {
                CurrentColumn = 0,
                CurrentRow = 0
            };
        }
    }
    public Rectangle GetSource(Texture2D texture) => Frame.GetSourceRectangle(texture);
    public void Update() {
        if (StartAnimation) {
            FrameTimer++;
            if (FrameTimer >= FrameDuration) {
                FrameTimer = 0;
                Frame.CurrentRow = (byte)((Frame.CurrentRow + 1) % Frame.RowCount);
                if (Frame.CurrentRow == Frame.RowCount - 1) {
                    StartAnimation = false;
                    Frame.CurrentRow = 0;
                }
            }
        }
    }
    public void Reset() {
        StartAnimation = false;
        Frame.CurrentRow = 0;
        Frame.CurrentColumn = 0;
        FrameTimer = 0;
    }
}