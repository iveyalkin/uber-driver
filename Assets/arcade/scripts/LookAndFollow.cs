using UnityEngine;

namespace iv.arcade.uberdriver
{
    public class LookAndFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [Range(1, 10)]
        [SerializeField]
        private float followSpeed = 2;

        [Range(1, 10)]
        [SerializeField]
        public float lookSpeed = 5;

        private Vector3 initialPosition;
        private Vector3 arm;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void Start()
        {
            initialPosition = gameObject.transform.position;
            arm = initialPosition - target.position;
        }

        private void LateUpdate()
        {
            Look();
            Move();
        }

        private void Move()
        {
            var targetPos = arm + target.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }

        private void Look()
        {
            var lookDirection = target.position - transform.position;
            var rot = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
        }
    }
}