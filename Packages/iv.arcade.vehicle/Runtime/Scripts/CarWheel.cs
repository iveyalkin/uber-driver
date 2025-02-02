using System;
using UnityEngine;

namespace IV.Arcade.Vehicle
{
    [Serializable]
    public class CarWheel
    {
        [SerializeField] private Transform transform;
        [SerializeField] private WheelCollider collider;
        [SerializeField] private ParticleSystem driftVfx;
        [SerializeField] private TrailRenderer skidVfx;

        private WheelFrictionCurve friction;
        private float extremumSlip;
        public float Rpm => collider.rpm;

        public float ExtremumSlip => extremumSlip;
        public float Radius => collider.radius;

        public void Initialize()
        {
            if (collider == null) return;

            var sideFriction = collider.sidewaysFriction;
            extremumSlip = sideFriction.extremumSlip;

            friction = new WheelFrictionCurve
            {
                extremumSlip = extremumSlip,
                extremumValue = sideFriction.extremumValue,
                asymptoteSlip = sideFriction.asymptoteSlip,
                asymptoteValue = sideFriction.asymptoteValue,
                stiffness = sideFriction.stiffness
            };
        }

        public void ToggleDriftVfx(bool activate)
        {
            if (!driftVfx || driftVfx.isPlaying == activate)
                return;

            if (activate)
                driftVfx.Play();
            else
                driftVfx.Stop();
        }

        public void ToggleSkidVfx(bool activate)
        {
            if (!skidVfx || skidVfx.emitting == activate)
                return;

            skidVfx.emitting = activate;
        }

        public bool RecoverTraction(float multiplier)
        {
            // the wheels have not recovered their traction if the value is not 0f. 
            // decrease the sideways friction of the wheels until we reach the initial car's grip
            if (friction.extremumSlip > extremumSlip)
            {
                friction.extremumSlip = extremumSlip * multiplier;
                collider.sidewaysFriction = friction;

                return false;
            }

            friction.extremumSlip = extremumSlip;
            collider.sidewaysFriction = friction;

            return true;
        }

        public void ApplyMotorTorque(float motorTorque)
        {
            collider.brakeTorque = 0f;
            collider.motorTorque = motorTorque;
        }

        public void CutMotorTorque()
        {
            collider.motorTorque = 0f;
        }

        public void ApplyBreakTorque(float breakTorque)
        {
            collider.brakeTorque = breakTorque;
            collider.motorTorque = 0f;
        }

        public void ApplySlipFriction(float slipFriction)
        {
            friction.extremumSlip = extremumSlip * slipFriction;
            collider.sidewaysFriction = friction;
        }

        public void Steer(float input) => collider.steerAngle = input;

        public void UpdateVisual()
        {
            collider.GetWorldPose(out var position, out var rotation);
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}