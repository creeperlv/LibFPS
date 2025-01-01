using LibFPS.Kernel.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class WeightedRotationSync : MonoBehaviour
	{
		public Transform Source;
		public Transform Target;
		public Vector3 Weight;
		public float fWeight;
		public Vector3 Debug0;
		public Vector3 Debug1;
		public Vector3 Debug2;
		public void Update()
		{
			Vector3 srcRot = Source.localEulerAngles.ToZeroCenteredRotation();
			Debug0 = srcRot;
			Vector3 calced = new Vector3(srcRot.x * Weight.x, srcRot.y * Weight.y, srcRot.z * Weight.z);
			Debug1 = calced;
			Target.localEulerAngles = calced;
			Debug2 = Target.localEulerAngles;
		}
	}
}
