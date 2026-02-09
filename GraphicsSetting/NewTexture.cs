using Avalon.Items.Weapons.Magic.Hardmode.AquaImpact;
using Avalon.Items.Weapons.Magic.Hardmode.Sunstorm;
using Bismuth.Content.Items.Weapons.Throwing;
using Bismuth.Content.Projectiles;
using Terraria.GameContent;
using ValhallaMod.Items.Weapons.Aura;

namespace Synergia.GraphicsSetting {
    public static class NewTexture {
        // name Texture TextureName+Resprite = TextureNameResprite 
        public enum EquableType {
            Head,
            Body,
            Legs,
        }
        public static void Load() {
            TextureWeapons(ItemType<OrcishJavelin>());
            TextureWeapons(ItemType<Sunstorm>());
            TextureWeapons(ItemType<StarAuraStaff>());
            TextureWeapons(ItemType<TitaniumJavelin>());
            TextureWeapons(ItemType<AquaImpact>());

            TextureProj(ProjectileType<OrcishJavelinP>());
        }
        public static void Unload() {
            TextureAssets.Item[ItemType<OrcishJavelin>()] = Request<Texture2D>("Bismuth/Content/Items/Weapons/Throwing/OrcishJavelin");
            TextureAssets.Item[ItemType<Sunstorm>()] = Request<Texture2D>("Avalon/Items/Weapons/Magic/Hardmode/Sunstorm/Sunstorm");
            TextureAssets.Item[ItemType<StarAuraStaff>()] = Request<Texture2D>("ValhallaMod/Items/Weapons/Aura/StarAuraStaff");
            TextureAssets.Item[ItemType<TitaniumJavelin>()] = Request<Texture2D>("Bismuth/Content/Items/Weapons/Throwing/TitaniumJavelin");
            TextureAssets.Item[ItemType<AquaImpact>()] = Request<Texture2D>("Avalon/Items/Weapons/Magic/Hardmode/AquaImpact/AquaImpact");

            TextureAssets.Projectile[ProjectileType<OrcishJavelinP>()] = Request<Texture2D>("Bismuth/Content/Projectiles/OrcishJavelinP"); 
        }
        public static string GetName(int type, string category) {
            string internalName = "";
            switch (category) {
                case "item": ModItem modItem = ItemLoader.GetItem(type);             internalName = modItem.Name; break;
                case "npc" : ModNPC modNpc   = NPCLoader.GetNPC(type);               internalName = modNpc.Name;  break;
                case "tile": ModTile modTile = TileLoader.GetTile(type);             internalName = modTile.Name; break;
                case "proj": ModProjectile P = ProjectileLoader.GetProjectile(type); internalName = P.Name;       break;
            }
            return internalName;
        }
        public static void TextureWeapons(int itemID, string fileName) => TextureAssets.Item[itemID] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}");
        public static void TextureWeapons(int itemID) => TextureAssets.Item[itemID] = Request<Texture2D>($"Synergia/Assets/Resprites/{GetName(itemID, "item") + "Resprite"}");
        public static void TextureNPC(int npcID, string fileName) => TextureAssets.Npc[npcID] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}");
        public static void TextureNPC(int npcID) => TextureAssets.Npc[npcID] = Request<Texture2D>($"Synergia/Assets/Resprites/{GetName(npcID, "npc") + "Resprite"}");
        public static void TextureTiles(int tileID, string fileName) => TextureAssets.Tile[tileID] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}");
        public static void TextureTiles(int tileID) => TextureAssets.Tile[tileID] = Request<Texture2D>($"Synergia/Assets/Resprites/{GetName(tileID, "tile") + "Resprite"}");
        public static void TextureProj(int projID, string fileName) => TextureAssets.Projectile[projID] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}");
        public static void TextureProj(int projID) => TextureAssets.Projectile[projID] = Request<Texture2D>($"Synergia/Assets/Resprites/{GetName(projID, "proj") + "Resprite"}");
        public static void TextureExtra(int extraID, string fileName) => TextureAssets.Extra[extraID] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}");
        public static void TextureArmor(EquableType equableType, int armType, string fileName) {
            switch (equableType) {
                case EquableType.Head: TextureAssets.ArmorHead[armType] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}"); break;
                case EquableType.Body: TextureAssets.ArmorBody[armType] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}"); break;
                case EquableType.Legs: TextureAssets.ArmorLeg [armType] = Request<Texture2D>($"Synergia/Assets/Resprites/{fileName}"); break;
            }
        }
    }
}