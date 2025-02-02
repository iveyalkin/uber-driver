using UnityEngine;
using UnityEngine.UI;

namespace IV.Arcade.Vehicle
{
    public class CarHUD : MonoBehaviour
    {
        [Tooltip("Show the speed of the car.")]
        public Text carSpeedText;

        private IVehicleState vehicleState;

        private void Start()
        {
            foreach (var rootGameObject in gameObject.scene.GetRootGameObjects())
            {
                if (rootGameObject.TryGetComponent(out IVehicle vehicle))
                {
                    vehicleState = vehicle.State;
                    return;
                }
            }
        }

        private void LateUpdate()
        {
            carSpeedText.text = $"{vehicleState.SpeedometerKph}";
        }
    }
}