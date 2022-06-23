using UnityEngine;
using NativeWebSocket;

public class Globals : MonoBehaviour
{
    public static bool isPaused;
    public static bool isInMap;
    public static bool isHobod;
    public static bool isHosting = true;
    public static bool isDialoguing;

    public static WebSocket websocket;
}
