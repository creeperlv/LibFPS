using System;

namespace LibFPS.Level
{
	public unsafe class SpawnCharacterEvent : ILevelEvent
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
