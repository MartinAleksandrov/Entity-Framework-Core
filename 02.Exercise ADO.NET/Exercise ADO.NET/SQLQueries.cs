using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_ADO.NET
{
    internal class SQLQueries
    {
        public const string ConnectionString = @"Server=THEBEAST;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";
                                                                                   

        public const string VillainsNameAndNumsOfMinions
            = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                  FROM Villains AS v 
                  JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
              GROUP BY v.Id, v.Name 
                HAVING COUNT(mv.VillainId) > 3 
              ORDER BY COUNT(mv.VillainId)";


        public const string VillainsName = @"SELECT Name FROM Villains WHERE Id = @Id";


        public const string GetAllMinionsByVillianId = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                                                             m.Name, 
                                                                             m.Age
                                                                        FROM MinionsVillains AS mv
                                                                        JOIN Minions As m ON mv.MinionId = m.Id
                                                                       WHERE mv.VillainId = @Id
                                                                    ORDER BY m.Name";


        public const string GetMinnionName = @"SELECT Id FROM Minions WHERE Name = @Name";

        public const string GetTownId = @"SELECT Id FROM Towns WHERE Name = @townName";

        public const string GetVillainId = @"SELECT Id FROM Villains WHERE Name = @Name";

        public const string InsertTownToDB = @"INSERT INTO Towns (Name) VALUES (@townName)";

        public const string InsertMinnionDB = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string InsertVillainDB = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        public const string InsertMinIdAndVillainIdDB = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";


        public const string UpdateTownsName = @"UPDATE Towns
                                                     SET Name = UPPER(Name)
                                                   WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";


        public const string GetCountryName = @"SELECT t.Name 
                                                 FROM Towns as t
                                                 JOIN Countries AS c ON c.Id = t.CountryCode
                                                WHERE c.Name = @countryName";


        public const string GetVillainNameById = @"SELECT Name FROM Villains WHERE Id = @villainId";

        public const string DeleteVillainsMinions = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";

        public const string DeleteVillainsById = @"DELETE FROM Villains WHERE Id = @villainId";

        public const string OrderMinions = @"SELECT Name FROM Minions";


        public const string UpdateMinionsAge = @"UPDATE Minions
                                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                  WHERE Id = @Id";


        public const string GetAllMinionsNameAndAge = @"SELECT Name, Age FROM Minions";

        public const string StoredProcedureIncreaseMinionsAge = @"GO
CREATE PROC usp_GetOlder @id INT
AS
UPDATE Minions
   SET Age += 1
 WHERE Id = @id";

        public const string GetMinionNameAndAgeById = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
    }
}
