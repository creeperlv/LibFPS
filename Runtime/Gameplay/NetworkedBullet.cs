using LibFPS.Gameplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedBullet : NetworkBehaviour
	{
		public Bullet Bullet;

		public void Update()
		{
			if (!this.IsHost && !this.IsServer)
				return;
			if (Bullet.TrackIntensity == 0)
			{
				this.transform.Translate(this.transform.forward * Bullet.MoveSpeed * Time.deltaTime);
			}
			else
			{

			}
		}
	}
}
