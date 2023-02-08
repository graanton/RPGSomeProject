using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloadble : MonoBehaviour
{
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
