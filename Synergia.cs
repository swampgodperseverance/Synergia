using Bismuth.Content.Items.Weapons.Throwing;
using Bismuth.Content.Projectiles;
using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Synergia.Common;
using Synergia.Content.Quests;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Synergia
{
    public class Synergia : Mod
    {
        internal UserInterface DwarfUserInterface;
        public const string ModName = "SynergiaModName";
        private Synergia instruktion;
        private ILHook ExtraMountCavesGeneratorILHook;
        static readonly Mod bismuth = ModLoader.GetMod("Bismuth");

        public override void Load()
        {
            #region UI
            DwarfUserInterface = new UserInterface();
            #endregion
            QuestRegistry.Register(new DwarfQuest());
            QuestRegistry.Register(new TaxCollectorQuest());
            instruktion = this;
            LoadRoAHook();
            ModList.LoadMod();
        }
        public override void Unload()
        {
            UnloadRoAHook();
        }
        public override void PostSetupContent()
        {
            base.PostSetupContent();

            if (!Main.dedServ)
            {
                // лучше сделать так, если будут еше респрайты для других предметов не только из бисмута.
                if (bismuth != null) {
                    TextureAssets.Item[ModContent.ItemType<OrcishJavelin>()] = ModContent.Request<Texture2D>("Synergia/Assets/Resprites/OrcishJavelinResprite");
                    TextureAssets.Projectile[ModContent.ProjectileType<OrcishJavelinP>()] = ModContent.Request<Texture2D>("Synergia/Assets/Resprites/OrcishJavelinResprite2");
                }
            }
        }
        public static string GetUIElementName(string element) => $"Synergia/Assets/UIs/{element}";
        void LoadRoAHook()
        {
            if (ModLoader.TryGetMod("RoA", out Mod RoAMod))
            {
                Type DryadEntranceClass = RoAMod.GetType().Assembly.GetType("RoA.Content.World.Generations.DryadEntrance");

                MethodInfo ExtraMountCavesGeneratorInfo = DryadEntranceClass.GetMethod("ExtraMountCavesGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
                ExtraMountCavesGeneratorILHook = new ILHook(ExtraMountCavesGeneratorInfo, ILExtraMountCavesGenerator);
            }
        }
        void UnloadRoAHook()
        {
            if (ExtraMountCavesGeneratorILHook != null)
            {
                ExtraMountCavesGeneratorILHook.Dispose();
                ExtraMountCavesGeneratorILHook = null;
            }
        }
        void ILExtraMountCavesGenerator(ILContext il)
        {
            try
            {
                var ilCursor = new ILCursor(il);
                ilCursor.GotoNext(i => i.MatchLdcI4(120));
                ilCursor.Remove();
                ilCursor.Emit(OpCodes.Ldc_I4, 230);
                ilCursor.GotoNext(i => i.MatchLdcI4(90));
                ilCursor.Remove();
                ilCursor.Emit(OpCodes.Ldc_I4, 200);
                ilCursor.GotoNext(i => i.MatchLdcI4(90));
                ilCursor.Remove();
                ilCursor.Emit(OpCodes.Ldc_I4, 200);
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<Synergia>(), il);
                throw new ILPatchFailureException(ModContent.GetInstance<Synergia>(), il, e);
            }
        }
    }
    public class MUtils : ModSystem
    {
        public static void DrawSimpleAfterImage(Color lightColor, Projectile projectile, Texture2D projectileTexture, float colorReduct = 1f, float scaleMult = 1f, float scaleReduct = 0.25f, float velOffset = 0f, float? yScaleDif = null, float extraRotate = 0f)
        {
            Vector2 toCenterTexture = new Vector2(projectileTexture.Width, projectileTexture.Height / Main.projFrames[projectile.type]) / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1) spriteEffects = SpriteEffects.FlipHorizontally;
            Rectangle source = new Rectangle(0, projectileTexture.Height / Main.projFrames[projectile.type] * projectile.frame, projectileTexture.Width, projectileTexture.Height / Main.projFrames[projectile.type]);
            for (byte k = 0; k < (byte)projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + new Vector2(projectile.width, projectile.height) / 2f;
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) * (float)(1f / (projectile.oldPos.Length * colorReduct)));
                //Vector2 oldVel = (projectile.oldVelocity).SafeNormalize(Vector2.Zero) * velOffset;
                Vector2 oldVel = (k == 0 ? projectile.oldPos[0] - projectile.position : projectile.oldPos[k] - projectile.oldPos[k - 1]).SafeNormalize(Vector2.Zero) * -velOffset;

                Vector2 scaling = new Vector2(projectile.scale * scaleMult - k / (float)projectile.oldPos.Length * scaleReduct);
                if (yScaleDif != null) scaling.Y *= (float)yScaleDif;
                float rotation = projectile.oldRot[k] * (1 + extraRotate);//*16f
                if (extraRotate == -69.1f) rotation = Main.GlobalTimeWrappedHourly * 16f * projectile.direction;
                if (extraRotate == -69.2f) rotation = Main.GlobalTimeWrappedHourly * 20f * projectile.direction;
                Main.spriteBatch.Draw(projectileTexture, drawPos - oldVel * k, new Rectangle?(source), color, rotation, toCenterTexture, scaling, spriteEffects, 0f);
            }
        }
    }
    public class Sounds : ModSystem
    {
        public static readonly SoundStyle BookOpenSound = new("Synergia/Assets/Sounds/BookOpen");
        public static readonly SoundStyle BookCloseSound = new("Synergia/Assets/Sounds/BookClose");
        public static readonly SoundStyle CragwormHit = new("Synergia/Assets/Sounds/CragwormHit");
        public static readonly SoundStyle CragwormHit2 = new("Synergia/Assets/Sounds/CragwormHit2");
        public static readonly SoundStyle WormRoar = new("Synergia/Assets/Sounds/WormRoar");
        public static readonly SoundStyle BrokenBone = new("Synergia/Assets/Sounds/BrokenBone");
        public static readonly SoundStyle SpearSound = new("Synergia/Assets/Sounds/SpearSound");
        public static readonly SoundStyle WormBoom = new("Synergia/Assets/Sounds/WormBoom");
        public static readonly SoundStyle LavaBone = new("Synergia/Assets/Sounds/LavaBone");
        public static readonly SoundStyle FeatherFlow = new("Synergia/Assets/Sounds/FeatherFlow");
        public static readonly SoundStyle WeakSword = new("Synergia/Assets/Sounds/WeakSword");
        public static readonly SoundStyle PowerSword = new("Synergia/Assets/Sounds/PowerSword");
        public static readonly SoundStyle SandSphere = new("Synergia/Assets/Sounds/SandSphere");
        public static readonly SoundStyle Impulse = new("Synergia/Assets/Sounds/Impulse");
        public static readonly SoundStyle Watersound = new("Synergia/Assets/Sounds/Watersound");
    }

    public class SynergiaSystems : ModSystem
    {
        public override void PostUpdateEverything()
        {
            TimerSystem.Update();
        }
    }
}