using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinWatcher : MonoBehaviour
{
  public GameObject inputObject;

  private GameObject websocketObject;
  private WebsocketWatcher websocketScript;

  private void Start()
  {
    websocketObject = GameObject.Find("WebsocketWatcher");
    websocketScript = websocketObject.GetComponent<WebsocketWatcher>();

    websocketScript.StartWebsocket();

    websocketScript.websocket.OnMessage += (bytes) =>
    {
      // Transform the JSON into a patch so we can validate the success
      string message = System.Text.Encoding.UTF8.GetString(bytes);
      Response patch = JsonConvert.DeserializeObject<Response>(message);

      if (patch.success)
      {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
    };
  }

  public void JoinGame()
  {
    TMP_InputField inputField = inputObject.GetComponent<TMP_InputField>();

    Action action = new Action();

    // Fetch the code from the input field and submit it too the server
    action.code = inputField.text;
    action.action = "subscribing";

    websocketScript.SendWebsocket(action);
  }
}
