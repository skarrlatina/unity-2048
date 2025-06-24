using System.Collections.Generic;
using UnityEngine;

public class GridGenerator
{
    private Cell _cellPrefab;
    private RectTransform _transform;
    private GridConfig _config;
    private MonoBehaviour _context;

    public GridGenerator(Cell cellPrefab, RectTransform transform, GridConfig config)
    {
        _cellPrefab = cellPrefab;
        _transform = transform;
        _config = config;
    }

    public Cell[,] GenerateGrid()
    {
        var cells = new Cell[_config.GridSize, _config.GridSize];

        float totalGridWidth = CalcGridWidth();
        _transform.sizeDelta = new Vector2(totalGridWidth, totalGridWidth);

        float startPosX = -1 * (totalGridWidth - _config.CellSize) / 2 + _config.CellSpacing;
        float startPosY = (totalGridWidth - _config.CellSize) / 2 - _config.CellSpacing;

        for (int x = 0; x < _config.GridSize; x++)
        {
            for (int y = 0; y < _config.GridSize; y++)
            {
                var cell = Object.Instantiate(_cellPrefab, _transform, false);
                var pos = new Vector2(
                    //startX + (y * (CellSize + CellSpacing)),
                    //startY - (x * (CellSize + CellSpacing))
                    startPosX + (x * (_config.CellSize + _config.CellSpacing)),
                    startPosY - (y * (_config.CellSize + _config.CellSpacing))
                    );
             
                cell.transform.localPosition = pos;

                cells[x, y] = cell;
                cell.SetValue(x, y, 0);
            }
        }

        return cells;
    }

    public List<Cell> GetEmptyCells(Cell[,] cells)
    {
        var emptyCells = new List<Cell>();
        int size = cells.GetLength(0);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                if (cells[i, j].IsEmpty)
                    emptyCells.Add(cells[i, j]);

        return emptyCells;
    }

    private float CalcGridWidth() 
    {
        return _config.GridSize * (_config.CellSize + _config.CellSpacing) + _config.CellSpacing; //
    }
}
