using UnityEngine;

public enum Type
{
    Clue = 0,
    Laser = 1,
    Hosting = 2,
    Position = 3,
    Subscribe = 4,
    Restoring = 5,
    Health = 6,
}

public class Update
{
    public Type type;
}

public class ClueUpdate : Update
{
    public string target;

    public ClueUpdate(string target)
    {
        this.type = Type.Clue;
        this.target = target;
    }
}

public class LaserUpdate : Update
{
    public bool triggered;

    public string target;

    public LaserUpdate(string target, bool triggered)
    {
        this.type = Type.Laser;
        this.target = target;
        this.triggered = triggered;
    }
}

public class HealthUpdate : Update
{
    public int health;

    public HealthUpdate(int health)
    {
        this.type = Type.Health;
        this.health = health;
    }
}

public class HostingUpdate : Update
{
    public bool success;

    public string code;

    public HostingUpdate(string code)
    {
        this.code = code;
        this.type = Type.Hosting;
    }
}

public class PositionUpdate : Update
{
    public string target;
    public float x;
    public float y;

    public PositionUpdate(string target, float x, float y)
    {
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

    public SubscribeUpdate(string code)
    {
        this.code = code;
        this.type = Type.Subscribe;
    }
}

public delegate void ClueCallback(string clue);

public delegate void LaserCallback(string laser, bool triggered);

public delegate void HealthCallback(int health);

public delegate void HostingCallback(bool success);

public delegate void PositionCallback(float x, float y);

public delegate void SubscribeCallback(bool success);

public class ClueListener
{
    public string target;

    public ClueCallback callback;

    public ClueListener(ClueCallback callback)
    {
        this.callback = callback;
    }
}

public class LaserListener
{
    public LaserCallback callback;

    public LaserListener(LaserCallback callback)
    {
        this.callback = callback;
    }
}

public class HealthListener
{
    public HealthCallback callback;

    public HealthListener(HealthCallback callback)
    {
        this.callback = callback;
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
