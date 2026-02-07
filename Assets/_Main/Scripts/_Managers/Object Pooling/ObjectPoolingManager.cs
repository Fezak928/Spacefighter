using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] private bool _dontDestroyOnLoad = false;

    private GameObject _emptyHolder;

    private static GameObject _objectEmpty;
    private static GameObject _projectilesEmpty;
    private static GameObject _sfxEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType { GameObjects, Projectiles, SFXs}

    public static PoolType PoolingType;

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _objectEmpty = new GameObject("GameObjects");
        _objectEmpty.transform.SetParent(_emptyHolder.transform);

        _projectilesEmpty = new GameObject("Projectiles");
        _projectilesEmpty.transform.SetParent(_emptyHolder.transform);

        _sfxEmpty = new GameObject("SFXs");
        _sfxEmpty.transform.SetParent(_emptyHolder.transform);

        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(_sfxEmpty.transform.root);
        }
    }

    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new( 
            createFunc: () => CreateObject(prefab, position, rotation, poolType), 
            actionOnGet: OnGetObject,   
            actionOnRelease: OnReleaseObject, 
            actionOnDestroy: OnDestroyObject
            );

        _objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, position, rotation);

        prefab.SetActive(true);

        GameObject parentObject = GetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static GameObject GetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return _objectEmpty;

            case PoolType.Projectiles:
                return _projectilesEmpty;

            case PoolType.SFXs:
                return _sfxEmpty;

            default:
                return null;
        }
    }

    private static void OnGetObject(GameObject obj)
    {

    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPosition, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
                _cloneToPrefabMap.Add(obj, objectToSpawn);

            obj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
                return obj as T;

            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesn't have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPosition, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPosition, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = GetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
                obj.transform.SetParent(parentObject.transform);

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
                pool.Release(obj);
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not pooled: " + obj.name);
        }
    }
}   