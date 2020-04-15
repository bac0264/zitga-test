using UnityEngine;
using System.Collections;

public class MapSlotList : _SlotListSetup<MapSlot,MapDataStat>
{
    public override void OnValidate()
    {
        base.OnValidate();
    }
    public override void SetupEvent()
    {
        base.SetupEvent();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Setup(MapDataStat[] dataBase = null)
    {
        base.Setup(dataBase);
    }
    public void SetupMapManager(IMapManager mapManager)
    {
        foreach(MapSlot slot in slotList)
        {
            slot.SetupMapManager(mapManager);
        }
    }
}
