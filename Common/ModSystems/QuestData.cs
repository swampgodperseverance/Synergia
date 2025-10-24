namespace Synergia.Common.ModSystems
{
    public struct QuestData(byte x, string questkey, byte progres, byte maxprogres)
    {
        public byte X = x;
        public byte Progres = progres;
        public byte MaxProgres = maxprogres;
        public string QuestKey = questkey;
    }
}