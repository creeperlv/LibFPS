using UnityEngine;

namespace LibFPS.CameraSystem
{
	public class MajorCamera : MonoBehaviour
	{
		public Transform OperatingTransform;
		public Camera Cam;
		public FPSCamera FPSCam;
		void Start()
		{

		}
		public void ToFPSMode()
		{
			FPSCam.ToFPSMode();
		}
		public void ToTPSMode()
		{
			FPSCam.ToTPSMode();
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
				else
				{
					if (instance.NonLinearFollow)
					{

						var d = instance.transform.position - OperatingTransform.position;
						OperatingTransform.position = OperatingTransform.position + d * Time.deltaTime * instance.SmoothFollowSpeed;
						OperatingTransform.rotation = instance.transform.rotation;
					}
				}
				FPSCam.Cam.fieldOfView = Cam.fieldOfView;
			}
		}
	}
}
