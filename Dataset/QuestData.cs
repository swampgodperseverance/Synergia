using System.Collections.Generic;

namespace Synergia.Dataset;

public struct QuestData(string questKey, byte progress, byte maxProgress, bool isFirstClicked) {
    public byte Progress = progress;
    public byte MaxProgress = maxProgress;
    public string QuestKey = questKey;
    public bool IsFirstClicked = isFirstClicked;
    public byte X = 0;
    public List<int> ItemList;

    public QuestData(string questKey, byte progress, byte maxProgress, bool isFirstClicked, byte x): this(questKey, progress, maxProgress, isFirstClicked) {
        X = x;
    }
    public QuestData(string questKey, byte progress, byte maxProgress, bool isFirstClicked, List<int> itemList) : this(questKey, progress, maxProgress, isFirstClicked) {
        ItemList = itemList;
    }
}