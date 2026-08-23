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

    [Space]
    [SerializeField]
    private Material attackRadiusMaterial;
    [SerializeField]
    private Material buildPreviewMaterial;

    public GridBuilder currentGrid;

    private BuildTileSlot selectedBuildTile;
    private bool bIsMouseOnUI;

    private void OnEnable()
    {
        GameServices.RegisterBuildManager(this);
    }

    private void OnDisable()
    {
        GameServices.UnregisterBuildManager(this);
    }

    private void Start()
    {
        MakeBuildTileAvaliablityFalse(currentGrid);
    }

    // Called by PlayerInputHandler when a world click doesn't land on a build tile.
    public void TryDeselectOnWorldClick()
    {
        if (bIsMouseOnUI)
        {
            return;
        }

        if (Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
        {
            if (!hit.collider.TryGetComponent<BuildTileSlot>(out _))
            {
                CancleBuildUnit();
            }
        }
    }

    public void CancleBuildUnit()
    {
        if (selectedBuildTile == null)
        {
            return;
        }

        uiCanvas.uiBuildBtns.GetLastSelectedButton()?.SelectButton(false);
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
        WaveData nextWave = GameServices.WaveManager.GetNextWaveData();
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

    public Material GetAttackRadiusMaterial() => attackRadiusMaterial;
    public Material GetBuildPreviewMaterial() => buildPreviewMaterial;

    public bool SetMouseOnUI(bool isMouseOnUI) => bIsMouseOnUI = isMouseOnUI;   
}
