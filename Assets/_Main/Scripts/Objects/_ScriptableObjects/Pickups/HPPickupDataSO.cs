using UnityEngine;

[CreateAssetMenu(fileName = "New HP Pickup Data", menuName = "Object Datas/Pickups/HP Pickup Data")]
public class HPPickupDataSO : CorePickupDataSO
{
    [Range(1,5)] public int HealPoints = 1;

    public override void OnTriggerEnterLogic(PlayerController player)
    {
        player.Heal(HealPoints);
    }

    public override void OnReturnToPoolLogic(GameObject obj)
    {
        SpawnManager.Instance.HPPickupDespawned();
        base.OnReturnToPoolLogic(obj);
    }
}
