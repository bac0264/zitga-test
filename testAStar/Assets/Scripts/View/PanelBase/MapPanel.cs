using UnityEngine;
using System.Collections;

public class MapPanel : _PanelSetup<MapSlot, MapDataStat>
{
    public static MapPanel instance;
    public MapEnhance enhance;
    
    IMapManager mapManager;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        SetupData();
    } 
    public void SetupData()
    {
        mapManager = DIContainer.GetModule<IMapManager>();
        InjectData(mapManager);
    }
    public void InjectData(IMapManager mapManager)
    {
        this.mapManager = mapManager;
        Setup(mapManager.MapDataList());
    }
    public override void Setup(MapDataStat[] dataBase = null)
    {
        base.Setup(dataBase);
    }
    public override void HideKey()
    {
        base.HideKey();
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }
    public override void ShowKey()
    {
        base.ShowKey();
    }
    public override void ShowPanel()
    {
        if (mapManager != null)
        {
            InjectData(mapManager);
        }
        base.ShowPanel();
    }
    public override void OnValidate()
    {
        base.OnValidate();
    }
}
