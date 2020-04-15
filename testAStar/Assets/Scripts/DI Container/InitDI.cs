using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class InitDI : MonoBehaviour
{
    public GameObject map;
    public Text randomValue;
    private void Awake()
    {
        DIContainer.SetModule<IDataService, DataService>();
        DIContainer.SetModule<IMapManager, MapManager>();
    }
    private void Start()
    {
        Invoke("Open", 0.1f);
    }
    public void Open()
    {
        if (map != null)
            map.SetActive(true);
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
    public void RandomUnlock()
    {
        int random = Random.Range(1, 1000);
        randomValue.text = "Unlock level: " + (random + 1).ToString();
        MapDataStat dataStat = DIContainer.GetModule<IMapManager>().GetMapWithID(random);
        dataStat.IS_OPEN = true;
        DIContainer.GetModule<IMapManager>().GetMapWithID(dataStat.ID);
        DIContainer.GetModule<IMapManager>().SetPick(dataStat);
        MapPanel.instance.enhance.masterScroller.ReloadData();
        MapPanel.instance.enhance.masterScroller.JumpToDataIndex(dataStat.ID / 4);
    }
}
