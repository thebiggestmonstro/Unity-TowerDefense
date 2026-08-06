using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitUnlockData
{
    public string unitName;
    public bool bUnlocked;

    public UnitUnlockData(string unitName, bool bUnlocked)
    {
        this.unitName = unitName;
        this.bUnlocked = bUnlocked;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private UI_Canvas uiCanvas;

    public List<UnitUnlockData> unlockedUnits;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void  Start()
    {
        UnlockUnit();
    }

    [ContextMenu("Init Unit Data")]
    private void InitializeUnitData()
    {
        unlockedUnits.Clear();
        unlockedUnits.Add(new UnitUnlockData("Crossbow", false));
        unlockedUnits.Add(new UnitUnlockData("Cannon", false));
        unlockedUnits.Add(new UnitUnlockData("Rapid Fire Gun", false));
        unlockedUnits.Add(new UnitUnlockData("Spider Nest", false));
        unlockedUnits.Add(new UnitUnlockData("Anti-air Harpon", false));
        unlockedUnits.Add(new UnitUnlockData("Blocking Fan", false));
        unlockedUnits.Add(new UnitUnlockData("Hammer", false));
    }

    private void UnlockUnit()
    {
        foreach (var unitData in unlockedUnits)
        {
            foreach (var buildBtn in uiCanvas.uiBuildBtns.GetBuildButtons())
            {
                buildBtn.UnlockUnit(unitData.unitName, unitData.bUnlocked);
            }
        }
    }
}
