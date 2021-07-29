namespace PA.Data
{
    public enum Action
    {
        NONE,
        MOVE_LEFT,
        MOVE_FORWARD,
        MOVE_RIGHT,
        MOVE_BACKWARD,
        JUMP,
    }

    public enum Components
    {
        None = 0,
        Time = 1 << 0,
        Transform = 1 << 1,
        Physics = 1 << 2,
        Action = 1 << 3,
        Network = 1 << 4,
    }

    public enum NetworkEventType
    {
        NONE,
        CREATED,
        MODIFIED,
        REMOVED,
    }
}
