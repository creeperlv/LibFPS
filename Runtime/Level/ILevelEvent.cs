using System;

namespace LibFPS.Level
{
	public interface ILevelEvent
	{
		uint GetReturnValueSize();
		void Execute(IntPtr parameters, uint ParameterSize, IntPtr ReturnValueAddress);
	}
}
