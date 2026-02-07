using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 5f;
    [SerializeField] private GameObject _asteroid;

    [SerializeField] private Camera _pixelPerfectCamera;

    private float _spawnTimer;

    private void Update()
    {
        Timer();

        if (_spawnTimer <= 0f)
            Spawn();
    }

    private void Spawn()
    {
        ObjectPoolingManager.SpawnObject(_asteroid, this.transform.position, Quaternion.identity);
        _spawnTimer = _spawnRate;
    }

    private void Timer()
    {
        if(_spawnTimer > 0f)
        {
            _spawnTimer -= Time.deltaTime;
        }
    }

}
