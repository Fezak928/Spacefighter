using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioClip[] _sfxClips;
    public void OnSelect(BaseEventData eventData)
    {
        AudioManagerSO.PlaySFX(_sfxClips, transform.position, 1f);
    }
}
