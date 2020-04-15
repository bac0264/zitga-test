using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class DataService : IDataService
{

    DataSave<MapDataStat> MapDataStat_DataSave;
    public DataService()
    {
        if (PlayerPrefs.GetInt("First", 0) == 0)
        {
            //Debug.Log("run");
            PlayerPrefs.SetInt("First", 1);
            AddMaps();
            Save();
        }
        else
            Load();
    }

    // For the first
    #region
    void AddMaps()
    {
        int CurMapCount = 999;
        MapDataStat_DataSave = new DataSave<MapDataStat>();
        int i = 0;
        for (; i < CurMapCount; i++)
        {
            MapDataStat map = new MapDataStat(i);
            if (i == 0)
                map.IS_OPEN = true;
            MapDataStat_DataSave.Add(map);
        }
    }
    //void CheckMapInPref()
    //{
    //    int MapInPrefbs = 100;
    //    MapDataStat_DataSave = new DataSave<MapDataStat>();
    //    int i = 0;
    //    for (; i < MapInPrefbs && i < MapDataStat_DataSave.results.Count; i++)
    //    {
    //    }
    //    if (i < MapInPrefbs)
    //    {
    //        for (; i < MapInPrefbs; i++)
    //        {
    //            MapDataStat map = new MapDataStat(i);
    //            MapDataStat_DataSave.Add(map);
    //        }
    //        Save<MapDataStat>();
    //        Debug.Log(JsonUtility.ToJson(MapDataStat_DataSave));
    //    }
    //}
    #endregion
    public DataSave<T> GetDataSaveWithType<T>()
    {
        if (typeof(T).ToString().Equals(typeof(MapDataStat).ToString())) return (DataSave<T>)(object)MapDataStat_DataSave;
        return null;
    }
    public void ResetData()
    {
        MapDataStat_DataSave = null;
    }
    void Save()
    {
        string dataMap = JsonUtility.ToJson(MapDataStat_DataSave);

        PlayerPrefs.SetString(KeySave.DATA_MAP, dataMap);
    }
    public void Save<T>()
    {
        if (typeof(T).ToString().Equals(typeof(MapDataStat).ToString())) PlayerPrefs.SetString(KeySave.DATA_MAP, JsonUtility.ToJson(MapDataStat_DataSave));
    }
    public void Load()
    {  
        MapDataStat_DataSave = JsonUtility.FromJson<DataSave<MapDataStat>>(PlayerPrefs.GetString(KeySave.DATA_MAP));       
    }
}

[System.Serializable]
public class DataSave<T>
{
    public List<T> results;
    public DataSave()
    {
        results = new List<T>();
    }
    public void Add(T b)
    {
        results.Add(b);
    }
}
