using IV.Arcade.Vehicle;
using IV.Core.Feedback;
using UnityEngine;

namespace iv.arcade.uberdriver
{
    [RequireComponent(typeof(IVehicle))]
    public class CarFeedback : MonoBehaviour
    {
        [SerializeField] private Feedback feedback;

        private IVehicleState carState;

        private void Start()
        {
            carState = GetComponent<IVehicle>().State;
        }

        private void LateUpdate()
        {
            if (carState.IsDrifting)
            {
                feedback.TriggerDrift(Mathf.Abs(carState.DriftingAxis));
            }
        }

        private void OnCollisionEnter(Collision _)
        {
            OnCollision();
        }

        private void OnCollision()
        {
            feedback.TriggerCollision(carState.SpeedSqr);
        }
    }
}