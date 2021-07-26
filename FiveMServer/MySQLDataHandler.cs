using System;
using System.Collections.Generic;
using CitizenFX.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace FiveMServer
{
	class MySQLDataHandler : BaseScript
	{
		public MySQLDataHandler()
		{
			EventHandlers.Add("DSCP5M:RequestPlayerData", new Action<Player>(GetPlayerData));
			EventHandlers.Add("DSCP5M:SavePlayerData", new Action<Player, string, string>(CheckIfPedExists));
		}

		private void GetPlayerData([FromSource] Player SourcePlayer)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ServerCore.SQLConnectionData))
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand($"SELECT * FROM PlayerData WHERE Licence = @searchitem", conn);

				cmd.Parameters.AddWithValue("@searchitem", SourcePlayer.Identifiers["license"]);

				cmd.Prepare();

				using (MySqlDataReader rdr = cmd.ExecuteReader())
				{
					dt.Load(rdr);
				}
			}

			List<string> Chars = new List<string>();

			Debug.WriteLine($"{dt.Rows.Count} characters found for {SourcePlayer.Name} ({SourcePlayer.Identifiers["license"]})");

			foreach (DataRow charData in dt.Rows)
			{
				Chars.Add(charData["Data"].ToString());
			}

			Debug.WriteLine($"{Chars.Count} characters loaded for {SourcePlayer.Name} ({SourcePlayer.Identifiers["license"]})");

			SourcePlayer.TriggerEvent("DSCP5M:LoadCharacterMenu", JsonConvert.SerializeObject(Chars));
		}

		private void CheckIfPedExists([FromSource] Player SourcePlayer, string CharacterData, string CharacterID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ServerCore.SQLConnectionData))
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand($"SELECT * FROM PlayerData WHERE Licence = @searchitem AND CharacterID = @charID", conn);
				cmd.Parameters.AddWithValue("@searchitem", SourcePlayer.Identifiers["license"]);
				cmd.Parameters.AddWithValue("@charID", CharacterID);

				cmd.Prepare();

				using (MySqlDataReader rdr = cmd.ExecuteReader())
				{
					dt.Load(rdr);
				}
			}

			if (dt.Rows.Count > 0)
				SavePlayerData(SourcePlayer, CharacterData, CharacterID);
			else
				SaveNewPlayerData(SourcePlayer, CharacterData, CharacterID);
		}
		private void SavePlayerData(Player SourcePlayer, string CharacterData, string CharacterID)
		{
			using (MySqlConnection conn = new MySqlConnection(ServerCore.SQLConnectionData))
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand($"UPDATE PlayerData SET Data = @data WHERE Licence = @licence AND CharacterID = @charID", conn);
				cmd.Parameters.AddWithValue("@licence", SourcePlayer.Identifiers["license"]);
				cmd.Parameters.AddWithValue("@charID", CharacterID);
				cmd.Parameters.AddWithValue("@data", CharacterData);

				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}
		private void SaveNewPlayerData(Player SourcePlayer, string CharacterData, string CharacterID)
		{
			using (MySqlConnection conn = new MySqlConnection(ServerCore.SQLConnectionData))
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand($"INSERT INTO PlayerData (`Licence`, `CharacterID`, `Data`) VALUES (@licence, @charID, @data)", conn);
				cmd.Parameters.AddWithValue("@licence", SourcePlayer.Identifiers["license"]);
				cmd.Parameters.AddWithValue("@charID", CharacterID);
				cmd.Parameters.AddWithValue("@data", CharacterData);

				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}
	}
}
