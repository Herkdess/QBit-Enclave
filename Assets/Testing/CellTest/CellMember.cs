using DG.Tweening;
using TMPro;
using UnityEngine;
public class CellMember : MonoBehaviour {
    public Cell cell;

    public string Value;
    
    public void InitMember(Cell parent) {
        this.cell = parent;
        Value = GetRandomChar().ToString();
        transform.GetChild(0).GetComponent<TextMeshPro>().text = Value;
    }
    
    public void SetCell(Cell cell) {
        this.cell = cell;
    }

    public void UpdateCellMember(Cell parent) {
        this.cell = parent;
        transform.SetParent(parent.transform);
        Vector3 movePos = Vector3.zero;
        movePos.y += .6f;
        transform.DOLocalMove(movePos, .5f);
    }
    
    //Return a random char
    public char GetRandomChar() {
        int rand = Random.Range(0, 26);
        return (char)('a' + rand);
    }
}