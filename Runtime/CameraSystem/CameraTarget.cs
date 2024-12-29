using UnityEngine;

namespace LibFPS.CameraSystem
{
    public class CameraTarget : MonoBehaviour
    {
		public bool IsSmoothFollow;
		public bool NonLinearFollow;
		public float TransitionInTime;
		public float SmoothFollowSpeed;
		public bool SmoothTransitionIn;
		public bool SmoothTransitionOut;
        public static CameraTarget Instance;
		public void OnEnable()
		{
			Instance=this;
		}
		public void Start()
		{
			Instance=this;
		}
	}
}
