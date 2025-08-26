using Margot;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager uiManager;
    public PoolManager poolManager;

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
