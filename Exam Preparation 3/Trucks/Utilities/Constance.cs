namespace Trucks.Utilities
{
    public class Constance
    {
        //Truck
        public const int MaxRegistrationNum = 8;
        public const string RegistrationNumberRegex = @"^[A-Z]{2}\d{4}[A-Z]{2}$";

        public const int MaxVinNumLenght = 17;

        public const int MinTankCapacity = 950;
        public const int MaxTankCapacity  = 1420;

        public const int MinCargoCapacity = 5000;
        public const int MaxCargoCapacity = 29000;

        //Client
        public const int MinClientNameLenght = 3;
        public const int MaxClientNameLenght = 40;

        public const int MinClientNationalityLenght = 2;
        public const int MaxClientNationalityLenght = 40;

        //Despatcher
        public const int MinDespatcherNameLenght = 2;
        public const int MaxDespatcherNameLenght = 40;

    }
}
