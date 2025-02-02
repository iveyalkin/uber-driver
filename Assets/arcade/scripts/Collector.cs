using IV.Core.Feedback;
using UnityEngine;

namespace iv.arcade.uberdriver
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private Score score;
        [SerializeField] private Feedback feedback;

        public void Collect(Pickup pickup)
        {
            Debug.Log($"Obtained {pickup.PickupObject.name}");
            score.AddScore(pickup.Score);

            feedback.TriggerPickup();
        }
    }
}
