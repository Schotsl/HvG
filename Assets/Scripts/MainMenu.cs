using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
