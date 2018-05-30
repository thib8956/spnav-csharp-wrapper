namespace spnavwrapper
{
    public abstract class SpaceNavEvent
    {
    }

    public class SpaceNavButtonEvent : SpaceNavEvent
    {
        public bool Pressed { get; }
        public int Button { get; }

        public SpaceNavButtonEvent(bool pressed, int button)
        {
            Pressed = pressed;
            Button = button;
        }

        public override string ToString()
        {
            return "pressed=" + Pressed + " button=" + Button;
        }
    }

    public class SpaceNavMotionEvent : SpaceNavEvent
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int Rx { get; }
        public int Ry { get; }
        public int Rz { get; }

        public SpaceNavMotionEvent(int x, int y, int z, int rx, int ry, int rz)
        {
            X = x;
            Y = y;
            Z = z;
            Rx = rx;
            Ry = ry;
            Rz = rz;
        }

        public override string ToString()
        {
            return "x=" + X + " y=" + Y + " z=" + Z +
                " rx=" + Rz + " ry=" + Ry + " rz=" + Rz;
        }
    }
}
