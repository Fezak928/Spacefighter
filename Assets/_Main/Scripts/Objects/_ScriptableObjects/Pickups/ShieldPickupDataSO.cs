using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Pickup Data", menuName = "Object Datas/Pickups/Shield Pickup Data")]
public class ShieldPickupDataSO : CorePickupDataSO
{
    public override void OnReturnToPoolLogic(GameObject obj)
    {
        SpawnManager.Instance.ShieldPickupDespawned();
        base.OnReturnToPoolLogic(obj);
    }

    public override void OnTriggerEnterLogic(PlayerController player)
    {
        player.ShieldPickedUp();
    }
}
