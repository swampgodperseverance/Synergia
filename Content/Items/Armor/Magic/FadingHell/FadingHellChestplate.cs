using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Body)]
public sealed class FadingHellChestplate : ModItem
{
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 16;
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 25;
        Item.value = Item.sellPrice(0, 3, 4, 50);
    }
    public override void UpdateEquip(Player player)
    {
        //player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
        //player.GetCritChance(DamageClass.Melee) += 17f;
    }
}
public class FadingHellChestplate_Cloak : PlayerDrawLayer
{
    private const int FramesAmount = 20;
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
        if (drawInfo.shadow != 0f || player.dead)
            return;

        Texture2D cloakTex = cloakTexture.Value;
        int x = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X);
        int y = (int)(drawInfo.Position.Y + player.height / 2f - Main.screenPosition.Y - 3);
        Rectangle bodyFrame = player.bodyFrame;
        float height = cloakTex.Height / FramesAmount;
        Vector2 origin = new Vector2(cloakTex.Width, height) / 2f;
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
        drawInfo.DrawDataCache.Add(drawData);
    }
}