using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostWatcher : MonoBehaviour
{
    private TextMeshProUGUI hostShort;

    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private void Start()
    {
        Globals.isHosting = true;

        string code = GenerateCode(5);

        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        // Generate a short code and show it too the user
        hostShort = GameObject.Find("HostText").GetComponent<TextMeshProUGUI>();
        hostShort.text = code;

        if (websocketScript.websocket == null)
        {
            websocketScript.StartWebsocket();
        }
        else
        {
            websocketScript.ConnectWebsocket();
        }

        websocketScript.AddHosting(HostingResponse);

        HostingUpdate action = new HostingUpdate(code);
        websocketScript.SendWebsocket(action);
    }

    private void HostingResponse(bool success)
    {
        // Once a partner has been found we can go to the next scene
        SceneManager.LoadScene(sceneName: "SceneGame");

        // Remove the listener since we won't be needing it
        websocketScript.RemoveHosting(HostingResponse);
    }

    private string GenerateCode(int length)
    {
        // I've removed characters that are hard to distinguish from each other
        string randomCharacters = "abcdefghjkmnopqrstuvwxyz023456789";
        string randomString = "";

        for (int i = 0; i < length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, randomCharacters.Length);
            char randomCharacter = randomCharacters[randomIndex];

            randomString += randomCharacter;
        }

        return randomString;
    }
}
