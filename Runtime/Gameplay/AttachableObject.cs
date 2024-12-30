using UnityEngine;

namespace LibFPS.Gameplay
{
	public class AttachableObject : MonoBehaviour
	{
		public Transform TargetTransform;
		public virtual void OnUpdate()
		{

		}
		public void Update()
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