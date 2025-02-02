namespace IV.Arcade.Vehicle
{
    public interface IVehicleController
    {
        void GoForward();
        void GoReverse();

        void Handbrake();
        void Boost();

        void Decelerate();
        void RecoverTraction();


        void SteerRight();
        void SteerLeft();

        void ResetSteering();
    }
}