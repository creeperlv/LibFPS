using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedBullet : NetworkBehaviour
	{
		public int BulletID;
		public void Update()
		{
			if (!this.IsHost && !this.IsServer)
				return;
			
		}
	}
}
