using IV.Arcade.Vehicle;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IV.Arcade.Vehicle.Handling
{
    [RequireComponent(typeof(IVehicle))]
    public class ManualSteering : MonoBehaviour
    {
        private const float inputThreshold = 0.1f;

        [SerializeField] private bool isAutoAccelerate;

        [SerializeField] private InputActionReference joystick;
        [SerializeField] private InputActionReference handbrake;
        [SerializeField] private InputActionReference boost;

        private IVehicleController controller;

        private void Start()
        {
            controller = GetComponent<IVehicle>().Controller;
        }

        private void Update()
        {
            if (joystick.action.inProgress)
            {
                var direction = joystick.action.ReadValue<Vector2>();

                if (isAutoAccelerate)
                {
                    if (Mathf.Abs(direction.x) <= inputThreshold)
                        controller.GoForward();
                    else
                        controller.Decelerate();
                }
                else
                    switch (direction.y)
                    {
                        case >= inputThreshold:
                            controller.GoForward();
                            break;
                        case <= -inputThreshold:
                            controller.GoReverse();
                            break;
                        default:
                            controller.Decelerate();
                            break;
                    }

                switch (direction.x)
                {
                    case >= inputThreshold:
                        controller.SteerRight();
                        break;
                    case <= -inputThreshold:
                        controller.SteerLeft();
                        break;
                    default:
                        controller.ResetSteering();
                        break;
                }
            }
            else
            {
                controller.ResetSteering();
                controller.Decelerate();
            }

            if (handbrake.action.IsPressed())
                controller.Handbrake();
            else
                controller.RecoverTraction();

            if (boost.action.IsPressed())
                controller.Boost();
        }
    }
}