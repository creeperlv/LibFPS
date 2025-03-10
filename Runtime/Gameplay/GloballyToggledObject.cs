using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class GloballyToggledObject : NetworkBehaviour
	{
		public GameObject objectToToggle;
		public string Key;
		public bool DefaultState;
		public void Update()
		{
			var s = GlobalFlagController.QueryFlag(Key, DefaultState);
			if (objectToToggle.activeSelf != s)
				objectToToggle.SetActive(s);
		}
	}
}