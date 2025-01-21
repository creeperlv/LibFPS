using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class TrackedObjectPool : MonoBehaviour
	{
		public static TrackedObjectPool Instance = null;
		public List<BaseEntity> Entities;
		void Start()
		{
			Instance = this;
		}
	}
}
