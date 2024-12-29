using UnityEngine;
namespace LibFPS.Gameplay
{
	public class FPSController : MonoBehaviour
	{
		public NetworkedCharacterController CharacterController;
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
			var h = Input.GetAxis("Mouse X");
			var v = Input.GetAxis("Mouse Y");
			CharacterController.Self.Rotate(new Vector3(0, h * HorizontalSpeed * Time.deltaTime, 0));
			CharacterController.Head.Rotate(new Vector3(v * VerticalSpeed * Time.deltaTime, 0, 0));
			var her = CharacterController.Head.localEulerAngles;
			if (her.x > 180)
			{
				her.x = Mathf.Max(her.x, 270 + VerticalRange.x);
			}
			else
			{
				her.x = Mathf.Min(her.x, 90 - VerticalRange.x);
			}
			CharacterController.Head.localEulerAngles = her;
		}
	}

}