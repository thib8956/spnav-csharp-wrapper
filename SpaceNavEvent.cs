using System;

namespace SpaceNavWrapper
{
    public class MotionEventArgs : EventArgs 
    {
        public readonly int X, Y, Z;
        public readonly int Rx, Ry, Rz;
        
        public MotionEventArgs(int x, int y, int z, int rx, int ry, int rz)
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

	public class ButtonEventArgs : EventArgs
    {
		public readonly int button;
		public readonly bool pressed;
        
        public override string ToString()
		{
			return string.Format("[ButtonEventArgs: button={0}, pressed={1}]", button, pressed);
		}
    }
}
