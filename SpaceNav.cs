using System;
using System.Runtime.InteropServices;

namespace SpaceNavWrapper
{
	public class SpaceNav : IDisposable
	{

		const string DLL_NAME = "spnavhdi";

		#region Constants
		private const int SPNAV_EVENT_MOTION = 1;
		private const int SPNAV_EVENT_BUTTON = 2;
		private const ushort SPNAV_VENDOR_ID = 0x046d;
		private const ushort SPNAV_PRODUCT_ID = 0x0c627;
		#endregion

		public event EventHandler<MotionEventArgs> Motion;
		public event EventHandler<ButtonEventArgs> Button;

		private double _sensitivity = 1.0;
		private int _threshold = 5;
		private bool isDisposed;

		#region Structures
		private struct SpNavEventMotion
		{
			public int type;
			public int x, y, z;
			public int rx, ry, rz;
			public uint period;
			public IntPtr data;
		}

		private struct SpNavEventButton
		{
			public int type;
            [MarshalAs(UnmanagedType.Bool)]
			public bool press;
			public int bnum;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct SpNavEvent
		{
			[FieldOffset(0)] public int type;
			[FieldOffset(0)] public SpNavEventMotion motion;
			[FieldOffset(0)] public SpNavEventButton button;
		}
		#endregion

		#region DLL Imports
		[DllImport(DLL_NAME)]
		private static extern int spnav_open(ushort vendor_id, ushort product_id);
		[DllImport(DLL_NAME)]
		private static extern int spnav_close();
		[DllImport(DLL_NAME)]
		private static extern int spnav_wait_event(ref SpNavEvent ev);
		[DllImport(DLL_NAME)]
		private static extern int spnav_wait_event_timeout(ref SpNavEvent ev, int timeout);
		[DllImport(DLL_NAME)]
		private static extern int spnav_sensitivity(double sens);
		[DllImport(DLL_NAME)]
		private static extern int spnav_deadzone(int threshold);
		#endregion

		public void InitDevice()
		{
			// TODO handle retcode and errors
			spnav_open(SPNAV_VENDOR_ID, SPNAV_PRODUCT_ID);
		}

		public void WaitEvent(int millis = -1)
		{
			SpNavEvent sev = new SpNavEvent();
			int ev_type;
			if (millis == -1)
			{
				ev_type = spnav_wait_event(ref sev);
			}
			else
			{
				ev_type = spnav_wait_event_timeout(ref sev, (int)millis);
			}

			switch (ev_type)
			{
				case SPNAV_EVENT_BUTTON:
					var e = sev.button;
					Button(this, new ButtonEventArgs(e.press, e.bnum));
					break;
				case SPNAV_EVENT_MOTION:
					var ev = sev.motion;
					Motion(this, new MotionEventArgs(ev.x, ev.y, ev.z, ev.rx, ev.ry, ev.rz));
					break;
				default:
					break;
			}
		}

		public double Sensitivity
		{
			get => _sensitivity;
			set
			{
				_sensitivity = value;
				spnav_sensitivity(value);
			}
		}

		public int Threshold
		{
			get => _threshold;
			set
			{
				_threshold = value;
				spnav_deadzone(value);
			}
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				CloseDevice();
				Button = null;
				Motion = null;
			}
			isDisposed = true;
		}

		private void CloseDevice()
		{
			// TODO : handle retcode and errors
			spnav_close();
		}
	}
}
