using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] CellColors = new Color[12];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        for (int i = 0; i < CellColors.Length; i++)
            CellColors[i].a = 1f;
    }
}
