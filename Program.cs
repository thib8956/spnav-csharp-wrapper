using System;
using spnavwrapper;

namespace spnavcsharpwrapper
{
	class Program
	{
		static void Main(string[] args)
		{
			SpaceNav.Instance.Threshold = 5;
			SpaceNav.Instance.Sensitivity = 0.1;

			for (; ; )
			{
				var ev = SpaceNav.Instance.WaitEvent(100);
				Console.WriteLine(ev);
			}
		}
	}
}
