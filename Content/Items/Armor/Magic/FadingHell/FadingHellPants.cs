using ReLogic.Content;
using Synergia.Common.GlobalItems;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Common.SUtils;
using Synergia.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Legs)]
public sealed class FadingHellPants : ModItem
{
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        for (int i = 0; i < 3; i++)
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_Legs{FadingHellArmorVisual.TextureSuffix[i]}", EquipType.Legs, this, $"FadingHellPants_{FadingHellArmorVisual.TextureSuffix[i]}");
    }
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 16;
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 9;
        Item.value = Item.sellPrice(0, 4, 0, 0);
    }
    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Magic) += 0.06f;
        player.moveSpeed += 0.12f;
    }
}
public class FadingHellPantsSpeedboost : ModPlayer
{

}

public class RopeVerlet
{
    public struct RopeSegment
    {
        public Vector2 Position;
        public Vector2 OldPosition;
        public RopeSegment(Vector2 position)
        {
            Position = position;
            OldPosition = position;
        }
    }
    protected List<RopeSegment> _ropeSegments;
    protected int _ropeLength;
    protected float _segmentSize;
    protected float _dampingForce;
    protected const int _constrainsIterations = 50;
    public RopeVerlet(Vector2 headPosition, int spawnDirection, int ropeLength, float ropeSegmentSize, float dampingForce)
    {
        _ropeLength = ropeLength;
        _segmentSize = ropeSegmentSize;
        _ropeSegments = new List<RopeSegment>();
        for (int i = 0; i < _ropeLength; i++)
            _ropeSegments.Add(new RopeSegment(headPosition - new Vector2(i * _segmentSize, 0f) * spawnDirection));
    }

    public virtual void Update(Vector2 gravity, Vector2 headPosition)
    {
        ValidatePoses(headPosition);
        Simulate(gravity, headPosition);
        for(int i = 0; i < _constrainsIterations; i++)
            Constrains(headPosition);
    }
    public Vector2[] GetRopePositions()
    {
        Vector2[] positions = new Vector2[_ropeLength];
        for (int i = 0; i < _ropeLength; i++)
            positions[i] = _ropeSegments[i].Position;
        return positions;
    }
    protected virtual void ValidatePoses(Vector2 headPosition)
    {
        for(int i = 0; i < _ropeLength; i++)
        {
            if (float.IsNaN(_ropeSegments[i].Position.X) || float.IsNaN(_ropeSegments[i].Position.Y))
            {
                RopeSegment segment = _ropeSegments[i];
                segment.Position = headPosition - new Vector2(i * _segmentSize, 0f);
                segment.OldPosition = segment.Position;
                _ropeSegments[i] = segment;
            }
        }
    }
    protected virtual void Simulate(Vector2 gravity, Vector2 headPosition)
    {
        Vector2 velocity;
        RopeSegment segment;
        for (int i = 0; i < _ropeLength; i++) {
            segment = _ropeSegments[i];
            velocity = (segment.Position - segment.OldPosition) * _dampingForce;
            velocity += gravity;
            velocity = SolveCollision(segment, velocity, headPosition);
            segment.OldPosition = segment.Position;
            segment.Position += velocity;
            _ropeSegments[i] = segment;
        }
    }
    protected virtual void Constrains(Vector2 headPosition)
    {
        RopeSegment segment, nextSegment;
        segment = _ropeSegments[0];
        segment.Position = headPosition;
        _ropeSegments[0] = segment;

        for (int i = 0; i < _ropeLength - 1; i++)
        {
            segment = _ropeSegments[i];
            nextSegment = _ropeSegments[i + 1];

            Vector2 direction = segment.Position - nextSegment.Position;
            float distance = direction.Length();
            float difference = distance - _segmentSize;
            direction.Normalize();
            direction *= difference;

            if (i != 0)
            {
                Vector2 segmentDirection = SolveCollision(segment, -direction * 0.5f, headPosition);
                segment.Position += segmentDirection;
                segmentDirection = SolveCollision(nextSegment, direction * 0.5f, headPosition);
                nextSegment.Position += segmentDirection;
            }
            else
            {
                Vector2 segmentDirection = SolveCollision(nextSegment, direction, headPosition);
                nextSegment.Position += segmentDirection;
            }

            _ropeSegments[i] = segment;
            _ropeSegments[i + 1] = nextSegment;
        }
    }
    protected virtual Vector2 SolveCollision(RopeSegment segment, Vector2 velocity, Vector2 headPosition)
    {
        if (Vector2.Distance(headPosition, segment.Position) < _ropeLength * 2 * _segmentSize)
            return Collision.TileCollision(segment.Position, velocity, (int)_segmentSize, (int)_segmentSize, true);
        return velocity;
    }
}

