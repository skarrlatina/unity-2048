using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement
{
    public bool MoveCells(Cell[,] cells, Vector2 direction)
    {
        bool anyCellMoved = false;
        int size = cells.GetLength(0);
        bool isHorizontal = direction.x != 0;
        int moveDirection = isHorizontal ? (int)direction.x : -(int)direction.y;
        bool moveRightOrDown = moveDirection > 0;
        int start = moveRightOrDown ? size - 1 : 0;

        for (int i = 0; i < size; i++)
        {
            for (int k = start; k >= 0 && k < size; k -= moveDirection)
            {
                var cell = isHorizontal ? cells[i, k] : cells[k, i];
                if (cell.IsEmpty) continue;

                var targetCell = FindTargetCell(cells, cell, direction);
                if (targetCell == null) continue;

                if (targetCell.Value == cell.Value && !targetCell.HasMerged)
                {
                    cell.MergeWithCell(targetCell);
                    anyCellMoved = true;
                }
                else
                {
                    cell.MoveToCell(targetCell);
                    anyCellMoved = true;
                }
            }
        }
        return anyCellMoved;
    }
    public bool HasAnyMove(Cell[,] cells)
    {
        int size = cells.GetLength(0);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var cell = cells[x, y];
                if (cell.IsEmpty ||
                    FindTargetCell(cells, cell, Vector2.left) ||
                    FindTargetCell(cells, cell, Vector2.right) ||
                    FindTargetCell(cells, cell, Vector2.up) ||
                    FindTargetCell(cells, cell, Vector2.down)
                    )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Cell FindTargetCell(Cell[,] cells, Cell cell, Vector2 direction)
    {
        int x = cell.X;
        int y = cell.Y;
        int size = cells.GetLength(0);
        Cell lastEmptyCell = null;

        while (true)
        {
            x += (int)direction.x;
            y -= (int)direction.y;

            if (x < 0 || x >= size || y < 0 || y >= size)
                break;

            var currentCell = cells[x, y];

            if (currentCell.IsEmpty)
            {
                lastEmptyCell = currentCell;
            }
            else
            {
                if (currentCell.Value == cell.Value && !currentCell.HasMerged)
                    return currentCell;
                break;
            }
        }
        return lastEmptyCell;
    }
}
