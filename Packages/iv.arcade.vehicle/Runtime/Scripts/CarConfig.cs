using UnityEngine;

namespace IV.Arcade.Vehicle
{
    [CreateAssetMenu(menuName = "iv/Arcade/Vehicle/CarConfig")]
    public class CarConfig : ScriptableObject
    {
        [Space(10)]
        [Range(20, 190)]
        [Tooltip("The maximum speed that the car can reach in km/h.")]
        public int maxSpeed = 90;

        [Range(10, 120)]
        [Tooltip("The maximum speed that the car can reach while going on reverse in km/h.")]
        public int maxReverseSpeed = 45;

        [Min(1)]
        [Tooltip("How fast the car can accelerate.")]
        public int maxTorque = 100;

        [Range(1, 10)]
        [Tooltip("How fast the pedal goes to the metal.")]
        public int throttleAcceleration = 3;

        [Range(1, 10)]
        [Tooltip("How fast the throttle pedal goes to the neutral position.")]
        public int throttleDeceleration = 10;

        [Space(10)]
        [Range(10, 45)]
        [Tooltip("The maximum angle that the wheel can reach while rotating the steering wheel.")]
        public int maxSteeringAngle = 27;

        [Min(1)]
        [Tooltip("How fast the steering wheel turns.")]
        public float steeringSpeed = 10;

        [Space(10)]
        [Range(100, 600)]
        [Tooltip("The strength of the car's brakes.")]
        public int brakeForce = 350;

        [Range(1, 10)]
        [Tooltip(@"How fast the car decelerates when the user is not using the throttle.
1 is the slowest and 10 is the fastest deceleration.")]
        public int decelerationMultiplier = 2;

        [Range(1, 10)]
        [Tooltip(@"How much grip the car loses when the user hit the handbrake.
If this value is small, then the car will not drift too much, but if it is high,
then you could make the car to feel like going on ice.")]
        public int handbrakeDriftMultiplier = 5;

        [Space(10)]
        [Tooltip(@"The center of mass of the car. Keep the value in the points of x = 0 and z = 0 of the car.
Tweak the value of the y axis. The higher this value is, the more unstable the car becomes.
Usually, the y value goes from 0 to 1.5.")]
        public Vector3 bodyMassCenter;
    }
}