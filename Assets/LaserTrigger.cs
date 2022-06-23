using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    public GameObject canClick;

    private bool isNear;

    public GameObject LaserUI;

    void Start() {
        LaserUI.SetActive(false);
    }

    public void TriggerLaserScreen()
    {
        if (isNear) {
            FindObjectOfType<BtnClickManager>().ResetLights();
            LaserUI.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player") 
        {
            canClick.SetActive(true);
            isNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            canClick.SetActive(false);
            isNear = false;
        }
    }
}
