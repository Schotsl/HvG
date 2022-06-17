using TMPro;
using Newtonsoft.Json;
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

    if (websocketScript.websocket == null) {
      websocketScript.StartWebsocket();
    } else {
      websocketScript.ConnectWebsocket();
    }

    websocketScript.websocket.OnMessage += WebsocketResponse;
  }

  private void WebsocketResponse(byte[] bytes) {
    // Transform the JSON into a patch so we can validate the success
    string message = System.Text.Encoding.UTF8.GetString(bytes);
    Response patch = JsonConvert.DeserializeObject<Response>(message);

    if (patch.success)
    {
      // Once a partner has been found we can go to the next scene
      SceneManager.LoadScene(sceneName:"SceneGame");

      // Stop ourself from triggering this function again
      websocketScript.websocket.OnMessage -= WebsocketResponse;
    }
  }


  public void JoinGame()
  {
    TMP_InputField inputField = inputObject.GetComponent<TMP_InputField>();

    SubscribeUpdate action = new SubscribeUpdate();

    // Fetch the code from the input field and submit it too the server
    action.code = inputField.text;

    websocketScript.SendWebsocket(action);
  }
}
