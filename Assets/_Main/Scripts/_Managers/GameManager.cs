using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [field: SerializeField, Range(1,64)] public int PixelsPerUnit { get; private set; } = 16;
    [field: SerializeField] public Camera PixelPerfectCamera { get; private set; }
    [field: SerializeField] public PlayerController Player { get; private set; }

    [SerializeField]
    private GameObject[] _hpDisplay, _nukeDisplay;

    [SerializeField, Range(1,5)] private int _maximumNukeAmount = 5;
    public int CurrentNukeAmount { get; private set; }

    private int _currentScore;
    [SerializeField] TextMeshProUGUI _scoreDisplay;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        CurrentNukeAmount = _maximumNukeAmount;

        if (Player == null)
        {
            Player = FindFirstObjectByType<PlayerController>();
        }

        UpdateScore(0);
    }

    public void RemoveHPDisplayHeart(int currentHP)
    {
        RemoveFromDisplayArray(_hpDisplay, currentHP);
    }

    public void AddHPDisplayHeart(int currentHP)
    {
        AddToDisplayArray(_hpDisplay, currentHP);
    }

    public bool AreNukesMaxedOut()
    {
        return CurrentNukeAmount >= _maximumNukeAmount;
    }

    public void AddNuke()
    {
        if (!AreNukesMaxedOut())
        {
            CurrentNukeAmount++;
            
            AddToDisplayArray(_nukeDisplay, CurrentNukeAmount);
        }
    }

    public void UseNuke()
    {
        CurrentNukeAmount--;

        RemoveFromDisplayArray(_nukeDisplay, CurrentNukeAmount);
    }

    private void AddToDisplayArray(GameObject[] displayArray, int value)
    {
        for (int i = 0; i < displayArray.Length; i++)
        {
            if (i <= value - 1 && !displayArray[i].activeSelf)
                displayArray[i].SetActive(true);
        }
    }

    private void RemoveFromDisplayArray(GameObject[] displayArray, int value)
    {
        for (int i = 0; i < displayArray.Length; i++)
        {
            if (i > value - 1 && displayArray[i].activeSelf)
                displayArray[i].SetActive(false);
        }
    }

    public void UpdateScore(int scorepoints)
    {
        _currentScore += scorepoints;

        _scoreDisplay.text = $"Score: {_currentScore}";

    }
}