public class PlayerFlameTailHandler : ModPlayer
{
    internal const int TailLength = 18;
    internal const float TailSegmentSize = 2f;
    internal const float TailDamping = 0.6f;
    RopeVerlet _tail;
    readonly Vector2 _headOffset = new Vector2(0, 12);
    readonly Vector2 _gravity = new Vector2(0, 2f);
    Vector2 GetHeadPosition() => Player.RotatedRelativePoint(Player.MountedCenter) + _headOffset;
    public override void ResetEffects()
    {
        if (IsWearingPants() && _tail == null)
            _tail = new RopeVerlet(GetHeadPosition(), Player.direction, TailLength, TailSegmentSize, TailDamping);
        else if (!IsWearingPants() && _tail != null)
            _tail = null;
    }
    public override void PostUpdateMiscEffects()
    {
        _tail?.Update(_gravity, GetHeadPosition());
    }
    public Vector2[] GetRopePositions()
    {
        if(_tail != null)
            return _tail.GetRopePositions();

        Vector2[] poses = new Vector2[TailLength];
        for(int i = 0; i < TailLength; i++)
            poses[i] = GetHeadPosition() - new Vector2(TailSegmentSize * i, 0);
        return poses;
    }
    private bool IsWearingPants() => Player.armor[2].type == ItemType<FadingHellPants>() && Player.armor[12].type == ItemID.None
            || Player.armor[12].type == ItemType<FadingHellPants>();
}

public class FadingHellPants_TailDraw : PlayerDrawLayer
{
    private const int FramesAmount = 6;
    private const int TypesAmount = 4;
    private Asset<Texture2D> tailFlameAsset;
    private Asset<Texture2D> lineAsset;
    public override void Load()
    {
        tailFlameAsset = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellPants_Fire");
        lineAsset = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellPants_Palette");
    }
    public override void Unload()
    {
        tailFlameAsset = null;
        lineAsset = null;
    }
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.armor[2].type == ModContent.ItemType<FadingHellPants>() && drawInfo.drawPlayer.armor[12].type == ItemID.None
            || drawInfo.drawPlayer.armor[12].type == ModContent.ItemType<FadingHellPants>();
    }
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.hideEntirePlayer)
            return;

        Player player = drawInfo.drawPlayer;
        if (drawInfo.shadow != 0f || player.dead)
            return;

        Texture2D lineTexture = lineAsset.Value;
        Vector2[] poses = player.GetModPlayer<PlayerFlameTailHandler>().GetRopePositions();
        int amount = poses.Length;
        Rectangle frame = new Rectangle(Math.Clamp((int)player.GetModPlayer<FadingHellPlayer>().currentFireType - 1, 0, TypesAmount) * 2, 0, 2, 2);
        Vector2 origin = frame.Size() / 2f;
        DrawData drawData;
        Vector2 pos;
        float distance, rotation;
        int shader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[2].type);
        for (int i = 0; i < amount - 1; i++)
        {
            distance = Vector2.Distance(poses[i], poses[i + 1]) + 0.1f;
            rotation = (poses[i + 1] - poses[i]).ToRotation();
            pos = poses[i] - Main.screenPosition;
            drawData = new DrawData(
                lineTexture,
                pos,
                new Rectangle?(frame),
                drawInfo.colorArmorLegs,
                rotation,
                origin,
                new Vector2(distance, 1f),
                SpriteEffects.None,
                0
            );
            drawData.shader = shader;
            drawInfo.DrawDataCache.Add(drawData);
        }

        if (!player.GetModPlayer<FadingHellPlayer>().isOnFire) return;

        Texture2D flameTex = tailFlameAsset.Value;
        ulong seed = (ulong)(player.miscCounter / 5);
        int frameCount = (int)seed % 6;
        int frameWidth = flameTex.Width / TypesAmount;
        int frameHeight = flameTex.Height / FramesAmount;
        frame = new Rectangle(
            frameWidth * ((int)player.GetModPlayer<FadingHellPlayer>().currentFireType - 1),
            frameHeight * frameCount,
            frameWidth,
            frameHeight
        );
        origin = frame.Size() / 2f - new Vector2(-3, -15);
        pos = poses[^1] - Main.screenPosition;
        rotation = (poses[^1] - poses[^2]).ToRotation() + MathHelper.PiOver2;
        float offsetX, offsetY;
        for (int i = 0; i < 6; i++)
        {
            offsetX = Utils.RandomInt(ref seed, -3, 3);
            offsetY = Utils.RandomInt(ref seed, -3, 3);
            drawData = new DrawData(
                flameTex,
                pos + new Vector2(offsetX, offsetY),
                new Rectangle?(frame),
                new Color(255, 255, 255, 0) * 0.4f,
                0f,
                origin,
                1f,
                SpriteEffects.None,
                0
            );
            drawInfo.DrawDataCache.Add(drawData);
        }
        drawData = new DrawData(
            flameTex,
            pos,
            new Rectangle?(frame),
            new Color(255, 255, 255, 200),
            0f,
            origin,
            1f,
            SpriteEffects.None,
            0
        );
        drawInfo.DrawDataCache.Add(drawData);
    }
}