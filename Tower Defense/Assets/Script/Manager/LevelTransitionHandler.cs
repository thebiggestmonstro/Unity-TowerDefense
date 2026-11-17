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

        GridBuilder newGridInstance = Instantiate(nextGridPrefab, Vector3.zero, Quaternion.identity);

        if (currentGrid != null)
        {
            var oldTiles = currentGrid.GetCreatedTiles();
            var newTiles = newGridInstance.GetCreatedTiles();

            for (int i = 0; i < oldTiles.Count; i++)
            {
                BuildTileSlot oldSlot = oldTiles[i].GetComponent<BuildTileSlot>();
                BuildTileSlot newSlot = newTiles[i].GetComponent<BuildTileSlot>();

                if (oldSlot != null && newSlot != null && !oldSlot.IsBuildTileAvailable())
                {
                    newSlot.SetBuildTileAvailability(false);
                }
            }

            Destroy(currentGrid.gameObject);
        }

        currentGrid = newGridInstance;
        buildManager.currentGrid = currentGrid;
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
