using ReLogic.Content;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Common.SUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic.FadingHell;

[AutoloadEquip(EquipType.Legs)]
public sealed class FadingHellPants : ModItem
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
    public RopeVerlet(Vector2 headPosition, int ropeLength, float ropeSegmentSize, float dampingForce)
    {
        _ropeLength = ropeLength;
        _segmentSize = ropeSegmentSize;
        _ropeSegments = new List<RopeSegment>();
        for (int i = 0; i < _ropeLength; i++)
            _ropeSegments.Add(new RopeSegment(headPosition));
    }

    public virtual void Update(Vector2 gravity, Vector2 headPosition)
    {
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
    protected virtual void Simulate(Vector2 gravity, Vector2 headPosition)
    {
        Vector2 velocity;
        RopeSegment segment;
        for (int i = 0; i < _ropeLength; i++) {
            segment = _ropeSegments[i];
            velocity = (segment.Position - segment.OldPosition) * _dampingForce;
            segment.OldPosition = segment.Position;
            segment.Position += velocity;
            segment.Position += gravity;
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
                segment.Position -= direction * 0.5f;
                nextSegment.Position += direction * 0.5f;
            }
            else
                nextSegment.Position += direction;

            _ropeSegments[i] = segment;
            _ropeSegments[i + 1] = nextSegment;
        }
    }
}

public class FlameTailPhysics : RopeVerlet {
    protected Vector2 _tipPosition;
    public FlameTailPhysics(Vector2 headPosition, int ropeLength, float ropeSegmentSize, float dampingForce, Vector2 tipPosition) : base(headPosition, ropeLength, ropeSegmentSize, dampingForce)
    {
        _tipPosition = tipPosition;
    }
    public void SetTipPosition(Vector2 tipPosition) => _tipPosition = tipPosition;
    protected override void Simulate(Vector2 force, Vector2 headPosition)
    {
        Vector2 velocity;
        RopeSegment segment;
        for (int i = 0; i < _ropeLength; i++)
        {
            segment = _ropeSegments[i];
            velocity = (segment.Position - segment.OldPosition) * _dampingForce;
            segment.OldPosition = segment.Position;
            segment.Position += velocity;
            Vector2 tipPosition = headPosition + _tipPosition;
            if (i == _ropeLength - 1)
            {
                segment.Position += force * 0.5f;
                Vector2 tipVelocity = (tipPosition - segment.Position) * new Vector2(0.5f, segment.Position.Y > tipPosition.Y ? 1.25f : -0.2f);
                segment.Position += tipVelocity;
            }
            else
                segment.Position += force;
            _ropeSegments[i] = segment;
        }
    }
}

public class PlayerFlameTailHandler : ModPlayer
{
    FlameTailPhysics _tail;
    readonly Vector2 _headOffset = new Vector2(0, 12);
    readonly Vector2 _tipPosition = new Vector2(-21f, -6f);
    readonly Vector2 _gravity = new Vector2(0, 2f);
    Vector2 tipPosition;
    Vector2 GetHeadPosition() => Player.RotatedRelativePoint(Player.MountedCenter) + _headOffset;
    public override void Initialize()
    {
        _tail = new FlameTailPhysics(Player.Center, 15, 2f, 0.2f, _tipPosition);
    }
    public override void PostUpdate()
    {
        tipPosition = _tipPosition * new Vector2(Player.direction, 1f);
        _tail.SetTipPosition(tipPosition);
        _tail.Update(_gravity, GetHeadPosition());
        //Vector2[] a = tail.GetRopePositions();
        //for(int i = 0; i < a.Length; i++)
        //    Main.NewText($"{i}: {a[i]}");
    }
    public Vector2[] GetRopePositions() => _tail.GetRopePositions();
}

public class FadingHellPants_TailDraw : PlayerDrawLayer
{
    private readonly Color tailColor = new Color(38, 18, 7);

    private const int FramesAmount = 6;
    private Asset<Texture2D> tailFlameAsset;
    private Asset<Texture2D> lineAsset;
    public override void Load()
    {
        tailFlameAsset = ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellPants_Fire");
        lineAsset = TextureAssets.MagicPixel;
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
        Rectangle frame = new Rectangle(0, 0, 1, 1);
        Vector2 origin = frame.Size() / 2f;
        DrawData drawData;
        Vector2 pos;
        float distance, rotation;
        for(int i = 0; i < amount - 1; i++)
        {
            distance = Vector2.Distance(poses[i], poses[i + 1]) + 0.5f;
            rotation = (poses[i + 1] - poses[i]).ToRotation();
            pos = poses[i] - Main.screenPosition;
            drawData = new DrawData(
                lineTexture,
                pos,
                new Rectangle?(frame),
                tailColor.MultiplyRGB(drawInfo.colorArmorLegs),
                rotation,
                origin,
                new Vector2(distance, 2f),
                SpriteEffects.None,
                0
            );
            drawInfo.DrawDataCache.Add(drawData);
        }

        Texture2D flameTex = tailFlameAsset.Value;
        ulong seed = (ulong)(player.miscCounter / 5);
        int frameCount = (int)seed % 6;
        int frameHeight = flameTex.Height / FramesAmount;
        frame = new Rectangle(0, frameHeight * frameCount, flameTex.Width, frameHeight);
        origin = frame.Size() / 2f - new Vector2(-3, -15);
        pos = poses[^1] - Main.screenPosition;
        rotation = (poses[^1] - poses[^2]).ToRotation() + MathHelper.PiOver2;
        drawData = new DrawData(
            flameTex,
            pos,
            new Rectangle?(frame),
            Color.White,
            rotation,
            origin,
            1f,
            SpriteEffects.None,
            0
        );
        drawInfo.DrawDataCache.Add(drawData);
    }
}