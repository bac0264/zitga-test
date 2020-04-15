[System.Serializable]
public class MapDataStat
{
    public int ID;
    public int STAR;
    public bool IS_OPEN;
    public MapDataStat(int ID, int STAR = 0, bool IS_OPEN = false)
    {
        this.ID = ID;
        this.STAR = STAR;
        this.IS_OPEN = IS_OPEN;
    }
}