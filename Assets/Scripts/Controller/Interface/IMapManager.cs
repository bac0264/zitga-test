using UnityEngine;
using System.Collections;

public interface IMapManager
{
    void SetPick(MapDataStat pick);
    MapDataStat[] MapDataList();
    MapDataStat GetMapWithID(int id);
    MapDataStat HIGHEST_MAP
    {
        get;
    }
    MapDataStat CUR_MAP
    {
        set;
        get;
    }
    bool SetupNextLevel(MapDataStat cur);
    void LoadMaps();

    void SaveMaps();
}
