using Margot;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public UIManager uiManager;
    public PoolManager poolManager;
    public WaveManager waveManager;
    public StatManager statManager;
    public ResolutionManager resolutionManager;
    public RouletteManager rouletteManager;
    public AudioManager audioManager;


    public bool isGameOver = false;
    public WorldShaking shaking;
    public int enemyKillCount = 0;

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
        if (uiManager.isCanvasOn)
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

    public void RestartGame()
    {
        rouletteManager.PlaySound(3);
        SceneManager.LoadScene(1);
    }
}
