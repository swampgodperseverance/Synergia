using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.Rarities;
using Synergia.Common.SynergiaCondition;
using Synergia.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges.Avalons;
using static Synergia.Common.SUtils.LocUtil;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.Misc {
    public partial class NULLItem {
        public abstract class BaseNULLItem : ModItem {
            readonly UIHelper Helper = new();

            public abstract int CloneItem { get; }
            public abstract List<int> AnimationList { get; }
            public virtual bool IsNotAnimationItem => false;
            public virtual bool ItemIconPulse => true;
            public virtual int TimeToNextItem => 60;
            public virtual float Scale => 0;
            public sealed override string Texture => Synergia.Blank;
            public sealed override void SetDefaults() {
                Item.CloneDefaults(CloneItem);
            }
            public sealed override void SetStaticDefaults() {
                ItemID.Sets.ItemIconPulse[Type] = ItemIconPulse;
            }
            public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
                int type = Helper.GetAnimatedItemType(AnimationList, TimeToNextItem);
                Texture2D animation = TextureAssets.Item[type].Value;
                spriteBatch.Draw(animation, position, animation.Bounds, Color.White, 0f, animation.Bounds.Size() * 0.5f, scale - Scale, SpriteEffects.None, 0f);
                return IsNotAnimationItem;
            }
            public override void ModifyTooltips(List<TooltipLine> tooltips) {
                tooltips.Add(new TooltipLine(Mod, "TooltipRecipeGroups", $"{ItemTooltip(WEP, "GroupsItem")} {TextureAndName()}"));
                tooltips.Add(new TooltipLine(Mod, "TooltipRecipeGroupsInf", ItemTooltip(WEP, "Inf")) { OverrideColor = NULLItemRarity.GetColor() });
            }
            string TextureAndName() {
                if (AnimationList == null || AnimationList.Count == 0) {
                    return "";
                }

                List<string> names = [];

                foreach (int type in AnimationList) {
                    Item item = new();
                    item.SetDefaults(type);
                    names.Add($"[i:{item.type}]" + " " + item.Name);
                }
                return string.Join(", ", names) + ".";
            }
            public override void AddRecipes() {
                Recipe recipe = CreateRecipe();
                for (int i = 0; i < AnimationList.Count;) {
                    recipe.AddIngredient(AnimationList[i]);
                    i++;
                }
                recipe.AddCondition(RecipeCondition.NULLitem(Item));
                recipe.Register();
            }
        }
        public class FragmentSolar : BaseNULLItem {
            public override int CloneItem => ItemID.FragmentSolar;
            public override List<int> AnimationList => [ItemID.FragmentSolar, ItemID.FragmentVortex];
        }
        public class FragmentStardust : BaseNULLItem {
            public override int CloneItem => ItemID.FragmentSolar;
            public override List<int> AnimationList => [ItemID.FragmentStardust, ItemID.FragmentNebula];
        }
        public class HardModeAnvil : BaseNULLItem {
            public override int CloneItem => ItemID.MythrilAnvil;
            public override List<int> AnimationList => [ItemID.MythrilAnvil, ItemID.OrichalcumAnvil, NaquadahAnvil];
            public override bool ItemIconPulse => false;
        }
        public class HardModeForge : BaseNULLItem {
            public override int CloneItem => ItemID.TitaniumForge;
            public override List<int> AnimationList => [ItemID.TitaniumForge, ItemID.AdamantiteForge, TroxiniumForge];
            public override float Scale => 0.3f;
            public override bool ItemIconPulse => false;
        }
        public class AlchemicOre : BaseNULLItem {
            public override int CloneItem => ItemID.IronOre;
            public override List<int> AnimationList => [ItemID.IronOre, ItemID.LeadOre, ItemType<NickelOre>()];
            public override bool ItemIconPulse => false;
        }
        public class AlchemicFlower : BaseNULLItem {
            public override int CloneItem => ItemID.Deathweed;
            public override List<int> AnimationList => [ItemID.Deathweed, ItemType<Bloodberry>(), ItemType<Barfbush>()];
            public override bool ItemIconPulse => false;
        } 
    }
}