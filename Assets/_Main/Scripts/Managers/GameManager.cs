using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [field: SerializeField, Range(1,64)] public int PixelsPerUnit { get; private set; } = 16;
    [field: SerializeField] public Camera PixelPerfectCamera { get; private set; }

    [SerializeField]
    private GameObject[] _hpDisplayHearts;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public void RemoveHPDisplayHeart(int currentHP)
    {
        for (int i = 0; i < _hpDisplayHearts.Length; i++)
        {
            if (i > currentHP - 1 && _hpDisplayHearts[i].activeSelf)
                _hpDisplayHearts[i].SetActive(false);
        }
    }

    public void AddHPDisplayHeart(int currentHP)
    {
        for (int i = 0; i < _hpDisplayHearts.Length; i++)
        {
            if(i <= currentHP - 1 && !_hpDisplayHearts[i].activeSelf)
                _hpDisplayHearts[i].SetActive(true);
        }
    }
}
