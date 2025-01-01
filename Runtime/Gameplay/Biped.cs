using UnityEngine;

namespace LibFPS.Gameplay
{
	public class Biped : MonoBehaviour
	{
		public CharacterController CharacterController;
		public Transform Head;
		public Transform Self;
		public Animator UpperAnimator;
		public Animator LowerAnimator;
		public float WalkSpeed;
		public float RunSpeed;
		public string AnimatorWalking = "IsWalking";
		public string AnimatorRunning = "IsRunning";
		public string Forward = "Forward";
		public string Backward = "Backward";
		public string Leftward = "Leftward";
		public string Rightward = "Rightward";
		public bool IsRunning;
		public void Move(float h, float v)
		{
			if (h == 0 && v == 0)
			{
				UpperAnimator.SetBool(AnimatorWalking, false);
				UpperAnimator.SetBool(AnimatorRunning, false);
				LowerAnimator.SetBool(AnimatorWalking, false);
				LowerAnimator.SetBool(AnimatorRunning, false);
			}
			else
			{
				UpperAnimator.SetBool(AnimatorWalking, true);
				UpperAnimator.SetBool(AnimatorRunning, IsRunning);
				LowerAnimator.SetBool(AnimatorWalking, true);
				LowerAnimator.SetBool(AnimatorRunning, IsRunning);
			}
			var t = Self;
			if (IsRunning)
			{
				h *= RunSpeed;
				v *= RunSpeed;
			}
			else
			{
				h *= WalkSpeed;
				v *= WalkSpeed;
			}
			var av = Mathf.Abs(v);
			var ah = Mathf.Abs(h);
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
			CharacterController.SimpleMove(t.forward * v + t.right * h + t.up * -10);
		}
		public void SetDirection(Animator animator, bool FWD, bool BWD, bool LWD, bool RWD)
		{
			animator.SetBool(Forward, FWD);
			animator.SetBool(Backward, BWD);
			animator.SetBool(Leftward, LWD);
			animator.SetBool(Rightward, RWD);
		}
	}
}