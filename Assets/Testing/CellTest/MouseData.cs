using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseData : MonoBehaviour
{
    
    public GridCreator gridCreator;
    
    Cell currentCell;

    public Vector3 MouseDataPosition;
    

    private void Update() {
        MouseDataPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseDataPosition.y = 0;
        if (Input.GetMouseButtonDown(0)) {
            currentCell = gridCreator.GetCell(MouseDataPosition);
        }
        if (Input.GetMouseButton(0)) {
            if (currentCell != null) {
                currentCell.transform.position = MouseDataPosition;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            currentCell = null;
        }
    }

    // Vector3 GetMouseWorldPosition(Vector3 pos) {
    //     Plane p =  new Plane(Vector3.up, )
    // }
}
