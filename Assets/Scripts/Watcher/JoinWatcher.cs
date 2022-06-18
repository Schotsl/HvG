using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinWatcher : MonoBehaviour
{
    public GameObject inputObject;

    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private void Start()
    {
        Globals.isHosting = false;

        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        if (websocketScript.websocket == null)
        {
            websocketScript.StartWebsocket();
        }
        else
        {
            websocketScript.ConnectWebsocket();
        }

        websocketScript.AddSubscribe(SubscribeResponse);
    }

    private void SubscribeResponse(bool success)
    {
        // Once a partner has been found we can go to the next scene
        SceneManager.LoadScene(sceneName: "SceneGame");

        // Remove the listener since we won't be needing it
        websocketScript.RemoveSubscribe(SubscribeResponse);
    }

    public void JoinGame()
    {
        TMP_InputField inputField = inputObject.GetComponent<TMP_InputField>();

        string code = inputField.text;

        // Fetch the code from the input field and submit it too the server
        SubscribeUpdate action = new SubscribeUpdate(code);
        websocketScript.SendWebsocket(action);
    }
}
