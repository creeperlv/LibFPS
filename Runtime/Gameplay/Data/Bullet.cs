using System;
using UnityEngine;

namespace LibFPS.Gameplay.Data
{
    public class Bullet : MonoBehaviour
    {
        public float MoveSpeed;
        public int HitEffect;
    }
    [Serializable]
    public class WeaponDef
    {
        public float HPDamage;
        public float ShieldDamage;
        public bool IsUsingMagazine;
        public int MaxAmmoReserve;
        public int AmmoPerMagzine;
    }
}
