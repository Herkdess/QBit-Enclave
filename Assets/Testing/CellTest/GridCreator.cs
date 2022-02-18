using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCreator : MonoBehaviour {
    public Cell CellPrefab;
    public CellMember CellMemberPrefab;
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
        for (int i = 0; i < GridSize.x; i++) {
            for (int j = 0; j < GridSize.y; j++) {
                Cell cell = Instantiate(CellPrefab, GetPosition(new Vector2Int(i, j)), Quaternion.identity, SpawnPoint);
                cell.transform.localScale = new Vector3(CellSize, CellSize, CellSize);
                cell.InitilizeCell(i, j, CellMemberPrefab);
                Grid[i, j] = cell;
            }
        }
    }

    public Cell[] GetCellRow(int row) {
        Cell[] columnCells = new Cell[GridSize.y];
        for (int i = 0; i < GridSize.y; i++) {
            columnCells[i] = Grid[row, i];
        }
        return columnCells;
    }

    public Cell[] GetCellColumn(int column) {
        Cell[] rowCells = new Cell[GridSize.x];
        for (int i = 0; i < GridSize.x; i++) {
            rowCells[i] = Grid[i, column];
        }
        return rowCells;
    }

    public Cell CellGetFirstCellInColumn(int column) {
        return Grid[column, 0];
    }

    public Cell CellGetFirstCellInRow(int row) {
        return Grid[0, row];
    }

    public Cell CellGetLastCellInColumn(int column) {
        return Grid[column, GridSize.y - 1];
    }

    public Cell CellGetLastCellInRow(int row) {
        return Grid[GridSize.x - 1, row];
    }

    public Cell CellGetCell(int column, int row) {
        return Grid[column, row];
    }

    public void MoveCellColumn(int column, int amount) {
        Cell[] columnCells = GetCellColumn(column);
        for (int i = 0; i < columnCells.Length; i++) {
            columnCells[i].MoveCellX(amount);
        }
    }

    public void MoveCellRow(int row, int amount) {
        Cell[] rowCells = GetCellRow(row);
        for (int i = 0; i < rowCells.Length; i++) {
            rowCells[i].MoveCellZ(amount);
        }
    }

    public void MoveCellMembersInRow(int row, int amount) {
        Cell[] rowCells = GetCellRow(row);
        CellMember[] rowCellMembers = new CellMember[rowCells.Length];
        for (int i = 0; i < rowCells.Length; i++) {
            rowCellMembers[i] = rowCells[i].CellMember;
        }
        if (amount > 0) {
            for (int i = 0; i < rowCells.Length - 1; i++) {
                rowCells[i].UpdateCellMember(rowCellMembers[i + 1]);
            }
            rowCells.Last().UpdateCellMember(rowCellMembers.First());
        }
        else {
            for (int i = 1; i < rowCells.Length; i++) {
                rowCells[i].UpdateCellMember(rowCellMembers[i - 1]);
            }
            rowCells.First().UpdateCellMember(rowCellMembers.Last());
        }
        Debug.Log(GetMemberValuesFromRow(row));
    }

    public string GetMemberValuesFromRow(int row) {
        Cell[] rowCells = GetCellRow(row);
        CellMember[] rowCellMembers = new CellMember[rowCells.Length];
        for (int i = 0; i < rowCells.Length; i++) {
            rowCellMembers[i] = rowCells[i].CellMember;
        }
        string memberValues = "";
        for (int i = 0; i < rowCellMembers.Length; i++) {
            memberValues += rowCellMembers[i].Value;
        }
        return memberValues;  }

    public void MoveCellMembersInColumn(int column, int amount) {
        Cell[] columnCells = GetCellColumn(column);
        CellMember[] columnCellMembers = new CellMember[columnCells.Length];
        for (int i = 0; i < columnCells.Length; i++) {
            columnCellMembers[i] = columnCells[i].CellMember;
        }
        if (amount > 0) {
            for (int i = 0; i < columnCells.Length - 1; i++) {
                columnCells[i].UpdateCellMember(columnCellMembers[i + 1]);
            }
            columnCells.Last().UpdateCellMember(columnCellMembers.First());
        }
        else {
            for (int i = 1; i < columnCells.Length; i++) {
                columnCells[i].UpdateCellMember(columnCellMembers[i - 1]);
            }
            columnCells.First().UpdateCellMember(columnCellMembers.Last());
        }
        Debug.Log(GetMemberValuesFromColumn(column));
    }

    public string GetMemberValuesFromColumn(int column) {
        Cell[] columnCells = GetCellColumn(column);
        CellMember[] columnCellMembers = new CellMember[columnCells.Length];
        for (int i = 0; i < columnCells.Length; i++) {
            columnCellMembers[i] = columnCells[i].CellMember;
        }
        string memberValues = "";
        for (int i = 0; i < columnCellMembers.Length; i++) {
            memberValues += columnCellMembers[i].Value;
        }
        return memberValues;
    }

    public Vector3 GetPosition(Vector2Int pos) {
        return new Vector3(StartPosition.x + pos.x * (CellSize + CellGap), StartPosition.y, StartPosition.z + pos.y * (CellSize + CellGap));
    }

    public Cell GetCell(Vector3 pos) {
        Vector2Int cellPos = GetCellPosition(pos);
        if (cellPos.x < 0 || cellPos.x >= GridSize.x || cellPos.y < 0 || cellPos.y >= GridSize.y)
            return null;
        return Grid[cellPos.x, cellPos.y];
    }

    public Vector2Int GetCellPosition(Vector3 pos) {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt((pos.x - StartPosition.x) / (CellSize + CellGap)), Mathf.FloorToInt((pos.z - StartPosition.z) / (CellSize + CellGap)));
        return cellPos;
    }

    public Vector2Int GetCellPosition(Vector3 pos, float errorMargin) {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt((pos.x - StartPosition.x) / (CellSize + CellGap)), Mathf.FloorToInt((pos.z - StartPosition.z) / (CellSize + CellGap)));
        if (cellPos.x < 0 || cellPos.x >= GridSize.x || cellPos.y < 0 || cellPos.y >= GridSize.y)
            return new Vector2Int(-1, -1);
        return cellPos;
    }


    #region Editor

    private void OnDrawGizmos() {
        if (Application.isPlaying)
            return;
        if (GridSize.x <= 0 || GridSize.y <= 0)
            return;

        // Vector3[,] positions = CellPositions();
        for (int x = 0; x < GridSize.x; x++) {
            for (int y = 0; y < GridSize.y; y++) {
                Gizmos.DrawWireCube(GetPosition(new Vector2Int(x, y)), new Vector3(CellSize, CellSize, CellSize));
            }
        }
    }

    #endregion


}