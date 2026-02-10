using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [field: SerializeField, Range(1,64)] public int PixelsPerUnit { get; private set; } = 16;
    [field: SerializeField] public Camera PixelPerfectCamera { get; private set; }

    [SerializeField] private GameObject[] _hpDisplay, _shieldDisplay, _nukeDisplay;

    [field: SerializeField, Range(1,5)] public int MaximumNukeAmount { get; private set; } = 5;
    public int CurrentNukeAmount { get; private set; }

    [SerializeField] private int _passiveTimeToGain1Score = 10;
    public int CurrentScore { get; private set; }
    [SerializeField] TextMeshProUGUI _scoreDisplay;

    public float RunPlayTime { get; private set; } = 0f;
    private float _passiveScoreGainTimer;

    [SerializeField] private GameObject _gameOverMenu;

    #region Initialization

    private void OnEnable()
    {
        PlayerController.TookDamageEvent += RemoveHPDisplayHeart;
        PlayerController.HealedEvent += AddHPDisplayHeart;
        PlayerController.UsedNukeEvent += UseNuke;
        PlayerController.PickedUpNukeEvent += AddNuke;
        PlayerController.PickedUpShieldEvent += FillShieldDisplay;
        PlayerController.ShieldTookDamageEvent += RemoveShield;

        PlayerController.PlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        PlayerController.TookDamageEvent -= RemoveHPDisplayHeart;
        PlayerController.HealedEvent -= AddHPDisplayHeart;
        PlayerController.UsedNukeEvent -= UseNuke;
        PlayerController.PickedUpNukeEvent -= AddNuke;
        PlayerController.PickedUpShieldEvent -= FillShieldDisplay;
        PlayerController.ShieldTookDamageEvent -= RemoveShield;

        PlayerController.PlayerDead -= OnPlayerDead;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
            
        else
            Instance = this;

        Time.timeScale = 1f;
        CurrentScore = 0;

        _gameOverMenu.SetActive(false);
        UpdateScore(0);
    }

    #endregion

    private void Update()
    {
        RunPlayTime += Time.deltaTime;
        _passiveScoreGainTimer += Time.deltaTime;

        if (_passiveScoreGainTimer >= _passiveTimeToGain1Score)
        {
            _passiveScoreGainTimer = 0.0f;
            UpdateScore(1);
        }
    }

    #region HUD Display

    public void FillShieldDisplay()
    {
        AddToDisplayArray(_shieldDisplay, _shieldDisplay.Length);
    }

    public void RemoveShield(int shieldHP)
    {
        RemoveFromDisplayArray(_shieldDisplay, shieldHP);
    }

    public void RemoveHPDisplayHeart(int currentHP, int damage)
    {
        RemoveFromDisplayArray(_hpDisplay, currentHP);
    }

    public void AddHPDisplayHeart(int currentHP, int heal)
    {
        AddToDisplayArray(_hpDisplay, currentHP);
    }

    public bool AreNukesMaxedOut()
    {
        return CurrentNukeAmount >= MaximumNukeAmount;
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
        CurrentScore += scorepoints;

        _scoreDisplay.text = $"Score: {CurrentScore}";
    }



    #endregion

    #region Game Over

    public void OnPlayerDead()
    {
        Time.timeScale = 0f;
        _gameOverMenu.SetActive(true);
    }

    #endregion
}
