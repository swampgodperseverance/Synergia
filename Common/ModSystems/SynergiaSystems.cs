namespace Synergia.Common.ModSystems {
    public class SynergiaSystems : ModSystem {
        public override void PostUpdateEverything() {
            TimerSystem.Update();
        }
    }
}
