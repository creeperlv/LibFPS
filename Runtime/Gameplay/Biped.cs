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
		public bool IsRunning;
		public void Move(float h, float v)
		{
			if (h == 0 && v == 0)
			{
				UpperAnimator.SetBool(AnimatorWalking, false);
				UpperAnimator.SetBool(AnimatorRunning, false);
			}
			else
			{
				UpperAnimator.SetBool(AnimatorWalking, true);
				UpperAnimator.SetBool(AnimatorRunning, IsRunning);
			}
			var t = CharacterController.transform;
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
			CharacterController.SimpleMove(t.forward * v + t.right * h + t.up * -10);
		}
	}
}