using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSlot : _ActionSlotSetup<MapSlot, MapDataStat>
{
    IMapManager mapManager;
    public Transform StarContainer;
    public Image[] stars;
    public Image isOpen;
    public Button pickLevel;
    private void Start()
    {
        mapManager = DIContainer.GetModule<IMapManager>();
        pickLevel.onClick.RemoveAllListeners();
        pickLevel.onClick.AddListener(delegate
        {
            PickLevel();
        });
    }
    public override MapDataStat DATA
    {
        get => base.DATA;
        set
        {
            base.DATA = value;
            if (isOpen != null)
            {
                if (DATA.IS_OPEN)
                {
                    isOpen.gameObject.SetActive(false);
                }
                else isOpen.gameObject.SetActive(true);
            }
            if (TXT_VALUE != null)
            {
                if (DATA.ID == 0)
                {
                    TXT_VALUE.text = "Tut";
                }
                else
                    TXT_VALUE.text = (DATA.ID + 1).ToString();
            }
            if (IMG_BG != null)
            {
                //IMG_BG.sprite = SpriteDB.Instance.GetBackgroundInMap(DATA.IS_OPEN);
            }
            if (stars.Length > 0)
            {
                int i = 0;
                for (; i < stars.Length && i < DATA.STAR; i++)
                {
                    stars[i].gameObject.SetActive(true);
                }
                for (; i < stars.Length; i++)
                {
                    stars[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void SetupMapManager(IMapManager mapManager)
    {
        this.mapManager = mapManager;
    }
    public bool PickLevel()
    {
        if (DATA != null && DATA.IS_OPEN)
        {
            mapManager.CUR_MAP = DATA;
            SceneManager.LoadScene("GamePlay");
            return true;
        }
        return false;
    }
    private void OnValidate()
    {
        if (stars.Length == 0) stars = StarContainer.GetComponentsInChildren<Image>();
    }
    public void SetData(MapDataStat data)
    {
        DATA = data;
    }
}
