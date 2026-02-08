using UnityEngine;

public abstract class CorePickupDataSO : CoreObjectDataSO
{
    public virtual void OnTriggerEnterLogic(PlayerController player)
    {

    }

    public virtual void OnReturnToPoolLogic(GameObject obj)
    {
        ObjectPoolingManager.ReturnObjectToPool(obj);
    }
}
