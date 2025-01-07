using LibFPS.Gameplay.HUDSystem;
using UnityEngine;
namespace LibFPS.Gameplay
{
	public class FPSController : MonoBehaviour
	{
		public static FPSController Instance;
		public NetworkedCharacterController NetCharacterController;
		public float MouseHorizontalSpeed = 1;
		public float MouseVerticalSpeed = 1;
		public float HorizontalSpeed = 1;
		public float VerticalSpeed = 1;
		public Vector2 VerticalRange;
		public bool IsControllingSomething;
		public float GetHPPercent()
		{
			return NetCharacterController.Entity.HP.Value / NetCharacterController.Entity.MaxHP;
		}
		public float GetShieldPercent()
		{
			if (NetCharacterController.Entity is ShieldedEntity SEntity)
			{
				return SEntity.Shield.Value / SEntity.MaxShield;
			}
			return 0;
		}
		public void TryBindTo(GameObject gameObject)
		{
			if (gameObject.TryGetComponent<NetworkedCharacterController>(out var controller))
			{
				IsControllingSomething = true;
				this.NetCharacterController = controller;
			}
			else { IsControllingSomething = false; }
		}
		void Start()
		{
			Instance = this;
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
				NetCharacterController.WillCrouch.Value = (Input.GetButton("Crouch"));
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