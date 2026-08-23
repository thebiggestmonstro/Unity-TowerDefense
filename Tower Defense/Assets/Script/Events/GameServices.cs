
public static class GameServices
{
    public static BuildManager BuildManager { get; private set; }
    public static WaveManager WaveManager { get; private set; }
    public static TileAnimator TileAnimator { get; private set; }
    public static EnemySpawner EnemySpawner { get; private set; }
    public static GridVisibilityController GridVisibilityController { get; private set; }

    public static void RegisterBuildManager(BuildManager manager) => BuildManager = manager;

    public static void UnregisterBuildManager(BuildManager manager)
    {
        if (BuildManager == manager)
        {
            BuildManager = null;
        }
    }

    public static void RegisterWaveManager(WaveManager manager) => WaveManager = manager;

    public static void UnregisterWaveManager(WaveManager manager)
    {
        if (WaveManager == manager)
        {
            WaveManager = null;
        }
    }

    public static void RegisterTileAnimator(TileAnimator animator) => TileAnimator = animator;

    public static void UnregisterTileAnimator(TileAnimator animator)
    {
        if (TileAnimator == animator)
        {
            TileAnimator = null;
        }
    }

    public static void RegisterEnemySpawner(EnemySpawner spawner) => EnemySpawner = spawner;

    public static void UnregisterEnemySpawner(EnemySpawner spawner)
    {
        if (EnemySpawner == spawner)
        {
            EnemySpawner = null;
        }
    }

    public static void RegisterGridVisibilityController(GridVisibilityController controller) => GridVisibilityController = controller;

    public static void UnregisterGridVisibilityController(GridVisibilityController controller)
    {
        if (GridVisibilityController == controller)
        {
            GridVisibilityController = null;
        }
    }
}
