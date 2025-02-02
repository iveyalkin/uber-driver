namespace IV.Arcade.Vehicle
{
    public interface IVehicle
    {
        IVehicleController Controller { get; }
        IVehicleState State { get; }
    }
}