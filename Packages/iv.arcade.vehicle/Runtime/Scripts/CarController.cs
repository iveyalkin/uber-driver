using UnityEngine;

namespace IV.Arcade.Vehicle
{
    public class CarController : IVehicleController
    {
        private readonly CarState state;
        private readonly CarConfig config;
        private readonly DriftController driftController;

        public CarController(DriftController driftController, CarState state, CarConfig config)
        {
            this.state = state;
            this.config = config;
            this.driftController = driftController;
        }

        void IVehicleController.Handbrake() => driftController.Handbrake();
        void IVehicleController.RecoverTraction() => driftController.RecoverTraction();

        void IVehicleController.Boost()
        {
        }

        #region STEERING ENGINE AND BRAKING

        /// <summary>
        /// Apply positive torque to the wheels in order to go forward.
        /// NOTE: Apply breaks if the car is going backwards.
        /// </summary>
        public void GoForward()
        {
            driftController.HandleTraction();

            if (state.IsGoingForward())
            {
                HandleThrottle(config.throttleAcceleration);
                ApplyTorque(config.maxSpeed);
            }
            else // apply brakes in order to avoid strange behaviours if the car is going backwards
                Brake();
        }

        /// <summary>
        /// Apply negative torque to the wheels in order to go backwards.
        /// NOTE: Apply breaks if the car is going forward.
        /// </summary>
        public void GoReverse()
        {
            driftController.HandleTraction();

            if (state.IsGoingBackward())
            {
                HandleThrottle(-config.throttleAcceleration);
                ApplyTorque(config.maxReverseSpeed);
            }
            else // apply brakes in order to avoid strange behaviours if the car is going forward
                Brake();
        }

        /// <summary>
        /// Applies brake torque to the wheels according to the brake force.
        /// </summary>
        public void Brake()
        {
            foreach (var wheel in state.wheelsState.all)
                wheel.ApplyBreakTorque(config.brakeForce);
        }

        /// <summary>
        /// Decelerate the speed of the car.
        /// <br/> Stop the car completely if the magnitude of the car's velocity is less than threshold
        /// <br/>NOTE: Should be called repeatedly when there is no user input.
        /// </summary>
        public void Decelerate()
        {
            if (!Mathf.Approximately(state.throttleAxis, 0f))
            {
                var deceleration = -Mathf.Sign(state.throttleAxis) * config.throttleDeceleration;
                HandleThrottle(deceleration);
            }

            if (Mathf.Approximately(state.SpeedSqr, 0f)) return;

            driftController.HandleTraction();
            ThrottleOff();

            const float threshold = 0.25f;
            if (state.SpeedSqr < threshold)
            {
                state.Velocity = Vector3.zero;
                return;
            }

            // todo: user curves instead
            var decelerationMultiplier = 1f / (1f + 0.025f * config.decelerationMultiplier);

            state.Velocity = state.SpeedSqr * decelerationMultiplier < threshold
                ? Vector3.zero
                : state.Velocity * decelerationMultiplier;
        }

        /// <summary>
        /// Set the motor torque to 0.
        /// </summary>
        private void ThrottleOff()
        {
            foreach (var wheel in state.wheelsState.all)
                wheel.CutMotorTorque();
        }

        /// <summary>
        /// Set the throttle power smoothly.
        /// </summary>
        private void HandleThrottle(float input)
        {
            // todo: replace with curve .evaluate(state.throttleAxis + input)
            var throttleAxis = state.throttleAxis + Time.deltaTime * input;
            state.throttleAxis = Mathf.Abs(throttleAxis) >= 0.01f
                ? Mathf.Clamp(throttleAxis, -1f, 1f)
                : 0f;

            // var absThrottle = Mathf.Abs(state.throttleAxis + Time.deltaTime * input);
            // state.throttleAxis = absThrottle >= 0.01f
            //     ? Mathf.Sign(state.throttleAxis) * Mathf.Clamp(absThrottle, 0f, 1f)
            //     : 0f;
        }

        /// <summary>
        /// Apply torque force to wheels if the max speed hasn't been reached yet.
        /// </summary>
        private void ApplyTorque(float maxSpeed)
        {
            if (state.SpeedometerKph < maxSpeed) // round up to int
            {
                var motorTorque = config.maxTorque * state.throttleAxis;
                foreach (var wheel in state.wheelsState.all)
                    wheel.ApplyMotorTorque(motorTorque);
            }
            else
                foreach (var wheel in state.wheelsState.all)
                    wheel.CutMotorTorque();
        }

        #endregion

        #region STEERING

        public void SteerLeft() => HandleSteering(-config.steeringSpeed);

        public void SteerRight() => HandleSteering(config.steeringSpeed);

        public void ResetSteering()
        {
            if (Mathf.Approximately(state.steeringAxis, 0f)) return;

            var input = Mathf.Sign(state.steeringAxis) * -config.steeringSpeed;
            HandleSteering(input);
        }

        private void HandleSteering(float input)
        {
            var steeringAxis = state.steeringAxis + Time.deltaTime * input;
            state.steeringAxis = Mathf.Abs(steeringAxis) >= .1f
                ? Mathf.Clamp(steeringAxis, -1f, 1f)
                : 0f;

            var steeringAngle = state.steeringAxis * config.maxSteeringAngle;
            foreach (var wheel in state.wheelsState.frontAxis) wheel.Steer(steeringAngle);
        }

        #endregion
    }
}