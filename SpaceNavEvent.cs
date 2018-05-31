using System;
using System.Collections.Generic;

namespace SpaceNavWrapper
{
    public enum SpaceNavAxis {
        X, Y, Z, Rx, Ry, Rz
    }

    public class MotionEventArgs : EventArgs 
    {
		public readonly Dictionary<SpaceNavAxis, int> axisValues;
                
        public MotionEventArgs(int x, int y, int z, int rx, int ry, int rz)
		{
            axisValues = new Dictionary<SpaceNavAxis, int>();
            axisValues[SpaceNavAxis.X] = x;
			axisValues[SpaceNavAxis.Y] = y;
			axisValues[SpaceNavAxis.Z] = y;
			axisValues[SpaceNavAxis.Rx] = rx;
			axisValues[SpaceNavAxis.Ry] = ry;
			axisValues[SpaceNavAxis.Rz] = rz;
		}

        public int X
        {
            get
            {
                return axisValues[SpaceNavAxis.X];
            }
        }

        public int Y
        {
            get
            {
                return axisValues[SpaceNavAxis.Y];
            }
        }

        public int Z
        {
            get
            {
                return axisValues[SpaceNavAxis.Z];
            }
        }

        public int Rx
        {
            get
            {
                return axisValues[SpaceNavAxis.Rx];
            }
        }

        public int Ry
        {
            get
            {
                return axisValues[SpaceNavAxis.Ry];
            }
        }

        public int Rz
        {
            get
            {
                return axisValues[SpaceNavAxis.Rz];
            }
        }

        public override string ToString()
		{
			return string.Format("x={0} y={1} z={2} rx={3} ry={4} rz={5}", X, Y, Z, Rx, Ry, Rz);
        }        

		public int GetAxis(SpaceNavAxis axis)
        {
			return axisValues[axis];
        }
    }

	public class ButtonEventArgs : EventArgs
	{
		public readonly bool Pressed;
		public readonly int Button;
        
		public ButtonEventArgs(bool pressed, int button)
		{
			Pressed = pressed;
			Button = button;
		}
        
		public override string ToString()
		{
			return string.Format("button={0}, pressed={1}", Button, Pressed);
		}
	}
}
