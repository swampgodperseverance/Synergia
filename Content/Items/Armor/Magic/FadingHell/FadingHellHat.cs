using System;
using System.Collections.Generic;
using rail;
using ReLogic.Content;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Common.ModSystems;
using Synergia.Common.Rarities;
using Synergia.Common.SUtils;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Head)]
public sealed class FadingHellHat : ModItem {
    private int equipSlot;
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        for (int i = 0; i < 3; i++)
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_Head{FadingHellArmorVisual.TextureSuffix[i]}", EquipType.Head, this, $"FadingHellHat_{FadingHellArmorVisual.TextureSuffix[i]}");
    }
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
        if (Main.netMode == NetmodeID.Server)
            return;

        equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.PreventBeardDraw[equipSlot] = true;
        for (int i = 0; i < 3; i++)
        {
            equipSlot = EquipLoader.GetEquipSlot(Mod, $"FadingHellHat_{FadingHellArmorVisual.TextureSuffix[i]}", EquipType.Head);
            ArmorIDs.Head.Sets.PreventBeardDraw[equipSlot] = true;
        }
    }
    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 20;

        Item.rare = ModContent.RarityType<CoreburnedRarity>();
        Item.defense = 20;
        Item.value = Item.sellPrice(0, 6, 50, 0);
    }
    public override void UpdateEquip(Player player) {
        player.GetDamage(DamageClass.Magic) += 0.09f;
        player.GetCritChance(DamageClass.Magic) += 9f;
        player.statManaMax2 += 40;
        player.manaCost -= 0.18f;
    }
    public override bool IsArmorSet(Item head, Item body, Item legs) =>
        head.type == Type &&
        body.type == ModContent.ItemType<FadingHellChestplate>() &&
        legs.type == ModContent.ItemType<FadingHellPants>();
    public override void UpdateArmorSet(Player player)
    {
        List<string> keys = VanillaKeybinds.ArmorSetBonusActivation.GetAssignedKeys();
        string keyname = keys.Count > 0 ? keys[0] : "UNBOUND";
        player.setBonus = string.Format(LocUtil.ItemTooltip(LocUtil.ARM, "FadingHellSetBonus"), keyname);
        player.GetModPlayer<FadingHellPlayer>().isSetBonus = true;
    }
}

