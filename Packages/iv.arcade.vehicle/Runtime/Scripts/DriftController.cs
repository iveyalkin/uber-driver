using UnityEngine;

namespace IV.Arcade.Vehicle
{
    public class DriftController
    {
        private readonly CarConfig config;
        private readonly CarState state;

        public DriftController(CarConfig config, CarState state)
        {
            this.config = config;
            this.state = state;
        }

        /// <summary>
        /// Set tje drifting state of the car.
        /// </summary>
        public void HandleTraction()
        {
            // the car lost its traction if the forces applied to the 'x' asis are greater than
            // 2.5f. if so, the car will start emitting drifting vfx
            state.isDrifting = Mathf.Abs(state.Velocity.x) > 2.5f;
            Drift();
        }

        /// <summary>
        /// Recover the traction of the car when the user has stopped using the car's handbrake.
        /// <br/>NOTE: Should be called repeatedly when there is no user input regarding drifting.
        /// </summary>
        public void RecoverTraction()
        {
            var isRecovered = true;
            state.isTractionLocked = false;
            state.driftingAxis -= Mathf.Max(0f, state.driftingAxis - Time.deltaTime / 1.5f);

            // wait for front (or should be all?) wheels to recover
            foreach (var wheel in state.wheelsState.frontAxis)
                isRecovered = isRecovered &&
                              wheel.RecoverTraction(config.handbrakeDriftMultiplier * state.driftingAxis);

            if (isRecovered)
                state.driftingAxis = 0f;
        }

        /// <summary>
        /// Make the car lose traction so the car will start drifting.
        /// </summary>
        public void Handbrake()
        {
            HandleSlip(1f);

            // handbrake locks the wheels, the car starts to emit skidding vfx
            state.isTractionLocked = true;
            Drift();
        }

        private void HandleSlip(float input)
        {
            // start losing traction smoothly
            var driftingAxis = state.driftingAxis + Time.deltaTime * input;

            // todo: use curves instead?
            var multipliedExtremum = config.handbrakeDriftMultiplier * state.wheelsState.extremumSlip;
            var secureStartingPoint = driftingAxis * multipliedExtremum;
            if (secureStartingPoint < state.wheelsState.extremumSlip)
            {
                driftingAxis = state.wheelsState.extremumSlip / multipliedExtremum;
            }

            state.driftingAxis = Mathf.Clamp01(driftingAxis);

            HandleTraction();

            // the wheels have not reached their maximum drifting value if the 'driftingAxis' value is not 1f,
            // if so, continue increasing the sideways friction of the wheels
            if (state.driftingAxis < 1f)
            {
                var driftMultiplier = config.handbrakeDriftMultiplier * state.driftingAxis;
                foreach (var wheel in state.wheelsState.all)
                {
                    wheel.ApplySlipFriction(driftMultiplier);
                }
            }
        }

        /// <summary>
        /// Emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
        /// depending on the state.
        /// </summary>
        private void Drift()
        {
            foreach (var wheel in state.wheelsState.all)
                wheel.ToggleDriftVfx(state.isDrifting);

            var isSkidding = (state.isTractionLocked || Mathf.Abs(state.Velocity.x) > 5f) && state.speedometerKph > 12;

            foreach (var wheel in state.wheelsState.all)
                wheel.ToggleSkidVfx(isSkidding);
        }
    }
}