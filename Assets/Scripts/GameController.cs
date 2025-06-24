using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static int Points { get; set; }
    public static bool GameStarted { get; set; }

    [SerializeField] private TextMeshProUGUI _gameResult;
    [SerializeField] private TextMeshProUGUI _pointsText;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _gameResult.text = "";

        SetPoints(0);
        GameStarted = true;

        GridManager.Instance.BuildGrid();
    }

    public void CheckGameState() 
    {
        if (GridManager.Instance.HasCellWithMaxValue())
        {
            _gameResult.text = "You win!";
            EndGame();
        }

        if (!GridManager.Instance.HasAvailableMove())
        {
            _gameResult.text = "You lose!";
            EndGame();
        }
    }

    public void EndGame()
    {
        GameStarted = false;
    }

    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        _pointsText.text = Points.ToString();
    }
}
