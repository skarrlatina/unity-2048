using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _valueText;

    public int X { get; set; }
    public int Y { get; set; }
    public int Value { get; set; }
    public bool IsEmpty => Value == 0;
    public bool HasMerged { get; set; }

    public const int MaxValue = 11;

    public void SetValue(int x, int y, int value)
    { 
        this.X = x;
        this.Y = y;
        this.Value = value;
        //HasMerged = false;
        UpdateCellVisuals();
    }

    public int GetPoints()
    { 
        return IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    }

    public void IncreaseValue()
    {
        if (Value >= MaxValue) return;

        Value++;
        HasMerged = true;
        GameController.Instance.AddPoints(GetPoints());
        UpdateCellVisuals();
    }

    public void MergeWithCell(Cell targetCell)
    {
        targetCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    public void MoveToCell(Cell targetCell)
    {
        targetCell.SetValue(targetCell.X, targetCell.Y, Value);
        SetValue(this.X, this.Y, 0);
    }
    private void UpdateCellVisuals()
    {
        _valueText.text = IsEmpty ? string.Empty : GetPoints().ToString();
        _image.color = ColorManager.Instance.CellColors[Value];
    }
}
