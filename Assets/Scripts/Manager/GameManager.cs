using Margot;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public UIManager uiManager;
    public PoolManager poolManager;
    public WaveManager waveManager;

    void Awake()
    {
        #region singleton
        if (Instance == null) { Instance = this; }
        else { Destroy(this.gameObject); }
        #endregion

        // hide mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
