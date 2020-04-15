using UnityEngine;
using System.Collections;

public class MapManager : IMapManager
{
    IDataService dataService;
    DataSave<MapDataStat> dataSave;
    MapDataStat currentLevel;
    public void SetPick(MapDataStat current)
    {
        for(int i = 0; i < dataSave.results.Count && i < current.ID; i++)
        {
            dataSave.results[i].IS_OPEN = true;
            dataSave.results[i].STAR = (int)Random.Range(1, 3);
        }
        SaveMaps();
    }
    public MapManager(IDataService dataService)
    {
        this.dataService = dataService;
        LoadMaps();
    }
    public MapDataStat HIGHEST_MAP
    {
        get
        {
            if (JsonUtility.FromJson<MapDataStat>(PlayerPrefs.GetString("HighestMap")) == null)
                return JsonUtility.FromJson<MapDataStat>(PlayerPrefs.GetString("HighestMap"));
            MapDataStat data = GetMapWithID(JsonUtility.FromJson<MapDataStat>(PlayerPrefs.GetString("HighestMap")).ID);
            return data;
        }
    }
    public MapDataStat CUR_MAP
    {
        get
        {
            MapDataStat data = GetMapWithID(JsonUtility.FromJson<MapDataStat>(PlayerPrefs.GetString("CurrentMap")).ID);
            return data;
        }
        set
        {
            if (value != null)
            {
                currentLevel = value;
                if (currentLevel != null)
                {
                    PlayerPrefs.SetString("CurrentMap", JsonUtility.ToJson(currentLevel));
                    MapDataStat highest = HIGHEST_MAP;
                    if (highest == null || highest.ID <= currentLevel.ID)
                    {
                        PlayerPrefs.SetString("HighestMap", JsonUtility.ToJson(currentLevel));
                    }
                }
            }
        }
    }

    public MapDataStat GetMapWithID(int id)
    {
        if (id < dataSave.results.Count) return dataSave.results[id];
        return null;
    }

    public MapDataStat[] MapDataList()
    {
        if (dataSave != null) return dataSave.results.ToArray();
        return null;
    }
    public void LoadMaps()
    {
        dataSave = dataService.GetDataSaveWithType<MapDataStat>();
        if (PlayerPrefs.GetString("CurrentMap", "") == "")
        {
            if (dataSave.results.Count > 0)
            {
                CUR_MAP = dataSave.results[0];
            }
        }
        else
        {
            CUR_MAP = HIGHEST_MAP;
        }
    }
    public void SaveMaps()
    {
        dataService.Save<MapDataStat>();
    }

    public bool SetupNextLevel(MapDataStat cur)
    {
        MapDataStat next = GetMapWithID(cur.ID + 1);
        next.IS_OPEN = true;
        if (cur.ID >= dataSave.results[dataSave.results.Count - 1].ID || cur.ID < HIGHEST_MAP.ID)
        {
            CUR_MAP = next;
            SaveMaps();
            return false;
        }
        CUR_MAP = next;
        SaveMaps();
        return true;
    }
}