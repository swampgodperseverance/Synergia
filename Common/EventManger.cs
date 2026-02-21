// Code by 𝒜𝑒𝓇𝒾𝓈
using System.Collections.Generic;

namespace Synergia.Common {
    public class EventManger : ModSystem {
        public Dictionary<string, ModEvent> Events { get; private set; } = [];

        public void Register(ModEvent evt) => Events[evt.Name] = evt;
    }
}