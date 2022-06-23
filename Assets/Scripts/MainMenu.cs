using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        
        Application.Quit();
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(sceneName: "SceneStart");
    }
}
