namespace Footballers.Utilities
{
    public class Constance
    {
        //Footballer
        public const int MinFootballerNameLenght = 2;
        public const int MaxFootballerNameLenght = 40;


        //Team
        public const int MinTeamNameLenght = 3;
        public const int MaxTeamNameLenght = 40;
        public const string RegexTeamName = @"^[a-zA-Z0-9 .-]{3,40}$";

        public const int MinNationalityLenght = 2;
        public const int MaxNationalityLenght = 40;


        //Coach
        public const int MaxCoachNameLenght = 40;
        public const int MinCoachNameLenght = 2;


    }
}
