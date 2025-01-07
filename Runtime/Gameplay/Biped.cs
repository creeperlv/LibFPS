using LibFPS.AnimationSystem;
using LibFPS.Kernel.ResourceManagement;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class Biped : MonoBehaviour
	{
		public string BipedName;
		public CharacterController CharacterController;
		public TransformRotationDelayedSync Syncer;
		public Transform Head;
		public Transform Self;
		public Animator UpperAnimator;
		public Animator LowerAnimator;
		public float WalkSpeed;
		public float RunSpeed;
		public float CrouchSpeed;
		public string AnimatorWalking = "IsWalking";
		public string AnimatorRunning = "IsRunning";
		public string AnimatorCrouch = "IsCrouch";
		public string Forward = "Forward";
		public string Backward = "Backward";
		public string Leftward = "Leftward";
		public string Rightward = "Rightward";
		public bool IsRunning;
		public bool IsCrouch;
		public Vector2 VerticalRange;
		public string UpperAnimatorAnimationController;
		private string __UpperAnimatorAnimationController;
		public Vector2 MoveDirection;
		private bool __IsRunning;
		private void Update()
		{
			if (__UpperAnimatorAnimationController != UpperAnimatorAnimationController)
			{
				if (ResourceManager.TryQueryAnimationController(BipedName, UpperAnimatorAnimationController, out var __controller))
				{
					UpperAnimator.runtimeAnimatorController = __controller;
					__UpperAnimatorAnimationController = UpperAnimatorAnimationController;
				}
			}
			Move(MoveDirection.x, MoveDirection.y);
		}
		public void Rotate(float h, float v)
		{

			Self.Rotate(new Vector3(0, h * Time.deltaTime, 0));
			Head.Rotate(new Vector3(v * Time.deltaTime, 0, 0));
			var her = Head.localEulerAngles;
			if (her.x > 180)
			{
				her.x = Mathf.Max(her.x, 270 + VerticalRange.x);
			}
			else
			{
				her.x = Mathf.Min(her.x, 90 - VerticalRange.x);
			}
			Head.localEulerAngles = her;
		}
		private void Move(float h, float v)
		{
			MoveDirection = MoveDirection.normalized;
			if (h == 0 && v == 0)
			{
				UpperAnimator.SetBool(AnimatorWalking, false);
				UpperAnimator.SetBool(AnimatorRunning, false);
				UpperAnimator.SetBool(AnimatorCrouch, IsCrouch);
				LowerAnimator.SetBool(AnimatorWalking, false);
				LowerAnimator.SetBool(AnimatorRunning, false);
				LowerAnimator.SetBool(AnimatorCrouch, IsCrouch);
				Syncer.IsActiveMoving = false;
			}
			else
			{
				__IsRunning = IsRunning;
				Syncer.IsActiveMoving = true;
				var av = Mathf.Abs(v);
				var ah = Mathf.Abs(h);
				if (IsCrouch) __IsRunning = false;
				if (av > ah)
				{
					if (v > 0)
					{
						SetDirection(UpperAnimator, true, false, false, false);
						SetDirection(LowerAnimator, true, false, false, false);
					}
					else
					{
						SetDirection(UpperAnimator, false, true, false, false);
						SetDirection(LowerAnimator, false, true, false, false);
					}
				}
				else
				{
					if (h > 0)
					{
						SetDirection(UpperAnimator, false, false, false, true);
						SetDirection(LowerAnimator, false, false, false, true);
					}
					else
					{
						SetDirection(UpperAnimator, false, false, true, false);
						SetDirection(LowerAnimator, false, false, true, false);
					}
				}
				if (IsCrouch)
				{
					h *= CrouchSpeed;
					v *= CrouchSpeed;
				}
				else
				if (__IsRunning)
				{
					h *= RunSpeed;
					v *= RunSpeed;
				}
				else
				{
					h *= WalkSpeed;
					v *= WalkSpeed;
				}
				UpperAnimator.SetBool(AnimatorWalking, true);
				UpperAnimator.SetBool(AnimatorRunning, __IsRunning);
				UpperAnimator.SetBool(AnimatorCrouch, IsCrouch);
				LowerAnimator.SetBool(AnimatorWalking, true);
				LowerAnimator.SetBool(AnimatorRunning, __IsRunning);
				LowerAnimator.SetBool(AnimatorCrouch, IsCrouch);
			}
			var t = Self;
			CharacterController.SimpleMove(t.forward * v + t.right * h + t.up * -10);
		}
		public void SetDirection(Animator animator, bool FWD, bool BWD, bool LWD, bool RWD)
		{
			if (FWD == false)
			{
				__IsRunning = false;
			}
			animator.SetBool(Forward, FWD);
			animator.SetBool(Backward, BWD);
			animator.SetBool(Leftward, LWD);
			animator.SetBool(Rightward, RWD);
		}
	}
}