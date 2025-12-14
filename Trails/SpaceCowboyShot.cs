using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace Synergia.Trails
{
	// Token: 0x020005F7 RID: 1527
	public struct SpaceCowboyShot
	{
		// Token: 0x06001815 RID: 6165 RVA: 0x000A7D5C File Offset: 0x000A5F5C
		public void Draw(Projectile proj)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
			miscShaderData.UseSaturation(0f);
			miscShaderData.UseOpacity(1f);
			miscShaderData.Apply(null);
			SpaceCowboyShot._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false, true);
			SpaceCowboyShot._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip)
		{
			// Яркий бирюзово-голубой
			Color result = new Color(0, 220, 255);
			result.A = 0;
			return result;
		}


		// Token: 0x06001817 RID: 6167 RVA: 0x000A7E44 File Offset: 0x000A6044
		private float StripWidth(float progressOnStrip)
		{
			float num = 1.7f;
			float lerpValue = Utils.GetLerpValue(0f, 1.7f, progressOnStrip, true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 33f, num);
		}

		// Token: 0x0400034C RID: 844
		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
