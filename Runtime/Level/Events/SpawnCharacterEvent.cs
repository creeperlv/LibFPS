using System;
using UnityEngine;

namespace LibFPS.Level.Events
{
	[CreateAssetMenu(menuName = "LibFPS/Events/SpawnCharacterEvent")]
	public unsafe class SpawnCharacterEvent : ScriptableObject, ILevelEvent
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
