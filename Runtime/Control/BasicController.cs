using Mirror;
using UnityEngine;

namespace LibFPS.Control
{
    public class BasicController : NetworkBehaviour
    {
        public bool IsMovementInUse;
        public bool IsLookInUse;
        public float MoveSpeed;
        public GameObject LocalPlayerOnlyObjects;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            LocalPlayerOnlyObjects.SetActive(isLocalPlayer);
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) { return; }
            if (IsMovementInUse)
            {
                var v = Input.GetAxis("Vertical");
                if (Mathf.Abs(v) > 0.05f)
                {
                    this.transform.Translate(Vector3.forward * v * MoveSpeed, Space.Self);
                }
                var h = Input.GetAxis("Horizontal");
                if (Mathf.Abs(h) > 0.05f)
                {
                    this.transform.Translate(Vector3.right * h * MoveSpeed, Space.Self);
                }
            }
        }
    }
}
