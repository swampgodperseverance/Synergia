using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
	public class TarHelper : GlobalProjectile
	{
		  public override bool InstancePerEntity => true;
		public bool Cangoback = false;
	}
}
