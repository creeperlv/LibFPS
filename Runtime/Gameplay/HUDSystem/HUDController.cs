using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LibFPS.Gameplay.HUDSystem
{
	public class HUDController : MonoBehaviour
	{
		public static HUDController Instance = null;
		public bool ShowHitMark;
		public bool ShowKillMark;
		public Image HP;
		public Image Shield;
		public CanvasGroup BaseElement;
		public List<BaseCrosshair> Crosshairs;
		public float TargetAlpha;
		public float FadeSpeed;
		public float DeltaThreshold;
		public bool ElasticFade;
		void Start()
		{
			if (Instance != null)
			{
				Destroy(this.gameObject);
				return;
			}
			Instance = this;
		}
		public void Show()
		{
			TargetAlpha = 1;
		}
		public void Hide()
		{
			TargetAlpha = 0;
		}
		public void ShowImmediately()
		{
			TargetAlpha = 1;
		}
		public void HideImmediately()
		{
			TargetAlpha = 0;
		}
		public void Update()
		{
			if (ElasticFade)
			{
				if (TargetAlpha != BaseElement.alpha)
				{
					if (Mathf.Abs(TargetAlpha - BaseElement.alpha) < DeltaThreshold)
					{
						BaseElement.alpha = TargetAlpha;
					}
					else
					{
						BaseElement.alpha += (TargetAlpha - BaseElement.alpha) * FadeSpeed * Time.deltaTime;
					}
				}
			}
			else
			{
				if (TargetAlpha != BaseElement.alpha)
				{
					if (Mathf.Abs(TargetAlpha - BaseElement.alpha) < DeltaThreshold)
					{
						BaseElement.alpha = TargetAlpha;
					}
					else
					{
						BaseElement.alpha += (TargetAlpha - BaseElement.alpha) < 0 ? -FadeSpeed * Time.deltaTime : FadeSpeed * Time.deltaTime;
					}
				}
			}
			if (FPSController.Instance != null)
			{
				HP.fillAmount = FPSController.Instance.GetHPPercent();
				Shield.fillAmount = FPSController.Instance.GetShieldPercent();
			}
		}
	}
}
