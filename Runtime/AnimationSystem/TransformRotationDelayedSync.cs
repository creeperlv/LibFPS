using LibFPS.Kernel.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class TransformRotationDelayedSync : MonoBehaviour
	{
		public Transform Source;
		public Transform Target;
		public float Tolerance = 15;
		public float Speed = 10;
		public bool WillTriggerAnimation;
		public Animator TargetAnimator;
		public List<string> PositiveAnimationTrigger;
		public List<string> NegativeAnimationTrigger;
		public bool IsLocalSpace = true;
		public bool IsMoving;
		public bool IsActiveMoving;
		public float Termination = 2;
		public bool PauseAutoDSync = false;
		public void Sync()
		{

			Quaternion Src;
			Quaternion Tgt;
			if (IsLocalSpace)
			{
				Src = Source.localRotation;
				Tgt = Target.localRotation;
				if (Src != Tgt) Target.localRotation = Src;
			}
			else
			{

				Src = Source.rotation;
				Tgt = Target.rotation;
				if (Src != Tgt) Target.rotation = Src;
			}
		}
		public void Update()
		{
			if (PauseAutoDSync) return;
			float angle;
			Quaternion Src;
			Quaternion Tgt;
			if (IsLocalSpace)
			{
				Src = Source.localRotation;
				Tgt = Target.localRotation;
			}
			else
			{

				Src = Source.rotation;
				Tgt = Target.rotation;
			}
			angle = Vector3.SignedAngle(Target.forward, Source.forward, Vector3.up);
			float absAngle = Mathf.Abs(angle);
			if (IsActiveMoving)
			{
				if (IsLocalSpace)
				{
					Target.localRotation = Src;
				}
				else Target.rotation = Src;
				return;
			}
			if (IsMoving)
			{
				if (absAngle <= Termination)
				{
					IsMoving = false;
					return;
				}
				var f_q = Quaternion.RotateTowards(Tgt, Src, absAngle * Speed * Time.deltaTime);
				if (IsLocalSpace)
				{
					Target.localRotation = f_q;
				}
				else Target.rotation = f_q;
			}
			else
			{
				if (absAngle >= Tolerance)
				{
					IsMoving = true;
					if (WillTriggerAnimation)
					{
						if (TargetAnimator != null)
						{
							if (angle > 0)
								TargetAnimator.SetTrigger(PositiveAnimationTrigger.PickOne());
							else
								TargetAnimator.SetTrigger(NegativeAnimationTrigger.PickOne());
						}
					}
				}
			}
		}
	}
}
