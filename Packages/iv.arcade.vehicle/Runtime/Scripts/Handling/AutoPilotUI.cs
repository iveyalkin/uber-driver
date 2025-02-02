using UnityEngine;
using UnityEngine.InputSystem;

namespace IV.Arcade.Vehicle.Handling
{
    [RequireComponent(typeof(IVehicle))]
    public class AutoPilotUI : MonoBehaviour
    {
        private const float inputThreshold = 0.1f;

        [SerializeField]
        private Transform cameraTransform;

        [SerializeField]
        private InputActionReference joystick;

        [SerializeField]
        private InputActionReference handbrake;

        [SerializeField]
        private InputActionReference boost;

        private IVehicleController vehicleController;

        private void Start()
        {
            vehicleController ??= GetComponent<IVehicle>().Controller;
        }

        private void FixedUpdate()
        {
            if (joystick.action.inProgress)
            {
                var direction = joystick.action.ReadValue<Vector2>();
                EngageAutopilot(direction);
            }
            else
            {
                vehicleController.ResetSteering();
                vehicleController.Decelerate();
            }

            if (handbrake.action.IsPressed())
                vehicleController.Handbrake();
            else
                vehicleController.RecoverTraction();

            if (boost.action.IsPressed())
                vehicleController.Boost();
        }

        private void EngageAutopilot(Vector2 direction)
        {
            var worldDirection = cameraTransform.TransformDirection(direction.x, 0f, direction.y);
            var localDirection = transform.InverseTransformDirection(worldDirection);
            switch (localDirection.z)
            {
                case > inputThreshold:
                    vehicleController.GoForward();
                    break;
                case < -inputThreshold:
                    vehicleController.GoReverse();
                    break;
                default:
                    vehicleController.Decelerate();
                    break;
            }

            switch (localDirection.x)
            {
                case > inputThreshold:
                    vehicleController.SteerRight();
                    break;
                case < -inputThreshold:
                    vehicleController.SteerLeft();
                    break;
                default:
                    vehicleController.ResetSteering();
                    break;
            }
        }
    }
}