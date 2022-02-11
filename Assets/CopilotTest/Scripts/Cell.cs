using UnityEngine;
public class Cell : MonoBehaviour {
    
    public int x;
    public int y;
    public int z;
    public int index;
    
    GridClass grid;
    
    
    public void SetIndex(int index)
    {
        this.index = index;
    }
    
    public void SetPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public void SetPosition(Vector3 position)
    {
        this.x = (int)position.x;
        this.y = (int)position.y;
        this.z = (int)position.z;
    }
    
    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
    
    public int GetIndex()
    {
        return index;
    }
    
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
    
    public void SetColor(float r, float g, float b)
    {
        GetComponent<Renderer>().material.color = new Color(r, g, b);
    }
    
    public void SetColor(float r, float g, float b, float a)
    {
        GetComponent<Renderer>().material.color = new Color(r, g, b, a);
    }
    
    public void SetGrid(GridClass grid)
    {
        this.grid = grid;
    }

}