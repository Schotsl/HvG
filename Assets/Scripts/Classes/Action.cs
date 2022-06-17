enum Type
{
    Clue = 0,
    Laser = 1,
    Hosting = 2,
    Position = 3,
    Subscribe = 4,
}

public delegate void PositionCallback(float x, float y);

public class PositionListener
{
    public string target;
    public PositionCallback callback;

    public PositionListener(string target, PositionCallback callback)
    {
        this.target = target;
        this.callback = callback;
    }
}

class Update {
    public Type type;
}

class ClueUpdate : Update
{
    public int number;

    public ClueUpdate() {
        type = Type.Clue;
    }
}

class LaserUpdate : Update
{
    public int number;

    public LaserUpdate() {
        type = Type.Laser;
    }
}

class HostingUpdate : Update
{
    public string code;

    public HostingUpdate() {
        type = Type.Hosting;
    }
}

class PositionUpdate : Update
{
    public string target;
    public float x;
    public float y;

    public PositionUpdate(string target, float x, float y) {
        this.type = Type.Position;
        this.target = target;
        
        this.x = x;
        this.y = y;
    }
}

class SubscribeUpdate : Update
{
    public string code;

    public SubscribeUpdate() {
        type = Type.Subscribe;
    }
}
