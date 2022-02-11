using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClass : MonoBehaviour {
    public Cell CellPrefab;
    public int Width;
    public int Height;
    public int Length;
    public float CellSize;
    public float DistanceBetweenCells;
    public Vector3 StartPosition;
    private Cell[,,] cells;

    public bool CenterGrid;
    
    private void Start()
    {
        CreateGrid();
    }

    private void FixedUpdate() {

    }
    
    public Vector3 GetCenterPosition()
    {
        return new Vector3(StartPosition.x + (Width * DistanceBetweenCells) / 2, StartPosition.y, StartPosition.z + (Length * DistanceBetweenCells) / 2);
    }
    
    //Create a grid of cells with given cellPrefab, Width, Height, Lenght, Cellsize, DistanceBetweenCells and StartPosition centered on the grid
    public void CreateGrid() {
        // StartPosition = GetCenterPosition();
        cells = new Cell[Width, Height, Length];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Length; z++)
                {
                    Cell cell = Instantiate(CellPrefab, new Vector3(StartPosition.x + x * DistanceBetweenCells, StartPosition.y + y * DistanceBetweenCells, StartPosition.z + z * DistanceBetweenCells), Quaternion.identity);
                    cell.transform.parent = transform;
                    cell.name = "Cell " + x + " " + y + " " + z;
                    cell.SetGrid(this);
                    cell.SetPosition(x, y, z);
                    cells[x, y, z] = cell;
                    cell.transform.localScale = cells[x, y, z].transform.localScale * CellSize;
                }
            }
        }
    }
    
    //Visualize the grid with given startPosition, Width, Height, Lenght, Cellsize, DistanceBetweenCells and StartPosition by using editor gizmos
    private void OnDrawGizmos()
    {
        if(Application.isPlaying) return;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Length; z++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(new Vector3(StartPosition.x + x * DistanceBetweenCells, StartPosition.y + y * DistanceBetweenCells, StartPosition.z + z * DistanceBetweenCells), new Vector3(CellSize, CellSize, CellSize));
                }
            }
        }
    }
    
    
    //Get cell at position x, y, z
    public Cell GetCell(int x, int y, int z)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height || z < 0 || z >= Length)
        {
            return null;
        }
        return cells[x, y, z];
    }
    
    //Get cell with vector3 
    public Cell GetCell(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / DistanceBetweenCells);
        int y = Mathf.FloorToInt(position.y / DistanceBetweenCells);
        int z = Mathf.FloorToInt(position.z / DistanceBetweenCells);
        return GetCell(x, y, z);
    }
}
