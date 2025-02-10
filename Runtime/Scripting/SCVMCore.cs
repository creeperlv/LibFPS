using LibFPS.Level;
using scvm.core;
using scvm.core.dispatchers;
using UnityEngine;

namespace LibFPS.Scripting
{
	public class VMInterruptTable
	{
		public const ushort FRAMEDONE = 0x0000;
	}
	public class SCVMCore : VMBridge
	{
		SCVMMachine machine;
		private void Start()
		{
			machine = new SCVMMachine();

			foreach (var item in machine.CPU.Processors)
			{
				item.SetupSyscall(new FullInterruptConfig() { IsConfigured = true, IsInjected = true }, VMInterruptTable.FRAMEDONE, InterruptType.SW);
				item.SetInterruptHandler(VMInterruptTable.FRAMEDONE, InterruptType.HW, (_) =>
				{
					machine.Dispatcher.StopExecute();
				});
			}
		}
		private void OnDestroy()
		{
			machine.Dispose();
		}
		InterruptHandler GenerateHandler(ILevelEvent levelEvent)
		{
			return (p) =>
			{
			};
		}
		public override void SetLevelEvent(ushort InterruptID, ILevelEvent levelEvent)
		{
			foreach (var item in machine.CPU.Processors)
			{
				item.SetupSyscall(new FullInterruptConfig() { IsConfigured = true, IsInjected = true }, InterruptID, InterruptType.SW);
				item.SetInterruptHandler(InterruptID, InterruptType.HW, GenerateHandler(levelEvent));
			}
		}
	}
}
