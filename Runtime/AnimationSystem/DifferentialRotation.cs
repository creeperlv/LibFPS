using LibFPS.Kernel.Utils;
using LibFPS.Utilities;
using UnityEngine;

namespace LibFPS
{
	public class DifferentialRotation : MonoBehaviour
	{
		public float Offset;
		public float ZOffset=0;
		public Transform Head;
		public Vector2 VerticalRange;
		public bool GlobalRotation;
		public bool ForceYValue=true;
		public bool ForceZValue=true;
		public void VRotate(float v)
		{

			Head.Rotate(new Vector3(v, 0, 0));
			var her = Head.localEulerAngles.ToZeroCenteredRotation();
			var x = her.x;
			x = Mathf.Min(VerticalRange.y, Mathf.Max(VerticalRange.x, x));
			her.x = x;
			//if (x > 180)
			//{
			//	her.x = Mathf.Max(x, 270 + VerticalRange.x);
			//}
			//else
			//{
			//	her.x = Mathf.Min(x, 90 - VerticalRange.x);
			//}
			if (ForceYValue)
			{
				her.y = Offset;

			}
			if (ForceZValue)
			{
				her.z = ZOffset;
			}
			Head.localEulerAngles = her;
		}
	}
}
