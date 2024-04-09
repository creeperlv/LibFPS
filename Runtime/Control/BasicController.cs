using UnityEngine;

namespace LibFPS
{
    public class BasicController : MonoBehaviour
    {
        public float MoveSpeed;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
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
