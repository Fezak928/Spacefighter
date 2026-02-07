using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 5f;

    [SerializeField] private GameObject _hpPickup, _nukePickup;
    [SerializeField] private GameObject[] _asteroids;
    [SerializeField] private Camera _pixelPerfectCamera;

    private BoxCollider2D _spawningRegion;
    private List<GameObject> _currentlySpawnableObjects = new();

    private float _spawnTimer;
    private PlayerController _player;

    private void Awake()
    {
        _spawningRegion = GetComponent<BoxCollider2D>();

        foreach(var asteroid in _asteroids)
        {
            _currentlySpawnableObjects.Add(asteroid);
        }
    }

    private void Start()
    {
        _player = GameManager.instance.Player;
    }

    private void Update()
    {
        Timer();
        UpdateSpawnables();

        if (_spawnTimer <= 0f)
            Spawn();
    }

    private void UpdateSpawnables()
    {
        if (CanSpawnHPPickups() && !_currentlySpawnableObjects.Contains(_hpPickup))
            _currentlySpawnableObjects.Add(_hpPickup);

        else if (!CanSpawnHPPickups())
            _currentlySpawnableObjects.Remove(_hpPickup);

        if (CanSpawnNukePickups() && !_currentlySpawnableObjects.Contains(_nukePickup))
            _currentlySpawnableObjects.Add(_nukePickup);

        else if (!CanSpawnNukePickups())
            _currentlySpawnableObjects.Remove(_nukePickup);
    }

    private bool CanSpawnHPPickups()
    {
        return _player.CurrentHitpoints != _player.PlayerData.HitPoints;
    }

    private bool CanSpawnNukePickups()
    {
        return !GameManager.instance.AreNukesMaxedOut();
    }

    private void Spawn()
    {
        GameObject objectToBeSpawned = PickObjectToSpawn();
        Vector3 spawningPosition = GetRandomStartingPosition(_spawningRegion.bounds);

        ObjectPoolingManager.SpawnObject(objectToBeSpawned, spawningPosition, Quaternion.identity);
        _spawnTimer = _spawnRate;
    }

    private Vector3 GetRandomStartingPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0);
    }

    private GameObject PickObjectToSpawn()
    {
        int selectedID = Random.Range(0, _currentlySpawnableObjects.Count);

        return _currentlySpawnableObjects[selectedID];
    }

    private void Timer()
    {
        if(_spawnTimer > 0f)
        {
            _spawnTimer -= Time.deltaTime;
        }
    }

}
