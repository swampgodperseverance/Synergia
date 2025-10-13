using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.Command
{
    public class MousePosition : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command  => "getMP";  
        public override string Usage => "/getMP true or false";
        public override string Description => "Gives you Mouse Position";
        public override void Action(CommandCaller caller, string input, string[] args) => Main.LocalPlayer.GetModPlayer<CommandPlayer>().ViewCoordinates = bool.Parse(args[0]);
    }
}