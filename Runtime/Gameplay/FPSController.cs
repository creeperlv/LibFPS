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
				NetCharacterController.biped.Self.Rotate(new Vector3(0, h * MouseHorizontalSpeed * Time.deltaTime, 0));
				NetCharacterController.biped.Head.Rotate(new Vector3(v * MouseVerticalSpeed * Time.deltaTime, 0, 0));
				var her = NetCharacterController.biped.Head.localEulerAngles;
				if (her.x > 180)
				{
					her.x = Mathf.Max(her.x, 270 + VerticalRange.x);
				}
				else
				{
					her.x = Mathf.Min(her.x, 90 - VerticalRange.x);
				}
				NetCharacterController.biped.Head.localEulerAngles = her;
			}
			{
				var h = Input.GetAxis("Horizontal");
				var v = Input.GetAxis("Vertical");
				NetCharacterController.biped.Move(h, v);
			}
		}
	}

}