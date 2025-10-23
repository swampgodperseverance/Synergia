using Avalon.Tiles.Furniture;
using Avalon.Tiles.Furniture.PurpleDungeon;
using Avalon.Tiles.Furniture.ResistantWood;
using Avalon.Tiles.Ores;
using Avalon.Walls;
using Consolaria.Content.Tiles;
using Microsoft.Xna.Framework;
using StramsSurvival.Tiles.Furniture;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using ValhallaMod.NPCs.TownNPCs;
using ValhallaMod.Tiles.Blocks;
using ValhallaMod.Tiles.Furnitures;
using static AssGen.Assets;
using static Synergia.Content.Tiles.WorldGen.SynergiaEditTiles;
using Chest = ValhallaMod.Tiles.Furnitures.Chest;

namespace Synergia.Common.WorldGenSystem
{
    public class SynergiaGenerationWorld : ModSystem
    {
        //x = 103, y = 25
        static readonly int[] SnowVilageGenTiles = [147, 161, 163, 200];
        int SnowVilagePositionX = 0;
        int SnowVilagePositionY = 0;
        bool GenerateSnowVilage = false;

        const byte a = 10, b = 11, c = 12, d = 13, e = 14, f = 15, g = 16, h = 17, i = 18, j = 19, k = 20, l = 21;
       
        static readonly Mod RoA = ModLoader.GetMod("RoA");
        static readonly Mod ValhallaMod = ModLoader.GetMod("ValhallaMod");

        #region Info
        // В RoA все классы select. из-за этого через Mod.Find<T>(String class name);
        // В Valhala гавно код поетому используем из Avalon
        // Вместо LaminatedTable и LaminatedBed из Valhala мой блоки от Avalon
        // с левого угла идет + X с верху масива - Y
        // не работает как нужно: FermentingBarrel, Oven
        // не может быть сгенерировано на
        // 001200 где 0 пустота 1 конец 1 стола 2 начала 2 стола
        // ↓ код с верху в низ по этому начало блок потом на блок
        // После 9 идут конст байт так удобней смотреть на масив чем если было бы двузначное число
        #endregion

