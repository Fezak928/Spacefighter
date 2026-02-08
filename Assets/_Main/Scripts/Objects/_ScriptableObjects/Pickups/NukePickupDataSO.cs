using UnityEngine;

[CreateAssetMenu(fileName = "New Nuke Pickup Data", menuName = "Object Datas/Pickups/Nuke Pickup Data")]
public class NukePickupDataSO : CorePickupDataSO
{
    public override void OnTriggerEnterLogic(PlayerController player)
    {
        player.PickedUpNuke();
    }

    public override void OnReturnToPoolLogic(GameObject obj)
    {
        SpawnManager.Instance.NukePickupDespawned();
        base.OnReturnToPoolLogic(obj);
    }
}
