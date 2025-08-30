using Margot;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public UIManager uiManager;
    public PoolManager poolManager;
    public WaveManager waveManager;
    public StatManager statManager;
    public ResolutionManager resolutionManager;

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

    void Update()
    {
        if (uiManager.rouletteCanvas.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
