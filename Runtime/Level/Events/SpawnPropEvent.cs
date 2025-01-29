using System;
using UnityEngine;

namespace LibFPS.Level.Events
{
	[CreateAssetMenu(menuName = "LibFPS/Events/SpawnPropEvent")]
	public unsafe class SpawnPropEvent : ScriptableObject, ILevelEvent
	{
		public void Execute(IntPtr parameters, uint ParameterSize, IntPtr ReturnValueAddress)
		{
			//TODO
			IntPtr ptr = parameters;
			int ID = ((int*)ptr)[0];
			ptr += sizeof(int);
			Vector3 Pos = ((Vector3*)ptr)[0];
			ptr += sizeof(Vector3);
			Quaternion rot = ((Quaternion*)ptr)[0];
			ptr += sizeof(Quaternion);
			Vector3 Size = ((Vector3*)ptr)[0];

			((byte*)ReturnValueAddress)[0] = 1;
		}

		public uint GetReturnValueSize()
		{
			return sizeof(byte);
		}
	}
}
