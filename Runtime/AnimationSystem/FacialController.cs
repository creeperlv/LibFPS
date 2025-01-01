using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class FacialController : MonoBehaviour
	{
		public MonoBehaviour Bridge;
		public IFacialBridge bridge;
		public float BlinkInterval;
		public float EyeCloseTime;
		public float EyeOpenWeight;
		public float EyeCloseWeight;
		private float Duration;
		public void Start()
		{
			bridge = Bridge as IFacialBridge;
		}
		public void Update()
		{
			if (Duration < BlinkInterval)
			{
				bridge.SetEyeOpen(EyeOpenWeight);
			}
			else
			{
				bridge.SetEyeOpen(EyeCloseWeight);
				if (Duration > BlinkInterval + EyeCloseTime)
				{
					Duration = 0;
				}
			}
			Duration += Time.deltaTime;
		}
	}
	public interface IFacialBridge
	{
		void SetEyeOpen(float v);
	}
}
