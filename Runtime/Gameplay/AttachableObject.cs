using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class AttachableObject : NetworkBehaviour
	{
		[Header("AttachableObject")]
		public Transform TargetTransform;
		public virtual void OnUpdate()
		{

		}
		public void LateUpdate()
		{
			if (TargetTransform != null)
			{
				this.transform.position = TargetTransform.position;
				this.transform.rotation = TargetTransform.rotation;
			}
			OnUpdate();
		}
	}
}