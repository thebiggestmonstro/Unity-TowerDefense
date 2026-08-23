using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    [SerializeField]
    private GridBuilder currentGrid;
    [SerializeField]
    private BuildManager buildManager;
    [SerializeField]
    private EnemySpawner enemySpawner;

    public GridBuilder CurrentGrid => currentGrid;

    public void TransitionToWave(WaveData nextWave)
    {
        if (nextWave == null)
        {
            return;
        }

        UpdateLevelGrid(nextWave.currentWaveGrid);
        UpdateLevelPortals(nextWave.newPortals);
        currentGrid.GetNavMesh()?.BuildNavMesh();
        buildManager.MakeBuildTileAvaliablityFalse(currentGrid);
    }

    private void UpdateLevelGrid(GridBuilder nextGridPrefab)
    {
        if (nextGridPrefab == null)
        {
            return;
        }

        if (currentGrid != null)
        {
            Destroy(currentGrid.gameObject);
        }

        GridBuilder newGridInstance = Instantiate(nextGridPrefab, Vector3.zero, Quaternion.identity);
        currentGrid = newGridInstance;
        buildManager.currentGrid = currentGrid;
        buildManager.MakeBuildTileAvaliablityFalse(currentGrid);
    }

    private void UpdateLevelPortals(Enemy_Portal[] portalPrefabs)
    {
        if (portalPrefabs == null || portalPrefabs.Length <= 0)
        {
            return;
        }

        foreach (Enemy_Portal portalPrefab in portalPrefabs)
        {
            if (portalPrefab == null)
            {
                continue;
            }

            Enemy_Portal createdPortal = Instantiate(
                portalPrefab,
                portalPrefab.transform.position,
                portalPrefab.transform.rotation
            );

            enemySpawner.RegisterPortal(createdPortal);
        }
    }
}
