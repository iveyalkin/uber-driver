using UnityEngine;

namespace iv.arcade.uberdriver
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private Score score;

        public void Collect(Pickup pickup)
        {
            Debug.Log($"Obtained {pickup.PickupObject.name}");
            score.AddCurrentScore(pickup.Score);
        }
    }
}
