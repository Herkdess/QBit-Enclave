using System;
using UnityEngine;
namespace CopilotTest.Scripts {
    public class GridController : MonoBehaviour {
        public GridClass grid;

        private void Update() {

            //Rotate on both axis the grid with mouse position on screen 
            grid.transform.rotation = Quaternion.Euler(Input.mousePosition.y * 0.1f, 0, -Input.mousePosition.x * 0.1f);
        }
    }
}