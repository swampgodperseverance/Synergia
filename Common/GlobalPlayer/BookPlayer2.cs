using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalPlayer
{
	public class BookPlayer2 : ModPlayer
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod vanilla = ModLoader.GetMod("Synergia");
		private static Mod valhalla = ModLoader.GetMod("ValhallaMod");
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		private static Mod roa = ModLoader.GetMod("RoA");
		public bool BookVisible = false;
		public float BookOpacity = 0f; 

		public override void PostUpdate()
		{
			float targetOpacity = BookVisible ? 1f : 0f;
			float opacitySpeed = 0.1f; 

			BookOpacity = MathHelper.Lerp(BookOpacity, targetOpacity, opacitySpeed);
		}

		public void DrawBook(SpriteBatch spriteBatch)
		{
			if (BookOpacity > 0.01f)
			{
				Texture2D texture = ModContent.Request<Texture2D>("Synergia/Assets/UIs/AncientBook2").Value;

				// Центр экрана
				Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
				Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;

				spriteBatch.Draw(
					texture,
					position,
					null,
					Color.White * BookOpacity,
					0f,
					origin,
					1f, // Масштаб 1:1
					SpriteEffects.None,
					0f
				);

				Point mousePos = Main.MouseScreen.ToPoint();

				Rectangle GlobeZone = new Rectangle((int)(position.X - 215), (int)(position.Y - 288), 26, 20);

				if (GlobeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("ColdSummonHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle ColdFatherZone = new Rectangle((int)(position.X - 150), (int)(position.Y - 300), 140, 60);

				if (ColdFatherZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("ColdHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle NautilusSummonZone = new Rectangle((int)(position.X - 213), (int)(position.Y - 210), 28, 25);

				if (NautilusSummonZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("NautilusSummonHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle NautilusZone = new Rectangle((int)(position.X - 150), (int)(position.Y - 223), 140, 55);

				if (NautilusZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("NautilusHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle StarZone = new Rectangle((int)(position.X - 211), (int)(position.Y - 155), 18, 25);

				if (StarZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.FallenStar).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle CobaltZone = new Rectangle((int)(position.X - 185), (int)(position.Y - 155), 25, 20);

				if (CobaltZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.SoulofFlight).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle FlightZone = new Rectangle((int)(position.X - 246), (int)(position.Y - 155), 20, 20);

				if (FlightZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.CobaltBar).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle AnzilZone1 = new Rectangle((int)(position.X - 217), (int)(position.Y - 125), 26, 20);

				if (AnzilZone1.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("NaquadahAnvil").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle SealZone = new Rectangle((int)(position.X - 214), (int)(position.Y - 94), 22, 22);

				if (SealZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(valhalla.Find<ModItem>("HeavensSeal").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle EmperorZone = new Rectangle((int)(position.X - 142), (int)(position.Y - 145), 135, 70);

				if (EmperorZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("EmperorHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle GelatineCrystalZone = new Rectangle((int)(position.X - 225), (int)(position.Y - 40), 26, 26);

				if (GelatineCrystalZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.QueenSlimeCrystal).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle QueenZone = new Rectangle((int)(position.X - 135), (int)(position.Y - 47), 125, 60);

				if (QueenZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("QueenHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle SkullZone = new Rectangle((int)(position.X - 208), (int)(position.Y - -40), 20, 25);

				if (SkullZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.MechanicalSkull).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle EyeZone = new Rectangle((int)(position.X - 250), (int)(position.Y - -40), 25, 20);

				if (EyeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.MechanicalEye).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle WormZone = new Rectangle((int)(position.X - 174), (int)(position.Y - -40), 25, 18);

				if (WormZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.MechanicalWorm).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle BeeBossZone = new Rectangle((int)(position.X - 135), (int)(position.Y - -24), 120, 50);

				if (BeeBossZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("TrioHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle WorkShopZone = new Rectangle((int)(position.X - 213), (int)(position.Y - -120), 20, 25);

				if (WorkShopZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.TinkerersWorkshop).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle ChlorophyteZone = new Rectangle((int)(position.X - 213), (int)(position.Y - -93), 25, 20);

				if (ChlorophyteZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.ChlorophyteBar).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle MasterBaitZone = new Rectangle((int)(position.X - 237), (int)(position.Y - -93), 22, 22);

				if (MasterBaitZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.MasterBait).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle BaitZone = new Rectangle((int)(position.X - 213), (int)(position.Y - -147), 25, 25);

				if (BaitZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(valhalla.Find<ModItem>("ShinyBait").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle CarnageZone = new Rectangle((int)(position.X - 135), (int)(position.Y - -98), 120, 50);

				if (CarnageZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("CarnageStory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle PiratesZone = new Rectangle((int)(position.X - 306), (int)(position.Y - -205), 275, 80);

				if (PiratesZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("PiratesHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				Rectangle MoonsZone = new Rectangle((int)(position.X - -68), (int)(position.Y - -185), 300, 100);

				if (MoonsZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("MoonsHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle PlanteraZone = new Rectangle((int)(position.X - -278), (int)(position.Y - 300), 130, 30);

				if (PlanteraZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("PlanteraHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle BlazewormZone = new Rectangle((int)(position.X - -268), (int)(position.Y - 225), 140, 60);

				if (BlazewormZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("BlazewormHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
		    	Rectangle GolemZone = new Rectangle((int)(position.X - -268), (int)(position.Y - 137), 140, 56);

				if (GolemZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("GolemHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle GolemSummonZone = new Rectangle((int)(position.X - -186), (int)(position.Y - 125), 33, 40);

				if (GolemSummonZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("GolemSummonHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;

				}
				Rectangle HeacyAnzilZone = new Rectangle((int)(position.X - -186), (int)(position.Y - 26), 30, 20);

				if (HeacyAnzilZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("CaesiumHeavyAnvil").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;

				}
				Rectangle FragmentZone = new Rectangle((int)(position.X - -206), (int)(position.Y - 49), 20, 20);

				if (FragmentZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.LunarTabletFragment).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle BoneZone = new Rectangle((int)(position.X - -170), (int)(position.Y - 49), 20, 20);

				if (BoneZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Bone).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle SuspSkullZone = new Rectangle((int)(position.X - -194), (int)(position.Y - -3), 20, 20);

				if (SuspSkullZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(consolaria.Find<ModItem>("SuspiciousLookingSkull").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;

				}
				Rectangle PlanteraSummonHistory = new Rectangle((int)(position.X - -188), (int)(position.Y - 295), 55, 35);

				if (PlanteraSummonHistory.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("PlanteraSummonHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;

				}
				Rectangle SinstoneZone = new Rectangle((int)(position.X - -180), (int)(position.Y - 228), 20, 15);

				if (SinstoneZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(valhalla.Find<ModItem>("Sinstone").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle BottledZone = new Rectangle((int)(position.X - -205), (int)(position.Y - 227), 20, 20);

				if (BottledZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("BottledLava").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle HellforgeZone = new Rectangle((int)(position.X - -200), (int)(position.Y - 203), 26, 20);

				if (HellforgeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Hellforge).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle ScaleZone = new Rectangle((int)(position.X - -200), (int)(position.Y - 173), 20, 20);

				if (ScaleZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("HellwormScale").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle LifeZone = new Rectangle((int)(position.X - -230), (int)(position.Y - 227), 20, 20);

				if (LifeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("LifeDew").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle OcramZone = new Rectangle((int)(position.X - -280), (int)(position.Y - 50), 157, 60);

				if (OcramZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("OcramHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle SolarZone = new Rectangle((int)(position.X - -365), (int)(position.Y - -108), 28, 40);

				if (SolarZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("EclipseHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle AnvilZone2 = new Rectangle((int)(position.X - -238), (int)(position.Y - -123), 15, 10);

				if (AnvilZone2.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("NaquadahAnvil").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle GiftZone = new Rectangle((int)(position.X - -238), (int)(position.Y - -141), 15, 19);

				if (GiftZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.NaughtyPresent).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle Ectoplasm1 = new Rectangle((int)(position.X - -242), (int)(position.Y - -101), 10, 10);

				if (Ectoplasm1.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Ectoplasm).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle SilkZone = new Rectangle((int)(position.X - -222), (int)(position.Y - -101), 10, 10);

				if (SilkZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Silk).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle IceZone = new Rectangle((int)(position.X - -264), (int)(position.Y - -101), 10, 10);

				if (IceZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("SoulofIce").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle AnvilZone3 = new Rectangle((int)(position.X - -110), (int)(position.Y - -122), 15, 10);

				if (AnvilZone3.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("NaquadahAnvil").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle Ectoplasm2 = new Rectangle((int)(position.X - -114), (int)(position.Y - -100), 10, 10);

				if (Ectoplasm2.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Ectoplasm).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle CorrodeZone = new Rectangle((int)(position.X - -132), (int)(position.Y - -105), 13, 10);

				if (CorrodeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(valhalla.Find<ModItem>("CorrodeBar").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle PupmkinZone = new Rectangle((int)(position.X - -92), (int)(position.Y - -102), 12, 12);

				if (PupmkinZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Pumpkin).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
				Rectangle MedalZone = new Rectangle((int)(position.X - -112), (int)(position.Y - -147), 19,22);

				if (MedalZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.PumpkinMoonMedallion).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
			}
		}
	}
}
