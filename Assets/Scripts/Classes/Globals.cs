using UnityEngine;
using NativeWebSocket;

public class Globals : MonoBehaviour
{
    public static bool isPaused;
    public static bool isHosting = true;
    public static bool isDialoguing;

    public static WebSocket websocket;

    public static bool triggering;
}
