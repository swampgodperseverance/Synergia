using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Buffs
{
    public class Bloodbound : ModBuff
    {
        public override void SetStaticDefaults()
        {

            

            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;    
        }

        public override void Update(Player player, ref int buffIndex)
        {
       
            player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
      
            player.moveSpeed += 0.2f;

            player.endurance -= 0.10f;

            if (Main.rand.NextBool(6)) 
            {
                Dust dust = Dust.NewDustDirect(
                    player.position,
                    player.width,
                    player.height,
                    DustID.Blood,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(0.5f, 2f),
                    100,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );

                dust.noGravity = false;
                dust.velocity *= 0.5f;
            }
        }
    }
}
