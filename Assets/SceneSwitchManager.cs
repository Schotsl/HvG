using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goBack(){
        SceneManager.LoadScene(sceneName:"SceneGame");
    }
    public void goToMap(){
        SceneManager.LoadScene(sceneName:"Cluemap");
    }
}
