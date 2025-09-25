using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    public static SceneControllerScript instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void BackToStart()
    {
        SceneManager.LoadSceneAsync(0);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
