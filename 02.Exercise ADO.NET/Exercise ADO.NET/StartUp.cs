using Microsoft.Data.SqlClient;
using System.Data;
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

            //string res2 = await GetVillainsNameAndHisMinionsName(connection,villainId);

            //Console.WriteLine(res);


            //Task-3
            //string[]? minnionsData = Console.ReadLine()?.Split(':', StringSplitOptions.RemoveEmptyEntries);
            //string[]? villainsData = Console.ReadLine()?.Split(": ", StringSplitOptions.RemoveEmptyEntries);

            //string? result3 = await InsertVillainAndMinnionsAsync(connection, minnionsData, villainsData);

            //Console.WriteLine(result);


            //Task-4
            //string countryName = Console.ReadLine();

            //string result4 = await UpdateTownsByCounryAndReturnTheirName(connection,countryName);

            //await Console.Out.WriteLineAsync(result);


            //Task-5
            //int villainId = int.Parse(Console.ReadLine());

            //string result5 = await DeleteVillainAndHisMinionsAsync(connection,villainId);

            //Console.WriteLine(result5);


            //Task-6
            //string result6 = await OrderMinionsAsync(connection);
            //Console.WriteLine(result6);


            //Task-7
            //int[]? minionsId = Console.ReadLine()?.
            //    Split(" ", StringSplitOptions.RemoveEmptyEntries).
            //    Select(int.Parse).
            //    ToArray();

            //string result7 = await IncreaseMinionsAgeAsync(connection, minionsId);

            //Console.WriteLine(result7);

            // Task -8
            int minionId = int.Parse(Console.ReadLine());

            string result8 = await IncreaseAgeAsync(connection,minionId);

            Console.WriteLine(result8);

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
        static async Task<string> UpdateTownsByCounryAndReturnTheirName(SqlConnection connection, string countryName)
        {

            SqlCommand getCountryName = new SqlCommand(SQLQueries.GetCountryName, connection);
            getCountryName.Parameters.AddWithValue("@countryName", countryName);

            SqlDataReader reader = await getCountryName.ExecuteReaderAsync();

            StringBuilder sb = new StringBuilder();

            if (!reader.HasRows)
            {
                return "No town names were affected.";
            }


            SqlCommand updateTownName = new SqlCommand(SQLQueries.UpdateTownsName, connection);
            updateTownName.Parameters.AddWithValue("@countryName", countryName);

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
            sb.AppendLine($"[{string.Join(", ", towns)}]");

            return sb.ToString().TrimEnd();
        }

        // Task - 5
        static async Task<string> DeleteVillainAndHisMinionsAsync(SqlConnection connection, int villainId)
        {
            SqlTransaction transaction = connection.BeginTransaction();
            StringBuilder sb = new StringBuilder();

            try
            {
                SqlCommand getVillainNameCmd = new SqlCommand(SQLQueries.GetVillainNameById, connection, transaction);
                getVillainNameCmd.Parameters.AddWithValue("@villainId", villainId);

                object? villainNameObj = await getVillainNameCmd.ExecuteScalarAsync();
                if (villainNameObj == null)
                {
                    return "No such villain was found.";
                }
                string villainName = (string)villainNameObj;


                SqlCommand deleteMinionsCmd = new SqlCommand(SQLQueries.DeleteVillainsMinions, connection, transaction);
                deleteMinionsCmd.Parameters.AddWithValue("@villainId", villainId);

                int deleteMinionsCount = await deleteMinionsCmd.ExecuteNonQueryAsync();

                SqlCommand deleteVillainCmd = new SqlCommand(SQLQueries.DeleteVillainsById, connection, transaction);
                deleteVillainCmd.Parameters.AddWithValue("@villainId", villainId);

                await deleteVillainCmd.ExecuteNonQueryAsync();

                sb.AppendLine($"{villainName} was deleted.");
                sb.AppendLine($"{deleteMinionsCount} minions were released.");

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return sb.ToString().TrimEnd();
        }

        // Task - 6
        static async Task<string> OrderMinionsAsync(SqlConnection connection)
        {
            var sb = new StringBuilder();

            SqlCommand orderMinionsCmd = new SqlCommand(SQLQueries.OrderMinions, connection);
            SqlDataReader minionReader = await orderMinionsCmd.ExecuteReaderAsync();

            var minionList = new List<string>();

            while (minionReader.Read())
            {
                string minionName = (string)minionReader["Name"];
                minionList.Add(minionName);
            }

            int k = 0;
            int t = 1;
            for (int i = 0; i < minionList.Count; i++)
            {
                if (i % 2 == 0 || i == 0)
                {
                    sb.AppendLine(minionList[0 + k++]);
                }
                else
                {
                    sb.AppendLine(minionList[minionList.Count - t++]);
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Task -7
        static async Task<string> IncreaseMinionsAgeAsync(SqlConnection connection, int[] minionsId)
        {
            var sb = new StringBuilder();

            SqlCommand updateMinionsAgeCmd = new SqlCommand(SQLQueries.UpdateMinionsAge, connection);

            foreach (var id in minionsId)
            {
                updateMinionsAgeCmd.Parameters.Clear();
                updateMinionsAgeCmd.Parameters.AddWithValue("@Id", id);
                await updateMinionsAgeCmd.ExecuteNonQueryAsync();
            }


            SqlCommand getAllMinionsCmd = new SqlCommand(SQLQueries.GetAllMinionsNameAndAge, connection);

            SqlDataReader minionReader = await getAllMinionsCmd.ExecuteReaderAsync();

            while (minionReader.Read())
            {
                string minionName = (string)minionReader["Name"];
                int minionAge = (int)minionReader["Age"];

                sb.AppendLine($"{minionName} {minionAge}");
            }

            return sb.ToString().TrimEnd();
        }

        // Task -8
        static async Task<string> IncreaseAgeAsync(SqlConnection connection,int minionId)
        {
            SqlCommand increaseAgeCmd = new SqlCommand(SQLQueries.StoredProcedureIncreaseMinionsAge, connection); // Replace with your actual stored procedure name

            increaseAgeCmd.CommandType = CommandType.StoredProcedure;
            increaseAgeCmd.Parameters.AddWithValue("@id", minionId);

            await increaseAgeCmd.ExecuteNonQueryAsync();

            SqlCommand getMinionsNameAndAgeCmd = new SqlCommand(SQLQueries.GetMinionNameAndAgeById, connection);
            getMinionsNameAndAgeCmd.Parameters.AddWithValue("@Id", minionId);

            SqlDataReader reader = await getMinionsNameAndAgeCmd.ExecuteReaderAsync();

            var sb = new StringBuilder();
            while (reader.Read())
            {
                string minionName = (string)reader["Name"];
                int minionAge = (int)reader["Age"];

                sb.AppendLine($"{minionName} - {minionAge} years old");
            }

            return sb.ToString().TrimEnd();
        }
    }
}