using System;
using UnityEngine;

namespace LibFPS.Level.Events
{
	[CreateAssetMenu(menuName = "LibFPS/Events/SpawnWeaponEvent")]
	public unsafe class SpawnWeaponEvent : ScriptableObject, ILevelEvent
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
