using UnityEngine;

namespace IV.Arcade.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarVehicle : MonoBehaviour, IVehicle
    {
        public CarWheel frontLeftWheel;
        public CarWheel frontRightWheel;
        public CarWheel rearLeftWheel;
        public CarWheel rearRightWheel;

        public CarConfig config;

        private Rigidbody carRigidbody;
        private CarState carState;
        private DriftController driftController;
        private CarController carController;

        public IVehicleController Controller => carController;
        public IVehicleState State => carState;

        private void Awake()
        {
            carRigidbody = GetComponent<Rigidbody>();
            carRigidbody.centerOfMass = config.bodyMassCenter;

            carState = new CarState(carRigidbody, CarState.WheelsState.Create(
                new[] { frontLeftWheel, frontRightWheel },
                new[] { rearLeftWheel, rearRightWheel })
            );

            driftController = new DriftController(config, carState);
            carController = new CarController(driftController, carState, config);
        }

        private void FixedUpdate() => carState.InvalidateState();

        private void LateUpdate()
        {
            AnimateWheels();
        }

        /// <summary>
        /// Match the WheelCollider's position and rotation with the wheel's Mesh.
        /// </summary>
        private void AnimateWheels()
        {
            foreach (var wheel in carState.wheelsState.all)
            {
                wheel.UpdateVisual();
            }
        }
    }
}