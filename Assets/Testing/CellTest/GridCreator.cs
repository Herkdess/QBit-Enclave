using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {
    public Cell CellPrefab;
    public Transform SpawnPoint;
    public Vector2Int GridSize;
    public float CellSize;
    public float CellGap;
    public Vector3 StartPosition;

    public Cell[,] Grid;

    private void Start() {
        CreateGrid();
    }
    
    public void CreateGrid() {
        Grid = new Cell[GridSize.x, GridSize.y];
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                Cell cell = Instantiate(CellPrefab, GetPosition(new Vector2Int(i, j)), Quaternion.identity, SpawnPoint);
                cell.transform.localScale = new Vector3(CellSize, CellSize, CellSize);
                Grid[i, j] = cell;
            }
        }
        CenterGrid();
    }

    public Vector3 GetPosition(Vector2Int pos)
    {
        return new Vector3(StartPosition.x + pos.x * (CellSize + CellGap), StartPosition.y, StartPosition.z + pos.y * (CellSize + CellGap));
    }
    
    Vector3[,] CellPositions() {
        
        Vector3[,] positions = new Vector3[GridSize.x, GridSize.y];
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                positions[x, y] = GetPosition(new Vector2Int(x, y));
            }
        }
        return positions;
    }
    
    public Cell GetCell(Vector3 pos)
    {
        Vector2Int cellPos = GetCellPosition(pos);
        if (cellPos.x < 0 || cellPos.x >= GridSize.x || cellPos.y < 0 || cellPos.y >= GridSize.y)
            return null;
        return Grid[cellPos.x, cellPos.y];
    }
    
    public Vector2Int GetCellPosition(Vector3 pos)
    {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt((pos.x - StartPosition.x) / (CellSize + CellGap)), Mathf.FloorToInt((pos.z - StartPosition.z) / (CellSize + CellGap)));
        return cellPos;
    }
    
    //Return the closest cell position, with the given error margin
    public Vector2Int GetCellPosition(Vector3 pos, float errorMargin)
    {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt((pos.x - StartPosition.x) / (CellSize + CellGap)), Mathf.FloorToInt((pos.z - StartPosition.z) / (CellSize + CellGap)));
        if (cellPos.x < 0 || cellPos.x >= GridSize.x || cellPos.y < 0 || cellPos.y >= GridSize.y)
            return new Vector2Int(-1, -1);
        return cellPos;
    }
    
    //Get grid center
    public Vector3 GetCenter()
    {
        return new Vector3(StartPosition.x + (GridSize.x * (CellSize + CellGap)) / 2, StartPosition.y, StartPosition.z + (GridSize.y * (CellSize + CellGap)) / 2);
    }
    
    //Offset spawn point position by the grid center
    public void CenterGrid()
    {
        var pos = SpawnPoint.position;
        pos -= GetCenter() * .5f;
        SpawnPoint.position = pos;
    }


    #region Editor

    
    private void OnDrawGizmos() {
        if(Application.isPlaying)
            return;
        if(GridSize.x <= 0 || GridSize.y <= 0)
            return;
        
        // Vector3[,] positions = CellPositions();
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Gizmos.DrawWireCube(GetPosition(new Vector2Int(x,y)), new Vector3(CellSize, CellSize, CellSize));
            }
        }
    }

    #endregion


}
