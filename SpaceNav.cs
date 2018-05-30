using System;
using System.Runtime.InteropServices;

namespace spnavwrapper
{
	public sealed class SpaceNav : IDisposable
	{
		double _sensitivity = 1.0;
		int _threshold = 5;

		static SpaceNav instance;

		const int SPNAV_EVENT_MOTION = 1;
		const int SPNAV_EVENT_BUTTON = 2;
		const ushort SPNAV_VENDOR_ID = 0x046d;
		const ushort SPNAV_PRODUCT_ID = 0x0c627;

		const string DLL_NAME = "spnavhdi";

		struct SpNavEventMotion
		{
			public int type;
			public int x, y, z;
			public int rx, ry, rz;
			public uint period;
			public IntPtr data;
		}

		struct SpNavEventButton
		{
			public int type;
			public int press;
			public int bnum;
		}

		[StructLayout(LayoutKind.Explicit)]
		struct SpNavEvent
		{
			[FieldOffset(0)] public int type;
			[FieldOffset(0)] public SpNavEventMotion motion;
			[FieldOffset(0)] public SpNavEventButton button;
		}

		[DllImport(DLL_NAME)]
		static extern int spnav_open(ushort vendor_id, ushort product_id);
		[DllImport(DLL_NAME)]
		static extern int spnav_close();
		[DllImport(DLL_NAME)]
		static extern int spnav_wait_event(ref SpNavEvent ev);
		[DllImport(DLL_NAME)]
		static extern int spnav_wait_event_timeout(ref SpNavEvent ev, int timeout);
		[DllImport(DLL_NAME)]
		static extern int spnav_sensitivity(double sens);
		[DllImport(DLL_NAME)]
		static extern int spnav_deadzone(int threshold);

		SpaceNav()
		{
			// TODO : handle retcode
			spnav_open(SPNAV_VENDOR_ID, SPNAV_PRODUCT_ID);
		}

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

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		void Dispose(bool disposing)
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
	}
}
