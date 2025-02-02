namespace IV.Arcade.Vehicle
{
    public interface IVehicleState
    {
        bool IsDrifting { get; }
        float DriftingAxis { get; }
        float SpeedSqr { get; }
        float Rpm { get; }
        bool IsTractionLocked { get; }
        int SpeedometerKph { get; }
    }
}