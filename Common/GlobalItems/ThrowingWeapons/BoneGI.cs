using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class BoneGlobalItem : ThrowingGI
    {
        public override int ItemType => ItemID.Bone;

        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity *= 1.3f;
            damage = (int)(damage * 0.8f);
            knockback *= 1.1f;

            int boneCount = Main.rand.Next(4, 7);

            float baseSpeed = velocity.Length();
            float angleSpread = 24f;
            float speedVariation = 0.28f;

            for (int i = 0; i < boneCount; i++)
            {
                float angleOffset = MathHelper.Lerp(-angleSpread / 2, angleSpread / 2, (float)i / (boneCount - 1));
                angleOffset += Main.rand.NextFloat(-4f, 4f);

                float speedMult = 1f + Main.rand.NextFloat(-speedVariation, speedVariation);

                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(angleOffset));
                newVelocity.Normalize();
                newVelocity *= baseSpeed * speedMult;

                Projectile.NewProjectile(
                    source,
                    position,
                    newVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            return false;
        }
    }
}