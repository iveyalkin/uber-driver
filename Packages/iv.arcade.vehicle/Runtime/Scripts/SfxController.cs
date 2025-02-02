using UnityEngine;

namespace IV.Arcade.Vehicle
{
    [RequireComponent(typeof(IVehicle))]
    public class SfxController : MonoBehaviour
    {
        [SerializeField] private AudioSource idleSound;
        [SerializeField] private AudioSource revolvingSound;
        [SerializeField] private AudioSource tireScreechSound; // the tire screech when the car is drifting

        private IVehicleState vehicleState;

        private float initialCarEngineSoundPitch; // store the initial pitch of the car engine sound

        private void Start()
        {
            vehicleState = GetComponent<IVehicle>().State;
            initialCarEngineSoundPitch = revolvingSound.pitch;
        }

        // This method controls the car sounds. For example, the car engine will sound slow when the car speed is low because the
        // pitch of the sound will be at its lowest point. On the other hand, it will sound fast when the car speed is high because
        // the pitch of the sound will be the sum of the initial pitch + the car speed divided by 100f.
        // Apart from that, the tireScreechSound will play whenever the car starts drifting or losing traction.
        public void LateUpdate()
        {
            // todo: rpm was carRigidbody.linearVelocity.magnitude
            var engineSoundPitch = initialCarEngineSoundPitch + vehicleState.Rpm / 100f;
            revolvingSound.pitch = Mathf.Clamp(engineSoundPitch, 0f, 3f);

            if (vehicleState.IsDrifting || (vehicleState.IsTractionLocked && Mathf.Abs(vehicleState.SpeedometerKph) > 12f))
            {
                if (!tireScreechSound.isPlaying)
                    tireScreechSound.Play();
            }
            else
            {
                tireScreechSound.Stop();
            }
        }
    }
}