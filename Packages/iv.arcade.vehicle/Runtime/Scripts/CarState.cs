using System;
using System.Linq;
using UnityEngine;

namespace IV.Arcade.Vehicle
{
    [Serializable]
    public class CarState : IVehicleState
    {
        public const float rpmToSpeedKph = 2 * Mathf.PI * 60f / 1000f;

        private const float rpmThreshold = 1f;

        public float rpm;
        public int speedometerKph;
        public bool isDrifting;
        public bool isTractionLocked;

        // whether the steering wheel has reached the maximum value from -1 to 1.
        public float steeringAxis;

        // whether the throttle pedal has reached the maximum value from -1 to 1.
        public float throttleAxis;

        public float driftingAxis;

        private float speedSqr;
        private Vector3 localVelocity;
        private Vector3 velocity;

        public readonly WheelsState wheelsState;
        private readonly Rigidbody rigidbody;
        private readonly Transform physicsTransform;

        public float Rpm => rpm;
        public bool IsDrifting => isDrifting;
        public float DriftingAxis => driftingAxis;

        public int SpeedometerKph => speedometerKph;

        public bool IsTractionLocked => isTractionLocked;

        public float SpeedSqr => speedSqr;

        public Vector3 LocalVelocity => localVelocity;

        public Vector3 Velocity
        {
            get => velocity;
            set
            {
                SetVelocityInternal(value);
                rigidbody.linearVelocity = value;
            }
        }

        private void SetVelocityInternal(Vector3 value)
        {
            velocity = value;
            localVelocity = physicsTransform.InverseTransformDirection(value);
            speedSqr = velocity.sqrMagnitude;
        }

        // check velocity against some threshold
        public bool IsGoingBackward() => localVelocity.z <= 1f;
        public bool IsGoingForward() => localVelocity.z >= -1f;

        public CarState(Rigidbody rigidbody, WheelsState wheelsState)
        {
            this.rigidbody = rigidbody;
            this.physicsTransform = rigidbody.transform;
            this.wheelsState = wheelsState;
        }

        public void InvalidateState()
        {
            var rpm = 0f;
            foreach (var wheel in wheelsState.all)
            {
                rpm += wheel.Rpm;
            }

            rpm = Mathf.Abs(rpm / wheelsState.all.Length);

            this.rpm = rpm >= rpmThreshold ? rpm : 0f;
            this.speedometerKph = (int)(wheelsState.radius * rpm * rpmToSpeedKph + .5f); // round up to int

            SetVelocityInternal(rigidbody.linearVelocity);
        }

        public readonly struct WheelsState
        {
            public readonly CarWheel[] frontAxis;
            public readonly CarWheel[] rearAxis;
            public readonly CarWheel[] all;

            public readonly float extremumSlip;
            public readonly float radius;

            public WheelsState(CarWheel[] frontAxis, CarWheel[] rearAxis, CarWheel[] all, float extremumSlip,
                float radius)
            {
                this.frontAxis = frontAxis;
                this.rearAxis = rearAxis;
                this.all = all;
                this.extremumSlip = extremumSlip;
                this.radius = radius;
            }

            public static WheelsState Create(CarWheel[] frontAxis, CarWheel[] rearAxis)
            {
                var allWheels = frontAxis.Concat(rearAxis).ToArray();
                var extremumSlip = float.MaxValue;
                var radius = 0f;

                foreach (var wheel in allWheels)
                {
                    wheel.Initialize();
                    extremumSlip = Mathf.Min(wheel.ExtremumSlip, extremumSlip);
                    radius += wheel.Radius;
                }

                radius /= allWheels.Length;

                return new WheelsState(frontAxis, rearAxis, allWheels, extremumSlip, radius);
            }
        }
    }
}