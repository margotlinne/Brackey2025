using UnityEngine;
using UnityEngine.SceneManagement;

namespace Margot
{
    public class StartGame : MonoBehaviour
    {
        
        public void StartGameButton()
        {
            SceneManager.LoadScene(1);
        }
    }

}
