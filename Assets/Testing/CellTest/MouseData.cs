using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseData : MonoBehaviour
{
    
    public GridCreator gridCreator;
    
    Cell currentCell;

    public Vector3 MouseDataPosition;

    private Vector3 startPos;
    

    private void Update() {
        MouseDataPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseDataPosition.y = 0;
        if (Input.GetMouseButtonDown(0)) {
            currentCell = gridCreator.GetCell(MouseDataPosition);
            startPos = Input.mousePosition;
        }
        if(currentCell == null) return;
        if (Input.GetMouseButtonUp(0)) {
            if (!ColumnOrRow()) {
                if(startPos.y > Input.mousePosition.y) {
                    gridCreator.MoveCellMembersInRow(currentCell.row, 1);
                } else {
                    gridCreator.MoveCellMembersInRow(currentCell.row, -1);
                }
            }
            else {
                if(startPos.x > Input.mousePosition.x) {
                    gridCreator.MoveCellMembersInColumn(currentCell.column, 1);
                } else {
                    gridCreator.MoveCellMembersInColumn(currentCell.column, -1);
                }
            }
            currentCell = null;
        }
    }
    
    float MouseDelta() {
        return Vector3.Distance(startPos, Input.mousePosition);
    }

    bool ColumnOrRow() {
        float xDelta = Mathf.Abs(startPos.x - Input.mousePosition.x);
        float yDelta = Mathf.Abs(startPos.y - Input.mousePosition.y);
        
        return xDelta > yDelta;
    }

}
