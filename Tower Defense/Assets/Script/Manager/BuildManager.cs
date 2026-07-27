using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private UI_Canvas uiCanvas;
    [SerializeField]
    private Camera mainCamera;

    public GridBuilder currentGrid;

    public static BuildManager Instance { get; private set; }
    private BuildTileSlot selectedBuildTile;

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

    private void Start()
    {
        MakeBuildTileAvaliablityFalse(currentGrid);
    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            CancleBuildUnit();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
            {
                if (!hit.collider.TryGetComponent<BuildTileSlot>(out _))
                {
                    CancleBuildUnit();
                }
            }
        }
    }

    public void CancleBuildUnit()
    {
        if (selectedBuildTile == null)
        { 
            return; 
        }
        
        selectedBuildTile.UnSelectTile();
        selectedBuildTile = null;
        DisableBuildMenu();
    }

    public void SetSelectedBuildTile(BuildTileSlot newSlot)
    {
        if (selectedBuildTile)
        {
            selectedBuildTile.UnSelectTile();
        }

        selectedBuildTile = newSlot;
    }

    public void EnableBuildMenu()
    {
        if (selectedBuildTile == null)
        {
            return;
        }

        uiCanvas.uiBuildBtns.ShowBuildButtons(true);
    }

    private void DisableBuildMenu()
    {
        uiCanvas.uiBuildBtns.ShowBuildButtons(false);
    }

    public BuildTileSlot GetSelectedBuildTile() => selectedBuildTile;

    public void MakeBuildTileAvaliablityFalse(GridBuilder currentGrid)
    {
        WaveData nextWave = WaveManager.Instance.GetNextWaveData();
        if (nextWave == null || nextWave.currentWaveGrid == null)
        {
            return;
        }

        List<GameObject> grid = currentGrid.GetCreatedTiles();
        List<GameObject> nextGrid = nextWave.currentWaveGrid.GetCreatedTiles();
        if (grid == null || nextGrid == null)
        {
            return;
        }

        for (int i = 0; i < grid.Count; i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot nextTile = nextGrid[i].GetComponent<TileSlot>();

            bool tileNotSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                               currentTile.GetMaterial() != nextTile.GetMaterial() ||
                               currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;

            if (tileNotSame == false)
            {
                continue;
            }

            BuildTileSlot buildTileSlot = grid[i].GetComponent<BuildTileSlot>();

            if (buildTileSlot != null)
            {
                buildTileSlot.SetBuildTileAvailability(false);
            }
        }
    }
}
