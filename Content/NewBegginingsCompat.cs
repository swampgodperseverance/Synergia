﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Synergia.Content
{
    internal class NewBeginningsCompat
    {
        internal static void AddOrigin()
        {
            // Проверяем наличие обоих модов
            if (ModLoader.TryGetMod("NewBeginnings", out Mod beginnings) &&
                ModLoader.TryGetMod("RoA", out Mod roa))
            {
                // Получаем типы предметов из RoA через отражение,
                // чтобы избежать жёсткой зависимости при отсутствии мода
                int sapStream = GetItemTypeSafe(roa, "RoA.Content.Items.Weapons.Druidic.SapStream");
                int herbarium = GetItemTypeSafe(roa, "RoA.Content.Items.Equipables.Accessories.Herbarium");

                if (sapStream <= 0 || herbarium <= 0)
                {
                    // что-то не найдено — не добавляем origin
                    return;
                }

                // Формируем данные
                object equip = beginnings.Call(
                    "EquipData",
                    ItemID.None,
                    ItemID.None,
                    sapStream,
                    new int[] { herbarium }
                );

                object misc = beginnings.Call(
                    "MiscData",
                    60, // HP
                    20, // Mana
                    -1,
                    ItemID.FlowerBoyHat
                );

                object dele = beginnings.Call(
                    "DelegateData",
                    () => true,
                    (List<GenPass> _) => { },
                    () => true,
                    (Action<Player>)(_ => { }) // заглушка
                );

                beginnings.Call(
                    "ShortAddOrigin",
                    ModContent.Request<Texture2D>("Synergia/Assets/Textures/NatureBeast"),
                    "NatureBeast",
                    "Mods.Synergia.Origins.NatureBeast",
                    Array.Empty<(int, int)>(),
                    equip, misc, dele
                );
            }
        }

        /// <summary>
        /// Безопасно получает ID предмета из другого мода по полному имени типа.
        /// Если класс отсутствует — возвращает -1.
        /// </summary>
        private static int GetItemTypeSafe(Mod mod, string fullName)
        {
            try
            {
                Type t = mod.Code.GetType(fullName);
                if (t == null)
                    return -1;

                var method = typeof(ModContent).GetMethod("ItemType", 1, Type.EmptyTypes);
                var generic = method.MakeGenericMethod(t);
                return (int)generic.Invoke(null, null);
            }
            catch
            {
                return -1;
            }
        }
    }
}
