using LibFPS.Level;
using System;
using UnityEngine;

namespace LibFPS.Scripting
{
	public class VMBridge : MonoBehaviour
	{
		public virtual void ExecuteInterrupt(ushort ID)
		{

		}
		public virtual void SetLevelEvent(ushort InterruptID, ILevelEvent levelEvent)
		{

		}
	}
}
