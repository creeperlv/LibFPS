using UnityEngine;

namespace LibFPS
{
	public class DifferentialRotation : MonoBehaviour
	{
		public float Offset;
		public Transform Head;
		public Vector2 VerticalRange;
		public bool GlobalRotation;
		public void VRotate(float v)
		{

			Head.Rotate(new Vector3(v, 0, 0));
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
			her.y = Offset;
			Head.localEulerAngles = her;
		}
	}
}
