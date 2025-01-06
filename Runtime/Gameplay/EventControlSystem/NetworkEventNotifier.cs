using Unity.Netcode;

namespace LibFPS.Gameplay.EventControlSystem
{
	public class NetworkEventNotifier : NetworkBehaviour, IEventNotifier
	{
		public void Start()
		{
			EventController.CurrentNotifier=this;
		}
		public void Hit(ulong HitterID)
		{
			HitRpc(RpcTarget.Single(HitterID, RpcTargetUse.Temp));
		}

		public void Kill(ulong HitterID)
		{
			KillRpc(RpcTarget.Single(HitterID, RpcTargetUse.Temp));
		}
		[Rpc(SendTo.SpecifiedInParams)]
		public void HitRpc(RpcParams rpcParams=default)
		{
			EventController.Instance.OnHit.Invoke();
		}
		[Rpc(SendTo.SpecifiedInParams)]
		public void KillRpc(RpcParams rpcParams = default)
		{
			EventController.Instance.OnKill.Invoke();
		}
	}
	public interface IEventNotifier
	{
		void Hit(ulong HitterID);
		void Kill(ulong HitterID);
	}
}
