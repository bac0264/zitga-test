using UnityEngine;
using System.Collections;
using EnhancedScrollerDemos.NestedScrollers;
using EnhancedUI.EnhancedScroller;

public class MapEnhance : Controller
{
    public static MapEnhance instance;
    IMapManager mapManager;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public override void Start()
    {
        base.Start();
        //mapManager = DIContainer.GetModule<IMapManager>();
        //Debug.Log(mapManager.HIGHEST_MAP.ID);
        //masterScroller.JumpToDataIndex(mapManager.HIGHEST_MAP.ID / 4);
    }
}
