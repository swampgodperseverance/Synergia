using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Common.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Body)]
public sealed class FadingHellChestplate : ModItem
{
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        for (int i = 0; i < 3; i++)
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_Body{FadingHellArmorVisual.TextureSuffix[i]}", EquipType.Body, this, $"FadingHellChestplate_{FadingHellArmorVisual.TextureSuffix[i]}");
    }
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 16;

        Item.rare = ModContent.RarityType<CoreburnedRarity>();
        Item.defense = 13;
        Item.value = Item.sellPrice(0, 5, 50, 0);
    }
    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Magic) += 0.08f;
        player.GetCritChance(DamageClass.Magic) += 10f;
    }
}
public class FadingHellChestplate_Cloak : PlayerDrawLayer
{
    private const int FramesAmount = 20;
    private const int TypesAmount = 4;
    private Asset<Texture2D> cloakTexture;
    public override void Load()
    {
        cloakTexture = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellChestplate_Cloak");
    }
    public override void Unload()
    {
        cloakTexture = null;
    }
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BackAcc);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.armor[1].type == ModContent.ItemType<FadingHellChestplate>() && drawInfo.drawPlayer.armor[11].type == ItemID.None
            || drawInfo.drawPlayer.armor[11].type == ModContent.ItemType<FadingHellChestplate>();
    }
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.hideEntirePlayer)
            return;

        Player player = drawInfo.drawPlayer;
        FadingHellPlayer fadingHellPlayer = player.GetModPlayer<FadingHellPlayer>();
        if (drawInfo.shadow != 0f || player.dead)
            return;

        Texture2D cloakTex = cloakTexture.Value;
        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3);
        Rectangle bodyFrame = player.bodyFrame;
        bodyFrame.X = Math.Clamp((int)fadingHellPlayer.currentFireType - 1, 0, TypesAmount) * bodyFrame.Width;
        float height = cloakTex.Height / FramesAmount;
        Vector2 origin = new Vector2(cloakTex.Width / TypesAmount, height) / 2f;
        DrawData drawData = new DrawData(
            cloakTex,
            new Vector2(x, y),
            new Rectangle?(bodyFrame),
            drawInfo.colorArmorBody,
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect,
            0
        );
        int shader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[1].type);
        drawData.shader = shader;
        drawInfo.DrawDataCache.Add(drawData);
    }
}