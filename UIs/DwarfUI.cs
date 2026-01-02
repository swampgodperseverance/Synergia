using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items.Tools;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Dataset;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using ValhallaMod.Items.Placeable.Blocks;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Helpers.UIHelper;

namespace Synergia.UIs;
// Some of the code is taken from EAMod
internal class DwarfUI : UIState {

    VanillaItemSlotWrapper itemSlotWeppon;
    VanillaItemSlotWrapper itemSlotPrace;
    Dictionary<int, int> Items = new(){
        {ItemID.MoltenPickaxe, ItemType<CoreburnedPickaxe>()},
        {ItemID.PhoenixBlaster, ItemType<PhoenixDownfall>() },
        //{ItemID.Scorcher, ItemType<ScorcherRequiem>() },
        {120, ItemType<Enfer>() },
    };
    readonly SaveItemPlayer saveItem = Main.LocalPlayer.GetModPlayer<SaveItemPlayer>();
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
        if (saveItem == null) {
            return;
        }
        itemSlotWeppon = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f) {
            Item = saveItem.weaponSlotItem,
            Left = { Pixels = 50 },
            Top = { Pixels = 335 },
            ValidItemFunc = item => item.IsAir || !item.IsAir && GetItem().Contains(item.type),
        };
        itemSlotPrace = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f) {
            Item = saveItem.praceSlotItem,
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
        anvil = Request<Texture2D>(Reassures.Reassures.GetUIElementName("Anvil")).Value;
        reforgeButtonTexture = Request<Texture2D>(Reassures.Reassures.GetUIElementName("NakovalnyaBoga")).Value;
        reforgeButtonTexture_glow = Request<Texture2D>(Reassures.Reassures.GetUIElementName("NakovalnyaBoga_glow")).Value;
        hellsmithBgTexture = Request<Texture2D>(Reassures.Reassures.GetUIElementName("HellsmithBg")).Value;
        hellsmithBarTexture = Request<Texture2D>(Reassures.Reassures.GetUIElementName("HellsmithBar")).Value;
        VanillaItemTexture = Request<Texture2D>($"Terraria/Images/Item_{GetNextItemType(GetItem())}").Value;
        magmaSinstoneTexture = Request<Texture2D>("ValhallaMod/Items/Placeable/Blocks/SinstoneMagma").Value;

        itemSlotWeppon.ItemTypeTextyre = VanillaItemTexture;
        itemSlotPrace.ItemTypeTextyre = magmaSinstoneTexture;
    }
    #endregion
    #region SaveItem
    void CloseUI_SaveOnly() {
        saveItem.weaponSlotItem = itemSlotWeppon.Item.Clone(); 
        saveItem.praceSlotItem = itemSlotPrace.Item.Clone();
    }
    void CloseUI_DropItems() {
        if (!itemSlotWeppon.Item.IsAir) {
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), itemSlotWeppon.Item, itemSlotWeppon.Item.stack);
            itemSlotWeppon.Item.TurnToAir();
            saveItem.weaponSlotItem.TurnToAir();
        }
        if (!itemSlotPrace.Item.IsAir) {
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), itemSlotPrace.Item, itemSlotPrace.Item.stack);
            itemSlotPrace.Item.TurnToAir();
            saveItem.praceSlotItem.TurnToAir();
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
        reforgeButtonAnim.RowCount = 1;
        hellsmithBarTextureAnim.RowCount = 1;
        reforgeButtonAnim.Update();
        hellsmithBarTextureAnim.Init(1, 25);
        spriteBatch.Draw(hellsmithBgTexture, new Vector2(slotX - 28, slotY - 17), Color.White);
        spriteBatch.Draw(anvil, new Vector2(reforgeX - 50, reforgeY + 70), Color.White);
        bool canReforge = Items.ContainsKey(itemSlotWeppon.Item.type) && !itemSlotPrace.Item.IsAir && itemSlotPrace.Item.stack >= 10;
        if (!itemSlotWeppon.Item.IsAir && canReforge) {
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
                    int valueStack, a = isStartForgoten ? 10 : 0;
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
                        if (Items.TryGetValue(itemSlotWeppon.Item.type, out int item)) {
                            if (currentFrame >= 14 && currentFrame <= 16) {
                                Main.LocalPlayer.mouseInterface = false;
                                forgingSuccess = true;
                                forgingEnded = true;

                                ItemLoader.PreReforge(itemSlotWeppon.Item);
                                bool favorited = itemSlotWeppon.Item.favorited;
                                int stack = itemSlotWeppon.Item.stack;

                                Item reforgeItem = new();
                                reforgeItem.SetDefaults(item);

                                itemSlotWeppon.Item = reforgeItem.Clone();
                                itemSlotWeppon.Item.favorited = favorited;
                                itemSlotWeppon.Item.stack = stack;
                                itemSlotWeppon.Item.position = Main.LocalPlayer.Center - new Vector2(itemSlotWeppon.Item.width / 2f, itemSlotWeppon.Item.height / 2f);

                                SoundEngine.PlaySound(SoundID.Item4);
                                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Lime, LocUIKey("DwarfUI", "IsGood"));
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
                                    CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, LocUIKey("DwarfUI", "IsNotGood"));
                                    Main.LocalPlayer.mouseInterface = false;

                                    isStartForgoten = true;
                                }
                            }
                        }
                        else {
                            return;
                        }         
                    }
                }
            }
        }
        string message = LocUIKey("DwarfUI", "Info");
        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, curfont, message, new Vector2(slotX, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
        if (mouseInWepponSlot) {
            Main.hoverItemName = LocUIKey("DwarfUI", "WepponSlotInfo");
        }
        if (mouseInPraceSlot) {
            Main.hoverItemName = LocUIKey("DwarfUI", "PraceSlotInfo");
        }
    }
}