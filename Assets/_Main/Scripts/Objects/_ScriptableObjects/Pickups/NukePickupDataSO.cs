using UnityEngine;

[CreateAssetMenu(fileName = "New Nuke Pickup Data", menuName = "Object Datas/Pickups/Nuke Pickup Data")]
public class NukePickupDataSO : CorePickupDataSO
{
    public override void OnTriggerEnterLogic(PlayerController player)
    {
        GameManager.instance.AddNuke();
    }
}
