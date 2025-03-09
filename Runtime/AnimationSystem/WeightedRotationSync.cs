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
		public Vector3 Offsets;
		public Vector3 SupposedAngle;
		public Vector3 ActualAngle;
		public float fWeight;
		public bool IsGlobalRot;
		public void LateUpdate()
		{
			if (IsGlobalRot)
			{
				Vector3 srcRot = Source.eulerAngles.ToZeroCenteredRotation();
				Vector3 calced = new Vector3(srcRot.x * Weight.x + Offsets.x, srcRot.y * Weight.y + Offsets.y, srcRot.z * Weight.z + Offsets.z);
				Target.eulerAngles = calced;
				SupposedAngle=calced;
				ActualAngle=Target.eulerAngles;
			}
			else
			{
				Vector3 srcRot = Source.localEulerAngles.ToZeroCenteredRotation();
				Vector3 calced = new Vector3(srcRot.x * Weight.x + Offsets.x, srcRot.y * Weight.y + Offsets.y, srcRot.z * Weight.z + Offsets.z);
				Target.localEulerAngles = calced;
			}
		}
	}
}
