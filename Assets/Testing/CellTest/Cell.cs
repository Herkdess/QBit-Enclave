using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cell : MonoBehaviour {
    public int row;
    public int column;

    public CellMember CellMember;
    
    public void InitilizeCell(int row, int column, CellMember memberPrefab)
    {
        this.row = row;
        this.column = column;
        CellMember mem = Instantiate(memberPrefab, transform);
        Vector3 memPos = Vector3.zero;
        memPos.y += .6f;
        mem.transform.localPosition = memPos;
        CellMember = mem;
        CellMember.InitMember(this);
    }

    public void SetCell(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void UpdateCell(int row, int column) {
        this.row = row;
        this.column = column;
    }
    
    public void HighlightCell()
    {
        transform.DOScale(1.2f, 0.5f);
    }
    
    public void MoveCellX(int amount) {
        Vector3 newPosition = transform.position;
        newPosition.x += amount;
        transform.DOMove(newPosition, .5f);
    }
    
    public void MoveCellY(int amount) {
        Vector3 newPosition = transform.position;
        newPosition.y += amount;
        transform.DOMove(newPosition, .5f);
    }
    
    public void MoveCellZ(int amount) {
        Vector3 newPosition = transform.position;
        newPosition.z += amount;
        transform.DOMove(newPosition, .5f);
    }
    
    public void UpdateCellColumn(int amount) {
        column = amount;
    }
    
    public void UpdateCellRow(int amount) {
        row = amount;
    }
    
    public void UpdateCellMember(CellMember cellMember) {
        CellMember = cellMember;
        cellMember.UpdateCellMember(this);
    }
}
