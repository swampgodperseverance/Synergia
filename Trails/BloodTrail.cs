using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace Synergia.Trails
{
	public struct BloodTrails
	{
		public void Draw(Projectile proj)
		{
			MiscShaderData shader = GameShaders.Misc["RainbowRod"];

			shader.UseSaturation(1.6f);   
			shader.UseOpacity(3.2f);   
			shader.Apply(null);

			_vertexStrip.PrepareStripWithProceduralPadding(
				proj.oldPos,
				proj.oldRot,
				StripColors,
				StripWidth,
				-Main.screenPosition + proj.Size / 2f,
				false,
				true
			);

			_vertexStrip.DrawTrail();

			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip)
		{

			Color color = Color.Lerp(
				new Color(120, 0, 0),      
				new Color(200, 10, 10),  
				progressOnStrip
			);

			color = Color.Lerp(
				color,
				new Color(40, 0, 0),        
				progressOnStrip * progressOnStrip
			);

			color.A = 0;
			return color;
		}

		private float StripWidth(float progressOnStrip)
		{
			float width = Utils.GetLerpValue(0f, 0.3f, progressOnStrip, true);
			width = (float)Math.Sin(width * MathHelper.Pi);

			return MathHelper.Lerp(1.5f, 6.5f, width);
		}


		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
