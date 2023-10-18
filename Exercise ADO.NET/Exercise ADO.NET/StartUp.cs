using Microsoft.Data.SqlClient;
using System.Text;

namespace Exercise_ADO.NET
{
    internal class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection? connection = new SqlConnection(SQLQueries.ConnectionString);

            await connection.OpenAsync();

            //Task-2
            //int villainId = int.Parse(Console.ReadLine());

            //string res = await GetVillainsNameAndHisMinionsName(connection,villainId);

            //Console.WriteLine(res);

            //Task-3
            //string[]? minnionsData = Console.ReadLine()?.Split(':', StringSplitOptions.RemoveEmptyEntries);
            //string[]? villainsData = Console.ReadLine()?.Split(": ", StringSplitOptions.RemoveEmptyEntries);

            //string? result = await InsertVillainAndMinnionsAsync(connection, minnionsData, villainsData);

            //Console.WriteLine(result);

            //Task-4
            string countryName = Console.ReadLine();

            string result = await UpdateTownsByCounryAndReturnTheirName(connection,countryName);

            await Console.Out.WriteLineAsync(result);
        }
        // Task - 1
        static async Task<string> GetVillainsAndTheirMinionsAsync(SqlConnection connection)
        {
            SqlCommand? command = new SqlCommand(SQLQueries.VillainsNameAndNumsOfMinions, connection);

            SqlDataReader? reader = await command.ExecuteReaderAsync();

            StringBuilder? stringBuilder = new StringBuilder();

            while (reader.Read())
            {

                string? villainName = (string)reader["Name"];

                int? minionsCount = (int)reader["MinionsCount"];

                stringBuilder.AppendLine($"{villainName} - {minionsCount}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        // Task - 2
        static async Task<string> GetVillainsNameAndHisMinionsName(SqlConnection connection, int VillainId)
        {
            SqlCommand? commandVillainName = new SqlCommand(SQLQueries.VillainsName, connection);
            commandVillainName.Parameters.AddWithValue("@Id", VillainId);

            object? villainNameObj = await commandVillainName.ExecuteScalarAsync();

            StringBuilder? sb = new StringBuilder();

            if (villainNameObj == null)
            {
                return $"No villain with ID {VillainId} exists in the database.";
            }

            string? villianName = (string)villainNameObj;

            SqlCommand? allMinionsCmd = new SqlCommand(SQLQueries.GetAllMinionsByVillianId, connection);
            allMinionsCmd.Parameters.AddWithValue("@Id", VillainId);

            SqlDataReader? reader = await allMinionsCmd.ExecuteReaderAsync();

            sb.AppendLine($"Villain: {villianName}");
            if (!reader.HasRows)
            {
                sb.AppendLine("(no minions)");

                return sb.ToString().TrimEnd();
            }

            while (reader.Read())
            {
                long? rowNum = (long)reader["RowNum"];
                string? minnionName = (string)reader["Name"];
                int? minnionAge = (int)reader["Age"];

                sb.AppendLine($"{rowNum}. {minnionName} {minnionAge}");
            }
            return sb.ToString().TrimEnd();
        }

        // Task - 3
        static async Task<string> InsertVillainAndMinnionsAsync(SqlConnection connection, string[] minionsDT, string[] villainsDT)
        {
            string[]? minionsData = minionsDT[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string? villianName = villainsDT[1];


            string? minnionName = minionsData[0];
            int? minnionAge = int.Parse(minionsData[1]);
            string? townName = minionsData[2];

            SqlTransaction? transaction = connection.BeginTransaction();

            StringBuilder? sb = new StringBuilder();
            try
            {
                SqlCommand? townNameCmd = new SqlCommand(SQLQueries.GetTownId, connection, transaction);
                townNameCmd.Parameters.AddWithValue("@townName", townName);

                object? townNameObj = await townNameCmd.ExecuteScalarAsync();

                if (townNameObj == null)
                {
                    SqlCommand? addTownDB = new SqlCommand(SQLQueries.InsertTownToDB, connection, transaction);
                    addTownDB.Parameters.AddWithValue("@townName", townName);

                    await addTownDB.ExecuteNonQueryAsync();
                    townNameObj = await townNameCmd.ExecuteScalarAsync();

                    sb.AppendLine($"Town {townName} was added to the database.");

                }
                int? townId = (int?)townNameObj;
                //////////////////////////////////////

                SqlCommand? villainIdCmd = new SqlCommand(SQLQueries.GetVillainId, connection, transaction);
                villainIdCmd.Parameters.AddWithValue("@Name", villianName);

                object? villainIdObj = await villainIdCmd.ExecuteScalarAsync();
                if (villainIdObj == null)
                {
                    SqlCommand? addVillainDB = new SqlCommand(SQLQueries.InsertVillainDB, connection, transaction);
                    addVillainDB.Parameters.AddWithValue("@villainName", villianName);

                    await addVillainDB.ExecuteNonQueryAsync();
                    villainIdObj = await villainIdCmd.ExecuteScalarAsync();

                    sb.AppendLine($"Villain {villianName} was added to the database.");
                }

                int? villainId = (int?)villainIdObj;
                //////////////////////////////////////

                SqlCommand? minnionIdCmd = new SqlCommand(SQLQueries.GetMinnionName, connection, transaction);
                minnionIdCmd.Parameters.AddWithValue("@Name", minnionName);

                object? minIdObj = await minnionIdCmd.ExecuteScalarAsync();
                if (minIdObj == null)
                {
                    SqlCommand? addMinnionDB = new SqlCommand(SQLQueries.InsertMinnionDB, connection, transaction);

                    addMinnionDB.Parameters.AddWithValue("@Name", minnionName);
                    addMinnionDB.Parameters.AddWithValue("@age", minnionAge);
                    addMinnionDB.Parameters.AddWithValue("@townId", townId);

                    await addMinnionDB.ExecuteNonQueryAsync();

                    minnionIdCmd = new SqlCommand(SQLQueries.GetMinnionName, connection, transaction);
                    addMinnionDB.Parameters.AddWithValue("@Name", minnionName);

                    int? minionId = (int?)await minnionIdCmd.ExecuteScalarAsync();

                    SqlCommand? addVillIdAndMinIdDB = new SqlCommand(SQLQueries.InsertMinIdAndVillainIdDB, connection, transaction);

                    addVillIdAndMinIdDB.Parameters.AddWithValue("@minionId", minionId);
                    addVillIdAndMinIdDB.Parameters.AddWithValue("@villainId", villainId);

                    await addVillIdAndMinIdDB.ExecuteNonQueryAsync();
                }
                //////////////////////////////////////

                sb.AppendLine($"Successfully added {minnionName} to be minion of {villianName}.");

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            return sb.ToString().TrimEnd();
        }

        // Task - 4
        static async Task<string> UpdateTownsByCounryAndReturnTheirName(SqlConnection connection,string countryName)
        {

            SqlCommand getCountryName = new SqlCommand(SQLQueries.GetCountryName,connection);
            getCountryName.Parameters.AddWithValue("@countryName",countryName);

            SqlDataReader reader = await getCountryName.ExecuteReaderAsync();

            StringBuilder sb = new StringBuilder();

            if (!reader.HasRows)
            {
                return "No town names were affected.";
            }


            SqlCommand updateTownName = new SqlCommand(SQLQueries.UpdateTownsName,connection);
            updateTownName.Parameters.AddWithValue("@countryName",countryName);

            await reader.CloseAsync();

            int count = await updateTownName.ExecuteNonQueryAsync();

            reader = await getCountryName.ExecuteReaderAsync();

            List<string> towns = new List<string>();
            while (reader.Read())
            {
                string town = (string)reader["Name"];
                towns.Add(town);
            }
            sb.AppendLine($"{count} town names were affected.");
            sb.AppendLine($"[{string.Join(", ",towns)}]");

            return sb.ToString().TrimEnd();
        }
    }
}