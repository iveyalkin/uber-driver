using UnityEngine;

namespace iv.arcade.uberdriver
{
    [RequireComponent(typeof(Collider))]
    public partial class Pickup : MonoBehaviour
    {
        [SerializeField]
        private int score;

        [SerializeField]
        private GameObject pickupObject;

        public int Score => score;

        public GameObject PickupObject => pickupObject;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Collector>(out var collector))
            {
                collector.Collect(this);
                gameObject.SetActive(false);
            }
        }
    }
}
