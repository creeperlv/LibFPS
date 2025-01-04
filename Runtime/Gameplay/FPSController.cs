using UnityEngine;
namespace LibFPS.Gameplay
{
	public class FPSController : MonoBehaviour
	{
		public NetworkedCharacterController NetCharacterController;
		public float MouseHorizontalSpeed = 1;
		public float MouseVerticalSpeed = 1;
		public float HorizontalSpeed = 1;
		public float VerticalSpeed = 1;
		public Vector2 VerticalRange;
		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Update is called once per frame
		void Update()
		{
			{
				var h = Input.GetAxis("Mouse X");
				var v = Input.GetAxis("Mouse Y");
				NetCharacterController.biped.Rotate(h * MouseHorizontalSpeed, v * MouseVerticalSpeed);
			}
			{
				NetCharacterController.WillRun.Value = (Input.GetButton("Run"));
			}
			{
				var h = Input.GetAxis("Horizontal");
				var v = Input.GetAxis("Vertical");
				//NetCharacterController.biped.Move(h, v);
				NetCharacterController.Move(h, v);
			}
		}
	}

}