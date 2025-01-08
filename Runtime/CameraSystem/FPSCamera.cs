using System.Runtime.CompilerServices;
using UnityEngine;

namespace LibFPS.CameraSystem
{
	public class FPSCamera : MonoBehaviour
	{
		public LayerMask FPSMode;
		public LayerMask TPSMode;
		public Camera Cam;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ToFPSMode()
		{
			Cam.cullingMask = FPSMode;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ToTPSMode()
		{
			Cam.cullingMask = TPSMode;
		}
	}
}
