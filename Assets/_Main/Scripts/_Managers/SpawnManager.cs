using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private float _timeToFastestSpawnRate = 5f;
    private float _currentSpawnRate;
    [SerializeField] private AnimationCurve _spawnCurve;

    [SerializeField, Range(0f, 100f)] private float _pickupSpawnChance = 13;
    [SerializeField] private GameObject _hpPickup, _nukePickup, _shieldPickup;
    [SerializeField] private GameObject[] _asteroids;
    [SerializeField] private Camera _pixelPerfectCamera;

    private BoxCollider2D _spawningRegion;
    private List<GameObject> _currentlySpawnablePickups = new();

    private float _spawnTimer;

    private int _amountOfSpawnableNukes, _amountOfSpawnableHPPickups, _amountOfSpawnableShields;
    private int _currentlySpawnedNukes, _currentlySpawnedHPPickups, _currentlySpawnedShields;

    private bool _isPlayerDead;

    #region Initialization
    private void OnEnable()
    {
        PlayerController.PickedUpNukeEvent += OnPickedUpNuke;
        PlayerController.UsedNukeEvent += OnUsedNuke;
        PlayerController.HealedEvent += OnHeal;
        PlayerController.TookDamageEvent += OnTakenDamage;

        PlayerController.PickedUpShieldEvent += OnPickedUpShield;
        PlayerController.ShieldDestroyedEvent += OnDestroyedShield;

        PlayerController.PlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        PlayerController.PickedUpNukeEvent -= OnPickedUpNuke;
        PlayerController.UsedNukeEvent -= OnUsedNuke;
        PlayerController.HealedEvent -= OnHeal;
        PlayerController.TookDamageEvent -= OnTakenDamage;

        PlayerController.PickedUpShieldEvent -= OnPickedUpShield;
        PlayerController.ShieldDestroyedEvent -= OnDestroyedShield;

        PlayerController.PlayerDead -= OnPlayerDead;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;
    }

    #endregion

    private void Start()
    {
        _spawningRegion = GetComponent<BoxCollider2D>();
        _amountOfSpawnableNukes = GameManager.Instance.MaximumNukeAmount;
        _amountOfSpawnableShields = 1;

        _isPlayerDead = false;
    }

    private void Update()
    {
        Timer();
        UpdateSpawnables();

        _currentSpawnRate = _spawnCurve.Evaluate(GameManager.Instance.RunPlayTime/_timeToFastestSpawnRate);

        if (_spawnTimer <= 0f && !_isPlayerDead)
            Spawn();
    }

    #region Spawning Variable Functions

    private void OnPickedUpShield()
    {
        _amountOfSpawnableShields--;
    }

    private void OnDestroyedShield()
    {
        _amountOfSpawnableShields++;
    }

    public void ShieldPickupDespawned()
    {
        _currentlySpawnedShields--;
    }

    private void OnUsedNuke()
    {
        _amountOfSpawnableNukes++;
    }

    private void OnPickedUpNuke()
    {
        _amountOfSpawnableNukes--;
    }

    public void NukePickupDespawned()
    {
        _currentlySpawnedNukes--;
    }

    private void OnHeal(int currentHP, int heal)
    {
        _amountOfSpawnableHPPickups -= heal;
    }

    public void HPPickupDespawned()
    {
        _currentlySpawnedHPPickups--;
    }

    private void OnTakenDamage(int currentHP, int damage)
    {
        _amountOfSpawnableHPPickups += damage;
    }

    private void UpdateSpawnables()
    {
        if (CanSpawnHPPickups() && !_currentlySpawnablePickups.Contains(_hpPickup))
            _currentlySpawnablePickups.Add(_hpPickup);

        else if (!CanSpawnHPPickups())
            _currentlySpawnablePickups.Remove(_hpPickup);

        if (CanSpawnNukePickups() && !_currentlySpawnablePickups.Contains(_nukePickup))
            _currentlySpawnablePickups.Add(_nukePickup);

        else if (!CanSpawnNukePickups())
            _currentlySpawnablePickups.Remove(_nukePickup);

        if (CanSpawnShields() && !_currentlySpawnablePickups.Contains(_shieldPickup))
            _currentlySpawnablePickups.Add(_shieldPickup);
        else if (!CanSpawnShields())
            _currentlySpawnablePickups.Remove(_shieldPickup);
    }

    private bool CanSpawnHPPickups()
    {
        if (_currentlySpawnedHPPickups < _amountOfSpawnableHPPickups)
            return true;

        return false;
    }

    private bool CanSpawnNukePickups()
    {
        if (_currentlySpawnedNukes < _amountOfSpawnableNukes)
            return true;

        return false;
    }

    private bool CanSpawnShields()
    {
        if(_currentlySpawnedShields < _amountOfSpawnableShields)
            return true;

        return false;
    }
    #endregion

    #region Spawning logic

    private void Spawn()
    {
        GameObject objectToBeSpawned = PickObjectToSpawn();
        Vector3 spawningPosition = GetRandomStartingPosition(_spawningRegion.bounds);

        ObjectPoolingManager.SpawnObject(objectToBeSpawned, spawningPosition, Quaternion.identity);
        _spawnTimer = _currentSpawnRate;
    }

    private Vector3 GetRandomStartingPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0);
    }

    private GameObject PickObjectToSpawn()
    {
        float chanceValue = Random.Range(0f, 100f);
        bool pickup = false;
        List<GameObject> spawnableObjectsList = new List<GameObject>();

        foreach(var asteroid in _asteroids)
        {
            spawnableObjectsList.Add(asteroid);
        }

        if(chanceValue <= _pickupSpawnChance && _currentlySpawnablePickups.Count != 0)
        {
            spawnableObjectsList = _currentlySpawnablePickups;
            pickup = true;
        }

        int selectedID = Random.Range(0, spawnableObjectsList.Count);

        if (pickup)
        {
            if (_currentlySpawnablePickups[selectedID] == _nukePickup)
                _currentlySpawnedNukes++;

            if (_currentlySpawnablePickups[selectedID] == _hpPickup)
                _currentlySpawnedHPPickups++;

            if (_currentlySpawnablePickups[selectedID] == _shieldPickup)
                _currentlySpawnedShields++;
        }

        return spawnableObjectsList[selectedID];
    }

    #endregion

    private void Timer()
    {
        if(_spawnTimer > 0f)
            _spawnTimer -= Time.deltaTime;
    }

    private void OnPlayerDead()
    {
        _isPlayerDead = true;
    }

}
