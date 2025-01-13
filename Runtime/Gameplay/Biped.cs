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
		public float JumpVelocity;
		public float VSpeedCap;
		public float Gravity;
		public string AnimatorWalking = "IsWalking";
		public string AnimatorRunning = "IsRunning";
		public string AnimatorCrouch = "IsCrouch";
		public string Forward = "Forward";
		public string Backward = "Backward";
		public string Leftward = "Leftward";
		public string Rightward = "Rightward";
		public string IsFloating = "IsFloating";
		public string Floating = "Floating";
		public bool IsRunning;
		public bool IsCrouch;
		public Vector2 VerticalRange;
		public string UpperAnimatorAnimationController;
		private string __UpperAnimatorAnimationController;
		public Vector2 MoveDirection;
		public bool IsInVehicle;
		private bool __IsRunning;
		private bool __IsGrounded;
		private bool __LastIsGrounded;
		private float __IdealVSpeed;
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
			__IsGrounded = CharacterController.isGrounded;
		}
		bool TryJump = false;
		public void Jump()
		{
			TryJump = true;
		}
		public void Rotate(float h, float v)
		{
			Self.Rotate(new Vector3(0, h * Time.deltaTime, 0));
			Head.Rotate(new Vector3(v * Time.deltaTime, 0, 0));
			var her = Head.localEulerAngles;
			var x = her.x % 360;
			if (x > 180)
			{
				her.x = Mathf.Max(x, 270 + VerticalRange.x);
			}
			else
			{
				her.x = Mathf.Min(x, 90 - VerticalRange.x);
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
			{
				UpperAnimator.SetBool(IsFloating, !__IsGrounded);
				LowerAnimator.SetBool(IsFloating, !__IsGrounded);
			}
			if (__LastIsGrounded != __IsGrounded)
			{
				__LastIsGrounded = __IsGrounded;
				if (!__IsGrounded)
				{
					UpperAnimator.SetTrigger(Floating);
					LowerAnimator.SetTrigger(Floating);
				}
			}
			CharacterController.Move((t.forward * v + t.right * h) * Time.deltaTime);// + t.up * -10);
			if (__IsGrounded)
			{
				if (TryJump)
					__IdealVSpeed = CharacterController.velocity.y + JumpVelocity;
			}
			else
			{
			}
			CharacterController.Move(t.up * __IdealVSpeed * Time.deltaTime);
			__IdealVSpeed += Gravity * Time.deltaTime;
			__IdealVSpeed = Mathf.Max(Mathf.Min(__IdealVSpeed, VSpeedCap), -VSpeedCap);
			TryJump = false;
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