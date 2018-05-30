using System;
using System.Runtime.InteropServices;

namespace spnavwrapper
{
	public sealed class SpaceNav : IDisposable
	{
		const string DLL_NAME = "spnavhdi";

		#region Constants
		private const int SPNAV_EVENT_MOTION = 1;
		private const int SPNAV_EVENT_BUTTON = 2;
		private const ushort SPNAV_VENDOR_ID = 0x046d;
		private const ushort SPNAV_PRODUCT_ID = 0x0c627;
		#endregion

		private double _sensitivity = 1.0;
		private int _threshold = 5;
		private static SpaceNav instance;

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
			public int press;
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

		private SpaceNav()
		{
			// TODO : handle retcode
			spnav_open(SPNAV_VENDOR_ID, SPNAV_PRODUCT_ID);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				// Free unmanaged resources
				spnav_close();
				disposedValue = true;
			}
		}

		~SpaceNav()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Methods
		public static SpaceNav Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new SpaceNav();
				}
				return instance;
			}
		}

		public void CloseDevice()
		{
			spnav_close();
			instance = null;
		}

		public SpaceNavEvent WaitEvent()
		{
			SpNavEvent sev = new SpNavEvent();
			spnav_wait_event(ref sev);
			switch (sev.type)
			{
				case SPNAV_EVENT_BUTTON:
					return new SpaceNavButtonEvent(Convert.ToBoolean(sev.button.press), sev.button.bnum);
				case SPNAV_EVENT_MOTION:
					var ev = sev.motion;
					return new SpaceNavMotionEvent(ev.x, ev.y, ev.z, ev.rx, ev.ry, ev.rz);
				default:
					return null;
			}
		}

		public SpaceNavEvent WaitEvent(int milliseconds)
		{
			SpNavEvent sev = new SpNavEvent();
			int ev_type = spnav_wait_event_timeout(ref sev, milliseconds);
			switch (ev_type)
			{
				case SPNAV_EVENT_BUTTON:
					return new SpaceNavButtonEvent(Convert.ToBoolean(sev.button.press), sev.button.bnum);
				case SPNAV_EVENT_MOTION:
					var ev = sev.motion;
					return new SpaceNavMotionEvent(ev.x, ev.y, ev.z, ev.rx, ev.ry, ev.rz);
				default:
					return null;
			}
		}

		public double Sensitivity
		{
			get
			{
				return _sensitivity;
			}

			set
			{
				// TODO : handle retcode
				spnav_sensitivity(value);
				_sensitivity = value;
			}
		}

		public int Threshold
		{
			get
			{
				return _threshold;
			}
			set
			{
				// TODO : handle retcode
				spnav_deadzone(value);
				_threshold = value;
			}
		}
		#endregion
	}
}
