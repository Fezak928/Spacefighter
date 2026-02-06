using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [field: SerializeField, Range(1,64)] public int PixelsPerUnit { get; private set; } = 16;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
