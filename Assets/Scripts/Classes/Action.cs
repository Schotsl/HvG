using UnityEngine;

public enum Type
{
    Clue = 0,
    Laser = 1,
    Hosting = 2,
    Position = 3,
    Subscribe = 4,
}

public class Update {
    public Type type;
}

public class ClueUpdate : Update
{
    public string target;

    public ClueUpdate() {
        type = Type.Clue;
    }
}

public class LaserUpdate : Update
{
    public bool triggered;

    public string target;

    public LaserUpdate() {
        type = Type.Laser;
    }
}

public class HostingUpdate : Update
{
    public bool success;

    public string code;

    public HostingUpdate() {
        type = Type.Hosting;
    }
}

public class PositionUpdate : Update
{
    public string target;
    public float x;
    public float y;

    public PositionUpdate(string target, float x, float y) {
        this.type = Type.Position;
        this.target = target;
        
        this.x = Mathf.Round(x * 100f) / 100f;
        this.y = Mathf.Round(y * 100f) / 100f;
    }
}

public class SubscribeUpdate : Update
{
    public bool success;

    public string code;

    public SubscribeUpdate() {
        type = Type.Subscribe;
    }
}

public delegate void ClueCallback();

public delegate void LaserCallback(bool triggered);

public delegate void HostingCallback(bool success);

public delegate void PositionCallback(float x, float y);

public delegate void SubscribeCallback(bool success);

public class ClueListener
{
    public string target;

    public ClueCallback callback;

    public ClueListener(ClueCallback callback, string target)
    {
        this.callback = callback;
        this.target = target;
    }
}

public class LaserListener
{
    public string target;

    public LaserCallback callback;

    public LaserListener(LaserCallback callback, string target)
    {
        this.callback = callback;
        this.target = target;
    }
}

public class HostingListener
{
    public HostingCallback callback;

    public HostingListener(HostingCallback callback)
    {
        this.callback = callback;
    }
}

public class PositionListener
{
    public string target;

    public PositionCallback callback;

    public PositionListener(PositionCallback callback, string target)
    {
        this.callback = callback;
        this.target = target;
    }
}

public class SubscribeListener
{
    public SubscribeCallback callback;

    public SubscribeListener(SubscribeCallback callback)
    {
        this.callback = callback;
    }
}