        static readonly byte[,] SnowVilageTiles =
        {
            // 0 - empty / 1 - snow block / 2 - ice block / 3 - Stone Slab / 4 - Stone / 5 - GrayBrick / 6 - Corrode Brick / 7 - Valhallite Brick / 8 - everwood Beam / 9 - Living Wood / 10:a - Boreal Wood Platform / 11:b - Everwood Platform / 12:c - Purple Brick Platform / 13:d - Wood / 14:e - EverWood / 15: f - Tin Roof / 16:g - Chain / 17:h - Silver Brick / 18:i - Diamond / 19:j - Blue Brick Platform / 20:k - Amber / 21:l - Zircon
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }, // 1
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2 }, // 2
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2 }, // 3
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2 }, // 4
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }, // 5
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0 }, // 6
            {0,1,1,1,1,1,1,1,1,1,1,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,3,3,1,1,1,1,3,4,4,3,3,3,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0 }, // 7
            {0,1,1,1,3,1,1,1,1,1,5,5,1,1,1,1,1,1,3,5,3,1,1,1,1,1,5,5,3,3,1,1,1,1,1,1,1,1,1,1,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,3,3,4,3,3,4,4,5,4,3,3,3,3,5,5,3,4,3,1,1,1,1,0,0,0,0,0,0 }, // 8
            {0,0,1,1,4,4,1,1,3,5,3,5,3,3,1,1,3,3,4,4,3,1,1,5,5,4,4,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,7,7,7,3,6,3,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,4,5,5,3,3,4,5,3,4,3,5,5,5,4,4,3,5,5,1,0,0,0,0,0,0,0,0 }, // 9
            {0,0,0,1,3,5,5,3,3,3,3,4,4,5,5,5,3,4,5,5,5,3,3,5,4,4,5,5,4,3,1,0,1,0,0,0,1,2,2,3,7,6,6,3,3,7,7,3,3,3,7,2,2,2,2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 10
            {0,0,0,0,0,0,3,4,5,5,5,3,3,4,3,5,5,4,4,3,5,5,3,5,3,3,5,3,1,0,0,0,0,0,0,0,0,6,6,7,3,3,3,7,7,6,6,6,3,7,6,3,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 11
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 12
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,9,a,a,a,0,0,0,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0 }, // 13
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0,0,a,c,b,b,0,9,d,0,0,0,0,0,0,0,0,0 }, // 14
            {0,0,0,0,0,0,e,a,0,0,0,0,0,0,0,0,0,0,0,a,a,a,3,3,0,0,3,3,0,0,0,0,0,0,0,0,0,0,0,0,g,0,a,a,8,0,0,8,0,0,0,0,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,d,e,0,0,a,a,a,0,0,0,0,0,0,0,0,0,0,e,0,0,0,0,0,0,0,0,0 }, // 15
            {0,0,0,0,0,d,d,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,e,0,0,g,0,0,0,8,0,0,8,0,g,0,9,d,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,0,e,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,d,d,0,0,0,0,0,0,0,0 }, // 16
            {0,0,0,0,0,e,9,d,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,f,f,0,0,0,0,0,0,0,0,0,0,d,d,9,g,9,d,9,8,d,9,8,9,9,9,9,d,e,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,9,9,d,e,d,9,0,0,0,0,0,0,0,0,0,e,e,d,e,0,0,0,0,0,0,0 }, // 17
            {0,0,0,0,d,e,9,e,0,j,j,j,j,0,0,0,0,3,3,3,3,f,f,f,f,0,0,0,0,0,0,0,0,0,0,0,4,9,9,d,d,d,d,9,9,d,d,9,e,e,d,d,d,d,d,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,f,e,d,9,9,d,d,9,e,0,0,9,9,d,d,0,f,0,0,0,0,0,0,0 }, // 18
            {0,0,0,f,f,d,9,d,e,0,0,0,0,0,e,e,d,3,f,f,f,f,f,0,0,0,0,0,0,0,0,0,0,0,0,3,4,9,4,i,e,d,4,d,1,1,h,3,4,3,e,4,4,e,4,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,f,0,0,0,9,9,d,d,e,d,d,0,0,f,0,0,0,0,0,0,0 }, // 19
            {0,f,f,f,f,f,e,9,e,0,0,0,e,9,d,f,f,f,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,h,5,4,9,4,4,4,e,e,1,1,5,5,k,4,4,i,5,h,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,f,f,f,f,0,0,0,0,0,0,f,f,f,0,0,0,0,0,0 }, // 20
            {0,0,f,f,f,0,d,9,e,e,d,d,e,0,f,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,4,5,h,l,4,4,1,1,1,1,1,h,5,5,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,f,f,f,f,f,f,0,0,0,0,0,0,0,0 }, // 21
            {0,0,0,f,f,f,f,d,9,9,d,d,f,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,4,5,5,1,1,0,0,1,1,1,4,1,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 22
            {0,0,0,0,0,f,f,f,e,e,0,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,5,1,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 23
            {0,0,0,0,0,0,f,f,f,f,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 24
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 25
        };
        static readonly byte[,] SnowVilageSlopes =
        {
            // 0 - empty / 1 - hamer / 2 - /| / 3 - |/ / 4 - \| / 5 - |\
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 1
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 2
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 3
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 4
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 5
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 6
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 7
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 8
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 9
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 10
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 11
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 12
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0 }, // 13
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0 }, // 14
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 15
            {0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0 }, // 16
            {0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,4,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 17
            {0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0 }, // 18
            {0,0,0,0,0,0,0,0,3,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 19
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 20
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 21
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,5,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 22
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 23
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 24
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 25
        };
        static readonly byte[,] SnowVilageWalls = 
        {
            // 0 - empty / 1 - BackWoods Root Wall / 2 - Stone Slab Wall / 3 - Palm Wood Fence + Brown / 4 - Shadewood Fence + Brown / 5 - Gray brick Wall / 6 - Living wood wall / 7 - Rich Mahogany Fence + Brown / 8 - Wood Wall / 9 - Planked Wall / 10:a - Glass Wall / 11:b - ice Brick Wall / 12:c - Resistant Wood Fence / 13:d - Everwood Wall / 14:e - Cloud Wail / 15:f - Tin Brick Wall
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 1
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 2
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 3
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 4
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 5
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 6
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0 }, // 7
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0 }, // 8
            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0 }, // 9
            {0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,2,2,2,2,5,5,2,5,2,2,3,4,4,3,2,0,1,1,0,0,0,0,0,0,0,0 }, // 10
            {0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,0,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,2,2,2,2,2,5,2,4,3,4,7,5,0,0,0,0,0,0,0,0,0,0,0 }, // 11
            {0,0,0,0,0,0,0,5,5,5,2,2,2,2,5,5,2,5,2,2,2,2,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,c,b,b,5,5,5,d,b,6,2,2,b,b,b,5,5,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,8,a,a,a,a,8,9,8,4,3,7,7,6,0,0,0,0,0,0,0,0,0,0,0 }, // 12
            {0,0,0,0,0,0,0,2,2,2,5,5,5,2,2,2,5,2,2,5,5,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,c,5,b,2,b,b,2,5,5,b,b,2,2,2,b,b,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,9,a,a,a,a,6,8,9,7,3,4,3,8,0,0,0,0,0,0,0,0,0,0,0 }, // 13
            {0,0,0,0,0,0,0,8,d,8,a,a,6,8,6,8,d,d,8,6,9,9,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,c,0,a,a,a,8,b,2,9,2,2,9,a,a,d,d,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,8,a,a,a,a,6,9,9,7,7,4,3,8,8,0,0,0,0,0,0,0,0,0,0 }, // 14
            {0,0,0,0,0,0,0,9,9,8,a,a,6,8,6,8,d,d,8,8,8,8,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,c,0,e,e,a,8,b,d,9,2,2,9,a,a,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,6,8,8,9,9,8,8,6,6,3,7,3,7,6,6,0,0,0,0,0,0,0,0,0,0 }, // 15
            {0,0,0,0,0,0,0,8,9,8,a,a,8,8,6,6,8,8,9,9,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,c,e,e,e,8,6,d,d,9,2,2,9,6,8,8,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,8,8,8,6,6,8,8,8,8,8,3,4,4,3,8,9,0,0,0,0,0,0,0,0,0,0 }, // 16
            {0,0,0,0,0,0,0,8,6,8,a,a,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,c,e,e,e,e,e,d,9,9,9,9,9,d,d,d,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,9,9,8,4,7,3,3,8,0,0,0,0,0,0,0,0,0,0,0 }, // 17
            {0,0,0,0,0,0,0,d,d,8,8,8,8,6,6,d,6,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,7,3,4,f,f,f,f,0,0,0,0,0,0,0,0 }, // 18
            {0,0,0,0,0,0,0,0,0,8,8,8,d,d,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,0,0,0,f,f,f,0,f,f,0,0,0,0,0,0,0,0 }, // 19
            {0,0,0,0,f,f,0,0,0,8,8,8,8,d,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,f,f,f,f,f,f,0,0,0,0,0,0,0,0,0 }, // 20
            {0,0,0,0,f,f,0,0,0,0,0,0,f,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,e,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 21
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,e,e,e,0,0,0,0,e,e,e,e,e,e,0,e,e,e,0,0,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 22
            {0,0,0,0,0,0,0,0,0,0,f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,e,0,e,0,0,0,0,0,e,e,e,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 23
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,e,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 24
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, // 25
        };

        public override void OnWorldLoad() => GenerateSnowVilage = false;
        public override void SaveWorldData(TagCompound tag) { var Generated = new BitsByte(); Generated[0] = GenerateSnowVilage; }
        public override void LoadWorldData(TagCompound tag) { var Generated = (BitsByte)tag.GetByte("Generated"); GenerateSnowVilage = Generated[0]; }
        public override void NetSend(BinaryWriter writer) { BitsByte Flags = new(); Flags[0] = GenerateSnowVilage; }
        public override void NetReceive(BinaryReader reader) { BitsByte Flags = reader.ReadByte(); GenerateSnowVilage = Flags[0]; }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int index = tasks.FindIndex(x => x.Name == "Final Cleanup");
            if (index != -1)
            {
                tasks.Add(new PassLegacy("[Synergia] Snow Vilage", (progress, config) => AddSnowVilage(progress)));
            }
        }
        void AddSnowVilage(GenerationProgress progress = null)
        {
            if (GenerateSnowVilage) return;
            bool Success = Do_MakeSnowVilage(progress);
            if (Success) GenerateSnowVilage = true;
        }
        bool Do_MakeSnowVilage(GenerationProgress progress)
        {
            if (progress != null)
            {
                progress.Message = Language.GetTextValue("Mods.Synergia.WorldGenString.Vilage");
                progress.Set(0.33f);
            }

            List<Point> list = [];

            foreach (int k in SnowVilageGenTiles)
            {
                for (int i = Main.maxTilesX / 5; i < Main.maxTilesX / 5 * 4; i++)
                {
                    int y = 200;
                    while (!WorldGen.SolidOrSlopedTile(i, y + 1)) y++;
                    bool canBeGenerated = true;
                    for (int j = 0; j < 33; j++) { if (!WorldGen.SolidOrSlopedTile(i + j, y + 1) || Main.tile[i + j, y + 1].TileType != k) { canBeGenerated = false; } }
                    if (canBeGenerated) { for (int a = 0; a < 17; a++) { if (WorldGen.SolidOrSlopedTile(i - 1, y - a) || WorldGen.SolidOrSlopedTile(i + 33, y - a)) { canBeGenerated = false; } } }
                    if (canBeGenerated) list.Add(new Point(i, y));
                }
                if (list.Count > 0)
                {
                    Point point = list[WorldGen.genRand.Next(0, list.Count)];
                    SnowVilagePositionX = point.X; SnowVilagePositionY = point.Y;
                    goto GenerateBuild;
                }
            }

            return false;

        GenerateBuild:
            NPC.NewNPC(new EntitySource_WorldGen(), (SnowVilagePositionX + 13) * 16, (SnowVilagePositionY - 11) * 16, ModContent.NPCType<Dwarf>(), 0, 0f, 0f, 0f, 0f, 255);

            int width = SnowVilageTiles.GetLength(1);
            int height = SnowVilageTiles.GetLength(0);

            WorldHelper.Cleaning(SnowVilagePositionX + 3,  SnowVilagePositionY - 11,  SnowVilagePositionX + 100, SnowVilagePositionY - 0,  TileID.SnowBlock, TileID.IceBlock, TileID.Grass, TileID.Dirt);

            WorldHelper.Cleaning(SnowVilagePositionX + 34, SnowVilagePositionY - 11, SnowVilagePositionX + 35, SnowVilagePositionY - 10, TileID.SnowBlock, TileID.IceBlock, TileID.Trees, TileID.Grass, TileID.Dirt);
            WorldHelper.Cleaning(SnowVilagePositionX + 7,  SnowVilagePositionY - 18, SnowVilagePositionX + 56, SnowVilagePositionY - 11, TileID.SnowBlock, TileID.IceBlock, TileID.Trees, TileID.Grass, TileID.Dirt); // - первый 2 домика
            WorldHelper.Cleaning(SnowVilagePositionX + 57, SnowVilagePositionY - 30, SnowVilagePositionX + 77, SnowVilagePositionY - 10, TileID.SnowBlock, TileID.IceBlock, TileID.Grass, TileID.Dirt); // - двор
            WorldHelper.Cleaning(SnowVilagePositionX + 78, SnowVilagePositionY - 20, SnowVilagePositionX + 97, SnowVilagePositionY - 9,  TileID.SnowBlock, TileID.IceBlock, TileID.Trees, TileID.Grass, TileID.Dirt); // - последний дом

            for (int X = 0; X < width; X++)
            {
                for (int Y = 0; Y < height; Y++)
                {
                    int worldX = SnowVilagePositionX + X;
                    int worldY = SnowVilagePositionY - Y;

                    if (!WorldGen.InWorld(worldX, worldY, 10))
                        continue;

                    Tile tile = Framing.GetTileSafely(SnowVilagePositionX + X, SnowVilagePositionY - Y);

                    switch (SnowVilageTiles[Y, X])
                    {
                        case 0: break;
                        case 1: tile.TileType = TileID.SnowBlock; tile.HasTile = true; break;
                        case 2: tile.TileType = TileID.IceBlock; tile.HasTile = true; break;
                        case 3: tile.TileType = TileID.StoneSlab; tile.HasTile = true; break;
                        case 4: tile.TileType = TileID.Stone; tile.HasTile = true; break;
                        case 5: tile.TileType = TileID.GrayBrick; tile.HasTile = true; break;
                        case 6: tile.TileType = (ushort)ModContent.TileType<CorrodeBrick>(); tile.HasTile = true; break;
                        case 7: tile.TileType = (ushort)ModContent.TileType<ValhalliteBrick>(); tile.HasTile = true; break;
                        case 8: tile.TileType = RoA.Find<ModTile>("ElderwoodBeam").Type; tile.HasTile = true; break;
                        case 9: tile.TileType = TileID.LivingWood; tile.HasTile = true; break;
                        case a: WorldGen.PlaceTile(SnowVilagePositionX + X, SnowVilagePositionY - Y, TileID.Platforms, false, false, -1, 19); break;
                        case b: tile.TileType = RoA.Find<ModTile>("ElderwoodPlatform").Type; tile.HasTile = true; break;
                        case c: tile.TileType = (ushort)ModContent.TileType<PurpleBrickPlatform>(); tile.HasTile = true; break;
                        case d: tile.TileType = TileID.WoodBlock; tile.HasTile = true; break;
                        case e: tile.TileType = RoA.Find<ModTile>("Elderwood").Type; tile.HasTile = true; break;
                        case f: tile.TileType = (ushort)ModContent.TileType<TinRoof>(); tile.HasTile = true; break;
                        case g: tile.TileType = TileID.Chain; tile.HasTile = true; break;
                        case h: tile.TileType = TileID.SilverBrick; tile.HasTile = true; break;
                        case i: tile.TileType = TileID.Diamond; tile.HasTile = true; break;
                        case j: WorldGen.PlaceTile(SnowVilagePositionX + X, SnowVilagePositionY - Y, TileID.Platforms, false, false, -1, 6); break;
                        case k: tile.TileType = TileID.AmberStoneBlock; tile.HasTile = true; break;
                        case l: tile.TileType = (ushort)ModContent.TileType<Zircon>(); tile.HasTile = true; break;
                    }
                    switch (SnowVilageWalls[Y, X])
                    {
                        case 0: WorldGen.KillWall(X, Y); break;
                        case 1: tile.WallType = RoA.Find<ModWall>("BackwoodsRootWall").Type; break;
                        case 2: tile.WallType = WallID.StoneSlab; break;
                        case 3: tile.WallType = WallID.PalmWoodFence; tile.WallColor = PaintID.BrownPaint; break;
                        case 4: tile.WallType = WallID.ShadewoodFence; tile.WallColor = PaintID.BrownPaint; break;
                        case 5: tile.WallType = WallID.GrayBrick; break;
                        case 6: tile.WallType = WallID.LivingWood; break;
                        case 7: tile.WallType = WallID.RichMahoganyFence; tile.WallColor = PaintID.BrownPaint; break;
                        case 8: tile.WallType = WallID.Wood; break;
                        case 9: tile.WallType = WallID.Planked; break;
                        case a: tile.WallType = WallID.Glass; break;
                        case b: tile.WallType = WallID.IceBrick; break;
                        case c: tile.WallType = (ushort)ModContent.WallType<ResistantWoodFence>(); break;
                        case d: tile.WallType = tile.WallType = RoA.Find<ModWall>("ElderwoodWall").Type; break;
                        case e: tile.WallType = WallID.Cloud; break;
                        case f: tile.WallType = WallID.TinBrick; break;
                    }
                    switch (SnowVilageSlopes[Y, X])
                    {
                        case 0: break;
                        case 1: tile.IsHalfBlock = true; break;
                        case 2: tile.Slope = SlopeType.SlopeDownRight; break;
                        case 3: tile.Slope = SlopeType.SlopeUpLeft; break;
                        case 4: tile.Slope = SlopeType.SlopeUpRight; break;
                        case 5: tile.Slope = SlopeType.SlopeDownLeft; break;
                    }
                }
            }

            WorldGen.Place2x2(SnowVilagePositionX + 96, SnowVilagePositionY - 8, (ushort)ModContent.TileType<FermentingBarrel>(), 0);
            WorldGen.Place3x2(SnowVilagePositionX + 88, SnowVilagePositionY - 9, (ushort)ModContent.TileType<Oven>());

            WorldGen.PlaceObject(SnowVilagePositionX + 86, SnowVilagePositionY - 9,  (ushort)ModContent.TileType<SmoothMarblePillarBroken>());
            WorldGen.PlaceObject(SnowVilagePositionX + 80, SnowVilagePositionY - 9,  (ushort)ModContent.TileType<ResistantWoodTable>());
            WorldGen.PlaceObject(SnowVilagePositionX + 66, SnowVilagePositionY - 9,  ValhallaMod.Find<ModTile>("Millstone").Type);
            WorldGen.PlaceObject(SnowVilagePositionX + 5,  SnowVilagePositionY - 10, TileID.Statues, mute: false, 32);
            WorldGen.PlaceObject(SnowVilagePositionX + 21, SnowVilagePositionY - 11, TileID.Statues);
            WorldGen.PlaceObject(SnowVilagePositionX + 62, SnowVilagePositionY - 10, TileID.Lampposts);
            WorldGen.PlaceObject(SnowVilagePositionX + 29, SnowVilagePositionY - 10, TileID.Lampposts);
            WorldGen.PlaceObject(SnowVilagePositionX + 56, SnowVilagePositionY - 10, TileID.WaterFountain, mute: false, 3);
            WorldGen.PlaceObject(SnowVilagePositionX + 27, SnowVilagePositionY - 11, (ushort)ModContent.TileType<Avalon.Tiles.NickelAnvil>());
            WorldGen.PlaceObject(SnowVilagePositionX + 18, SnowVilagePositionY - 11, (ushort)ModContent.TileType<Workbench>());
            WorldGen.PlaceObject(SnowVilagePositionX + 16, SnowVilagePositionY - 11, TileID.FishingCrate, mute: false, 18);
            WorldGen.PlaceObject(SnowVilagePositionX + 9,  SnowVilagePositionY - 11, (ushort)ModContent.TileType<LaminatedTable>());
            WorldGen.PlaceObject(SnowVilagePositionX + 11, SnowVilagePositionY - 11, (ushort)ModContent.TileType<Chair>(), mute: false, 2);
            WorldGen.PlaceObject(SnowVilagePositionX + 13, SnowVilagePositionY - 11, (ushort)ModContent.TileType<LaminatedBed>());
            WorldGen.PlaceObject(SnowVilagePositionX + 38, SnowVilagePositionY - 11, TileID.Lamps);
            WorldGen.PlaceObject(SnowVilagePositionX + 40, SnowVilagePositionY - 11, TileID.Sawmill);
            WorldGen.PlaceObject(SnowVilagePositionX + 42, SnowVilagePositionY - 11, TileID.WorkBenches, mute: false, 43);
            WorldGen.PlaceObject(SnowVilagePositionX + 45, SnowVilagePositionY - 11, (ushort)ModContent.TileType<ResistantWoodClock>());
            WorldGen.PlaceObject(SnowVilagePositionX + 49, SnowVilagePositionY - 11, TileID.Fireplace);
            WorldGen.PlaceObject(SnowVilagePositionX + 90, SnowVilagePositionY - 9,  TileID.Lamps);
            WorldGen.PlaceObject(SnowVilagePositionX + 92, SnowVilagePositionY - 11, TileID.ClosedDoor, mute: false, 30);
            WorldGen.PlaceObject(SnowVilagePositionX + 76, SnowVilagePositionY - 11, TileID.ClosedDoor, mute: false, 30);
            WorldGen.PlaceObject(SnowVilagePositionX + 78, SnowVilagePositionY - 13, TileID.FishingCrate, mute: false, 1);
            WorldGen.PlaceObject(SnowVilagePositionX + 42, SnowVilagePositionY - 12, TileID.MusicBoxes, mute: false, 14);
            WorldGen.PlaceObject(SnowVilagePositionX + 52, SnowVilagePositionY - 12, RoA.Find<ModTile>("ElderwoodDoorClosed").Type);
            WorldGen.PlaceObject(SnowVilagePositionX + 22, SnowVilagePositionY - 12, TileID.ClosedDoor, mute: false, 44);
            WorldGen.PlaceObject(SnowVilagePositionX + 6,  SnowVilagePositionY - 12, TileID.ClosedDoor, mute: false, 30);
            WorldGen.PlaceObject(SnowVilagePositionX + 10, SnowVilagePositionY - 18, TileID.Furnaces);
            WorldGen.PlaceObject(SnowVilagePositionX + 9,  SnowVilagePositionY - 16, TileID.Banners, false, 2);
            WorldGen.PlaceObject(SnowVilagePositionX + 12, SnowVilagePositionY - 16, TileID.Banners, false, 2);
            WorldGen.PlaceObject(SnowVilagePositionX + 89, SnowVilagePositionY - 12, TileID.LightningBuginaBottle);

            WorldGen.Place6x4Wall(SnowVilagePositionX + 15,  SnowVilagePositionY - 14, (ushort)ModContent.TileType<RiseOfTheOldGod>(), 0);

            WorldGen.Place1x1(SnowVilagePositionX + 37, SnowVilagePositionY - 11, TileID.ClayPot, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 7,  SnowVilagePositionY - 13, TileID.Candles, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 80, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 81, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 82, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 42, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 43, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 19, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 20, SnowVilagePositionY - 15, TileID.Books, 0);
            WorldGen.Place1x1(SnowVilagePositionX + 7,  SnowVilagePositionY - 15, TileID.Candles, 50);
            WorldGen.Place1x2(SnowVilagePositionX + 78, SnowVilagePositionY - 9,  TileID.Chairs, 17);
            WorldGen.Place1x2(SnowVilagePositionX + 51, SnowVilagePositionY - 11, TileID.Chairs, 17);

            WorldGen.Place3x2(SnowVilagePositionX + 83, SnowVilagePositionY - 9, TileID.Dressers, 18);

            WorldGen.Place2x1(SnowVilagePositionX + 79, SnowVilagePositionY - 11, TileID.Bowls, 0);
            WorldGen.Place2x1(SnowVilagePositionX + 8,  SnowVilagePositionY - 13, TileID.Bowls, 1);
            WorldGen.Place2x2(SnowVilagePositionX + 88, SnowVilagePositionY - 14, TileID.UlyssesButterflyJar, 0);
            WorldGen.Place2x2(SnowVilagePositionX + 90, SnowVilagePositionY - 14, TileID.Heart, 0);
            WorldGen.Place2x2(SnowVilagePositionX + 49, SnowVilagePositionY - 13, TileID.FishBowl, 0);
            WorldGen.Place2x2(SnowVilagePositionX + 84, SnowVilagePositionY - 11, (ushort)ModContent.TileType<OtherworldlyMusicBox2>(), 0);

            WorldGen.Place3x2(SnowVilagePositionX + 25, SnowVilagePositionY - 11, TileID.Blendomatic, 0);
            WorldGen.Place3x3(SnowVilagePositionX + 85, SnowVilagePositionY - 16, (ushort)ModContent.TileType<ZincChandelier>(), 0);

            WorldGen.PlaceBanner(SnowVilagePositionX + 91, SnowVilagePositionY - 16, TileID.Banners, 124);
            WorldGen.PlaceBanner(SnowVilagePositionX + 83, SnowVilagePositionY - 16, TileID.Banners, 126);

            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 81, SnowVilagePositionY - 11, TileID.Bottles, 4);
            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 82, SnowVilagePositionY - 11, RoA.Find<ModTile>("ElderwoodCandle").Type, 0);
            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 50, SnowVilagePositionY - 13, TileID.Bottles, 1);
            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 18, SnowVilagePositionY - 12, TileID.Bottles, 1);
            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 19, SnowVilagePositionY - 12, TileID.Candles, 0);
            WorldGen.PlaceOnTable1x1(SnowVilagePositionX + 10, SnowVilagePositionY - 13, TileID.Bottles, 4);

            int BarrelIndex = WorldGen.PlaceChest(SnowVilagePositionX + 33, SnowVilagePositionY - 9, (ushort)ModContent.TileType<Chest>(), false, 2);
            if (BarrelIndex != -1) { GenerateBarrelLoot(Main.chest[BarrelIndex].item, 0); }

            return true;
        }
        static void GenerateBarrelLoot(Item[] ChestInventory, int BarrelIndex)
        {
            WorldHelper.RandomLootInCoutainer(ChestInventory, ref BarrelIndex, 1, 1, ItemID.IceBlade, ItemID.IceBoomerang, ItemID.Snowball);
            WorldHelper.RandomLootInCoutainer(ChestInventory, ref BarrelIndex, 1, 1, ItemID.BlizzardinaBottle, ItemID.FlurryBoots, ItemID.IceSkates);
            WorldHelper.LootInContainers(ChestInventory, ref BarrelIndex, ItemID.Fish, 1, 1);
            WorldHelper.RandomLootInCoutainer(ChestInventory, ref BarrelIndex, 3, 7, ItemID.Topaz, ItemID.Amethyst, ItemID.Sapphire, ItemID.Amber, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond, ModContent.ItemType<Avalon.Items.Material.Ores.Tourmaline>(), ModContent.ItemType<Avalon.Items.Material.Ores.Zircon>());
            WorldHelper.IfOreTireLoot(ChestInventory, ref BarrelIndex, 4, ItemID.GoldBar, ItemID.PlatinumBar, 5, 15);
            WorldHelper.LootInContainers(ChestInventory, ref BarrelIndex, ItemID.FlinxFur, 5, 10);
            WorldHelper.LootInContainers(ChestInventory, ref BarrelIndex, ItemID.HealingPotion, 3, 5);
            WorldHelper.LootInContainers(ChestInventory, ref BarrelIndex, ItemID.GoldCoin, 1, 3);
        }
    }
}