public class FadingHellHat_MaskDraw : PlayerDrawLayer
{
    private const int FramesAmount = 20;
    private const int TypesAmount = 4;
    private Asset<Texture2D> maskOpened, maskClosed;
    public override void Load()
    {
        maskOpened = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellHat_MaskOpened");
        maskClosed = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellHat_MaskClosed");
    }
    public override void Unload()
    {
        maskOpened = null;
        maskClosed = null;
    }
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.armor[0].type == ModContent.ItemType<FadingHellHat>() && drawInfo.drawPlayer.armor[10].type == ItemID.None
            || drawInfo.drawPlayer.armor[10].type == ModContent.ItemType<FadingHellHat>();
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.hideEntirePlayer)
            return;

        Player player = drawInfo.drawPlayer;
        FadingHellPlayer fadingHellPlayer = player.GetModPlayer<FadingHellPlayer>();
        if (drawInfo.shadow != 0f || player.dead)
            return;

        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3);
        Rectangle bodyFrame = player.bodyFrame;
        Texture2D maskTexture = maskOpened.Value;
        float height = maskTexture.Height / FramesAmount;
        Vector2 origin = new Vector2(maskTexture.Width, height) / 2f;
        if (fadingHellPlayer.ShouldFireBeDrawn()){
            maskTexture = maskClosed.Value;
            bodyFrame.X = Math.Clamp((int)fadingHellPlayer.currentFireType - 1, 0, TypesAmount) * bodyFrame.Width;
            origin = new Vector2(maskTexture.Width / TypesAmount, height) / 2f;
        }
        DrawData drawData = new DrawData(
            maskTexture,
            new Vector2(x, y),
            new Rectangle?(bodyFrame),
            drawInfo.colorArmorHead,
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect,
            0
        );
        int shader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[0].type);
        drawData.shader = shader;
        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class FadingHellHat_CandlewickDraw : PlayerDrawLayer
{
    private const int CandlewickOffsetY = -12;
    private const int FramesAmount = 20;
    private Asset<Texture2D> candlewickTexture;
    public override void Load()
    {
        candlewickTexture = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellHat_Candlewick");
    }
    public override void Unload()
    {
        candlewickTexture = null;
    }
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.armor[0].type == ModContent.ItemType<FadingHellHat>() && drawInfo.drawPlayer.armor[10].type == ItemID.None
            || drawInfo.drawPlayer.armor[10].type == ModContent.ItemType<FadingHellHat>();
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.hideEntirePlayer)
            return;

        Player player = drawInfo.drawPlayer;
        if (drawInfo.shadow != 0f || player.dead)
            return;

        if (player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn())
            return;

        Texture2D candlewickTex = candlewickTexture.Value;
        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3 + CandlewickOffsetY);
        Rectangle bodyFrame = player.bodyFrame;
        float height = candlewickTex.Height / FramesAmount;
        Vector2 origin = new Vector2(candlewickTex.Width, height) / 2f;
        DrawData drawData = new DrawData(
            candlewickTex,
            new Vector2(x, y),
            new Rectangle?(bodyFrame),
            drawInfo.colorArmorHead,
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect,
            0
        );
        int shader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[0].type);
        drawData.shader = shader;
        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class FadingHellHat_FlameDraw : PlayerDrawLayer
{
    private readonly Vector2 FlameOffset = new(-3, -26);
    private const int FramesAmount = 8;
    private const int TypesAmount = 4;
    private Asset<Texture2D> flameTexture;
    public override void Load()
    {
        flameTexture = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellHat_Flame");
    }
    public override void Unload()
    {
        flameTexture = null;
    }
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.armor[0].type == ModContent.ItemType<FadingHellHat>() && drawInfo.drawPlayer.armor[10].type == ItemID.None
            || drawInfo.drawPlayer.armor[10].type == ModContent.ItemType<FadingHellHat>();
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.hideEntirePlayer)
            return;

        Player player = drawInfo.drawPlayer;
        if (drawInfo.shadow != 0f || player.dead)
            return;

        if (!player.GetModPlayer<FadingHellPlayer>().ShouldFireBeDrawn())
            return;

        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X + FlameOffset.X * player.direction);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3 + FlameOffset.Y);
        ulong seed = (ulong)(player.miscCounter / 5);
        int frameCount = (int)seed % FramesAmount;
        Texture2D flameTex = flameTexture.Value;
        int frameWidth = flameTex.Width / TypesAmount;
        int frameHeight = flameTex.Height / FramesAmount;
        Rectangle bodyFrame = new Rectangle(
            frameWidth * Math.Clamp((int)player.GetModPlayer<FadingHellPlayer>().currentFireType - 1, 0, TypesAmount),
            frameHeight * frameCount,
            frameWidth,
            frameHeight
        );
        Vector2 origin = bodyFrame.Size() / 2f;
        int walkFrame = player.bodyFrame.Y / player.bodyFrame.Height;
        int walkOffset = (walkFrame > 6 && walkFrame < 10) || (walkFrame > 13 && walkFrame < 17) ? 2 : 0;
        y -= walkOffset;
        DrawData drawData;
        if (!drawInfo.headOnlyRender)
        {
            float offsetX, offsetY;
            for (int i = 0; i < 6; i++)
            {
                offsetX = Utils.RandomInt(ref seed, -3, 3);
                offsetY = Utils.RandomInt(ref seed, -3, 3);
                drawData = new DrawData(
                    flameTex,
                    new Vector2(x + offsetX, y + offsetY),
                    new Rectangle?(bodyFrame),
                    new Color(255, 255, 255, 0) * 0.4f,
                    player.bodyRotation,
                    origin,
                    1f,
                    drawInfo.playerEffect,
                    0
                );
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
        drawData = new DrawData(
            flameTex, new Vector2(x, y) + new Vector2(drawInfo.headOnlyRender ? 1f * player.direction : 0f, 0f),
            new Rectangle?(bodyFrame),
            new Color(255, 255, 255, 200),
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect,
            0
        );
        drawInfo.DrawDataCache.Add(drawData);
    }
}