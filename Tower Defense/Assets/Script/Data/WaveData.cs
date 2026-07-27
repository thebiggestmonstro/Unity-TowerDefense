using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnInfo
{
    public GameObject enemyPrefab; 
    public int spawnCount;         
}

[CreateAssetMenu(fileName = "EnmeyWaveData", menuName = "Wave System/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Configuration")]
    public List<EnemySpawnInfo> waveInfo;

    [Header("Grid Configuration")]
    public GridBuilder currentWaveGrid;
    public Enemy_Portal[] newPortals;
}
