using System.Runtime.CompilerServices;
using UnityEngine;

namespace LibFPS.Kernel.Utils
{
	public static class Vector3Tool
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToZeroCenteredRotation(this Vector3 v)
		{
			Vector3 vector3;
			vector3.x = v.x > 180 ? v.x - 360 : v.x;
			vector3.y = v.y > 180 ? v.y - 360 : v.y;
			vector3.z = v.z > 180 ? v.z - 360 : v.z;
			return vector3;
		}
	}
}
