using UnityEngine;

// Takes and handles input and movement for a player character
public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;

    public GameObject PauseUI;
    public GameObject PauseMain;
    public GameObject PauseOptions;
    public GameObject PauseBtn;
    public GameObject ClueNotifications;

    void Start()
    {
        Globals.isPaused = false;
    }

    //Pause menu is van https://www.youtube.com/watch?v=JivuXdrIHK0&t=431s

    public void Toggle()
    {
        Globals.isPaused = !Globals.isPaused;

        UpdateUi();
    }

    void UpdateUi()
    {
        PauseUI.SetActive(Globals.isPaused);
        PauseMain.SetActive(Globals.isPaused);
        PauseBtn.SetActive(!Globals.isPaused);
        PauseOptions.SetActive(false);
        ClueNotifications.SetActive(!Globals.isPaused);
    }
}
