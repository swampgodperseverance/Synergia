using rail;
using ReLogic.Content;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Common.SUtils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Head)]
public sealed class FadingHellHat : ModItem {
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
        if (Main.netMode == NetmodeID.Server)
            return;

        int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
    }
    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 20;
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 25;
        Item.value = Item.sellPrice(0, 3, 4, 50);
    }
    public override void UpdateEquip(Player player) {
        //player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
        //player.GetCritChance(DamageClass.Melee) += 17f;
    }
    public override bool IsArmorSet(Item head, Item body, Item legs) =>
        head.type == Type &&
        body.type == ModContent.ItemType<FadingHellChestplate>() &&
        legs.type == ModContent.ItemType<FadingHellPants>();
    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = LocUtil.ItemTooltip(LocUtil.ARM, "FadingHellSetBonus");
        player.GetModPlayer<FadingHellPlayer>().isSetBonus = true;
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

        if (player.GetModPlayer<FadingHellPlayer>().isOnFire || drawInfo.drawPlayer.armor[10].type == ModContent.ItemType<FadingHellHat>() || Main.gameMenu)
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
        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class FadingHellHat_FlameDraw : PlayerDrawLayer
{
    private readonly Vector2 FlameOffset = new(-9, -44);
    private const int FramesAmount = 20;
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

        if (!(player.GetModPlayer<FadingHellPlayer>().isOnFire || drawInfo.drawPlayer.armor[10].type == ModContent.ItemType<FadingHellHat>()) && !Main.gameMenu)
            return;

        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X + FlameOffset.X * player.direction);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3 + FlameOffset.Y);
        ulong seed = (ulong)(player.miscCounter / 5);
        int frameCount = (int)seed % 10;
        Texture2D flameTex = flameTexture.Value;
        int frameHeight = flameTex.Height / FramesAmount;
        Rectangle bodyFrame = new Rectangle(0, frameHeight * frameCount, flameTex.Width, frameHeight);
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