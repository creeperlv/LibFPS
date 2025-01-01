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
		public void Update()
		{
			Vector3 srcRot = Source.localEulerAngles.ToZeroCenteredRotation();
			Vector3 calced = new Vector3(srcRot.x * Weight.x, srcRot.y * Weight.y, srcRot.z * Weight.z);
			Target.localEulerAngles = calced;
		}
	}
}
