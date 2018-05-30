using System;

namespace SpaceNavWrapper
{
	class Program
	{
		static void Main(string[] args)
		{
			//spnavwrapper.SpaceNav.Instance.Threshold = 5;
			//spnavwrapper.SpaceNav.Instance.Sensitivity = 0.1;

			//for (; ; )
			//{
			//	var ev = spnavwrapper.SpaceNav.Instance.WaitEvent(100);
			//	Console.WriteLine(ev);
			//}
			SpaceNav navDriver = new SpaceNav();
			navDriver.InitDevice();
			navDriver.Button += OnButton;
			navDriver.Motion += OnMotion;
            
            Console.CancelKeyPress += delegate {
				navDriver.Dispose();
            };
			for (; ; ) 
            {
                navDriver.WaitEvent();
            }
		}

		private static void OnMotion(object sender, MotionEventArgs e)
		{
			Console.WriteLine(e);
		}

		private static void OnButton(object sender, EventArgs e)
		{
			Console.WriteLine(e);
		}
	}
}
