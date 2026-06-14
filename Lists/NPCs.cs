// Code by SerNik
using Synergia.Content.NPCs.Swamp;
using System.Collections.Generic;

namespace Synergia.Lists {
    public static class NPCs {
        public static List<int> NewHellNPCs { get; internal set; } = [];
        public static List<int> NewSwampNPCs { get; internal set; } = [NPCType<FrogRider>(), NPCType<MischievousDuo>(), NPCType<Swamling>()];
    }
}
