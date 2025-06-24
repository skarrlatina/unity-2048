using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private GridConfig _config;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private RectTransform _gridRectTransform;

    private GridGenerator _gridGenerator;
    private GridMovement _gridMovement;
    private Cell[,] _cells;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        InitializeComponents();
    }

    private void Start()
    {
        SwipeInput.OnSwipe += OnInput;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W)) OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) OnInput(Vector2.down);
    }

    private void InitializeComponents()
    {
        _gridGenerator = new GridGenerator(_cellPrefab, _gridRectTransform, _config);
        _gridMovement = new GridMovement();
    }

    public void BuildGrid()
    {
        if(_cells == null)
            _cells = _gridGenerator.GenerateGrid();

        ClearGrid();

        SpawnRandomCells();
        SpawnRandomCells();
    }

    public bool HasCellWithMaxValue()
    {
        foreach (var cell in _cells)
            if (cell.Value == Cell.MaxValue)
                return true;

        return false;
    }

    public bool HasAvailableMove() => _gridMovement.HasAnyMove(_cells);

    private void ClearGrid()
    {
        foreach (var cell in _cells)
            cell.SetValue(cell.X, cell.Y, 0);
    }

    private void SpawnRandomCells()
    {
        var emptyCells = _gridGenerator.GetEmptyCells(_cells);
        if (emptyCells.Count == 0) return;

        var randomCell = emptyCells[Random.Range(0, emptyCells.Count)];

        int value = Random.Range(0, 10) == 0 ? 2 : 1; // (10% for 2);
        randomCell.SetValue(randomCell.X, randomCell.Y, value);
    }

    private void ResetMergeFlags()
    {
        int size = _cells.GetLength(0);
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                _cells[x, y].HasMerged = false;
    }

    // [!!!] Move to input controller [!!!]
    private void OnInput(Vector2 direction)
    {
        if (!GameController.GameStarted)
            return;

        ResetMergeFlags();
        if (_gridMovement.MoveCells(_cells, direction))
        {
            SpawnRandomCells();
            GameController.Instance.CheckGameState();
        }
    }
    // [-]====================================[-]
}