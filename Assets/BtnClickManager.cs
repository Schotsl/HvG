using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClickManager : MonoBehaviour
{
    public GameObject greenLight;
    // Start is called before the first frame update
    void Start()
    {
        greenLight.SetActive(false);
    }

    public void ResetLights() {
        greenLight.SetActive(false);
    }

    public void BtnClick() {
        StartCoroutine(GreenToggle());
    }
    IEnumerator GreenToggle() {
        greenLight.SetActive(true);
        yield return new WaitForSeconds(1);
        greenLight.SetActive(false);
    }
}
