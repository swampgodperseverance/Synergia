using Avalon.Items.Placeable.Statue;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using ValhallaMod.Items.Consumable.Bag;

namespace Synergia.Content.Projectiles.Other
{
    public class GreenPresentProj : BasePresentProj {
        public override void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.45f, Pitch = -0.3f }, Projectile.Center);

            for (int i = 0; i < 28; i++){
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
                d.velocity *= 2.8f;
                d.noGravity = true;
                d.scale = 1.5f;
            }
            if (Main.myPlayer == Projectile.owner){
                if (Main.expertMode || Main.masterMode){
                    if (Main.rand.NextFloat() < 0.2f) {
                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), ItemType<GreatGift>());
                    }
                    else {
                        DropTrash();
                    }
                }
                else{
                    if (Main.rand.NextFloat() < 0.2f) {
                        int[] good = [ItemType<IceSculpture>(), ItemID.IceFeather];
                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(good));
                    }
                    else {
                        DropTrash();
                    }
                }
            }
            Projectile.Kill();
        }
        void DropTrash() {
            int[] trash = [ItemID.RottenEgg, ItemID.MudBlock, ItemID.SlushBlock, ItemID.SnowBlock, ItemID.FlinxFur, ItemID.IceBlock];
            Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(trash));
        }
    }
}
