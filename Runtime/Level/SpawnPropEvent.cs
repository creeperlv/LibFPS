using System;
using UnityEngine;

namespace LibFPS.Level
{
	[CreateAssetMenu(menuName = "LibFPS/Events/SpawnPropEvent")]
	public unsafe class SpawnPropEvent : ScriptableObject, ILevelEvent
	{
		public void Execute(IntPtr parameters, uint ParameterSize, IntPtr ReturnValueAddress)
		{
			//TODO
			((byte*)ReturnValueAddress)[0] = 1;
		}

		public uint GetReturnValueSize()
		{
			return sizeof(byte);
		}
	}
}
