using Avalon.Tiles.Furniture.ResistantWood;
using System.Collections.Generic;
using ValhallaMod.Tiles.Furnitures;
using static Synergia.Content.Tiles.WorldGen.SynergiaEditTiles;
using static Synergia.ModList;
using static Terraria.ID.TileID;

namespace Synergia.Lists {
    public class Tiles {
        public static List<int> EvilTiles { get; private set; } = [ShadowOrbs, TileType<Avalon.Tiles.Contagion.SnotOrb.SnotOrb>()];
        public static List<int> VanillaTile { get; private set; } = [Adamantite, Titanium];
        public static HashSet<int> SnowVillagesMultiTile { get; private set; } = [TileType<SmoothMarblePillarBroken>(), TileType<ResistantWoodTable>(), Valhalla.Find<ModTile>("Millstone").Type, Statues, Lampposts, WaterFountain, TileType<Avalon.Tiles.NickelAnvil>(), TileType<Workbench>(), FishingCrate, TileType<LaminatedTable>(), TileType<Chair>(), TileType<LaminatedBed>(), Lamps, Sawmill, WorkBenches, TileType<ResistantWoodClock>(), Fireplace, Lamps, ClosedDoor, MusicBoxes, Roa.Find<ModTile>("ElderwoodDoorClosed").Type, Furnaces, Banners, LightningBuginaBottle];
        public static HashSet<int> HellArenaMultiTile { get; private set; } = [Ava.Find<ModTile>("ResistantWoodLamp").Type];
        public static HashSet<int> HellVillageMultiTile { get; private set; } = [Ava.Find<ModTile>("ResistantWoodLamp").Type, Ava.Find<ModTile>("ResistantWoodBookcase").Type, Valhalla.Find<ModTile>("DwarvenAnvil").Type, Ava.Find<ModTile>("ResistantWoodLantern").Type, Ava.Find<ModTile>("ResistantWoodCandelabra").Type, ClosedDoor, Bookcases, PottedPlants2, Benches, Tables, Statues, Statues, Bottles, LavafishBowl];
        public static HashSet<int> HellLakeMultiTile { get; private set; } = [Containers, TileType<ResistantWoodCandelabra>()];
    }
}