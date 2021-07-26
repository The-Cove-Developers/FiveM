using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json.Linq;
using System.IO;
using MySql.Data.MySqlClient;

namespace FiveMServer
{
	class ServerCore : BaseScript
	{
		internal static string SQLConnectionData { get; private set; }

		public ServerCore()
		{
			string fileLocation = Path.Combine(GetResourcePath(GetCurrentResourceName()), "config.json");

			if (File.Exists(fileLocation))
			{
				using (StreamReader r = new StreamReader(fileLocation))
				{
					string json = r.ReadToEnd();
					var connData = JObject.Parse(json);

					SQLConnectionData = new MySqlConnectionStringBuilder()
					{
						Server = connData["server"].ToString(),
						UserID = connData["user"].ToString(),
						Password = connData["password"].ToString(),
						Database = connData["database"].ToString(),
						Port = (uint)connData["port"]
					}.ToString();
				}
			}
		}
	}
}
