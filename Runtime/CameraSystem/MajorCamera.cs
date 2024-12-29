using UnityEngine;

namespace LibFPS.CameraSystem
{
	public class MajorCamera : MonoBehaviour
	{
		public Transform OperatingTransform;
		void Start()
		{

		}

		void Update()
		{
			if (CameraTarget.Instance != null)
			{
				var instance = CameraTarget.Instance;
				if (!instance.IsSmoothFollow)
				{
					OperatingTransform.position = instance.transform.position;
					OperatingTransform.rotation = instance.transform.rotation;
				}
			}
		}
	}
}
