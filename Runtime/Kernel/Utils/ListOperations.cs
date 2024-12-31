using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibFPS.Kernel.Utils
{
    public static class ListOperations
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ObtainOne<T>(List<T> list)
		{
			return list[RandomTool.NextInt(0, list.Count)];
		}
	}
}
