//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using System.Collections.Generic; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using System.IO; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using Terraria; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using Terraria.GameContent.Generation; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using Terraria.ModLoader.IO; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
using Terraria.WorldBuilding; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
namespace Synergia.Common.ModSystems.WorldGens {//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
    public abstract class BaseWorldGens : ModSystem { //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋ //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋ //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public const byte a = 10, b = 11, c = 12, d = 13, e = 14, f = 15, g = 16, h = 17, i = 18, j = 19, k = 20, l = 21;//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public virtual string SaveName => GetType().Name; //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋ //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋ //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public abstract bool GensBool { get; set; }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public virtual string NameGen => "Error";//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public string Favorit = "Final Cleanup";//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public virtual string VanillaIndexName => "Final Cleanup";//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) { //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            int index = tasks.FindIndex(x => x.Name == VanillaIndexName); //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            if (index != -1) {//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
                tasks.Insert(index, new PassLegacy(NameGen, (progress, config) => AddGen(progress)));
            }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        protected void AddGen(GenerationProgress progress = null) { //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋ //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            if (GensBool) return;//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            bool Success = Do_MakeGen(progress);//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            if (Success) GensBool = true;//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public abstract bool Do_MakeGen(GenerationProgress progress);//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void OnWorldLoad() {//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            GensBool = false;//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void SaveWorldData(TagCompound tag) {//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            tag[SaveName] = GensBool;//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void LoadWorldData(TagCompound tag) {
            GensBool = tag.GetBool(SaveName);//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void NetSend(BinaryWriter writer) {//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            writer.Write(GensBool);//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        public override void NetReceive(BinaryReader reader) { //Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
            GensBool = reader.ReadBoolean();//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
        }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
    }//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
}//Code by C̶͈̤̗͗̐̉͌L̶̟͖͌̒ E҈ ̏̒̂ ͓̩̮Ȏ̴ ͖̲̜̲́͆͋
