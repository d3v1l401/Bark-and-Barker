using Newtonsoft.Json;

namespace BarkAndBarker.Shared.Settings
{
    public class Settings
    {
        public class SData
        {
            public string CSAddress { get; set; }
            public UInt16 CSPort { get; set; }

            public string LobbyAddress { get; set; }
            public UInt16 LobbyPort { get; set; }

            public string DBConnectionString { get; set; }
        }

        public static SData ImportSettings()
        {
            var envSettings = Environment.GetEnvironmentVariables();

            return new SData()
            {
                CSAddress = envSettings["CSAddress"] as string,
                CSPort = UInt16.Parse(envSettings["CSPort"] as string),
                LobbyAddress = envSettings["LobbyAddress"] as string,
                LobbyPort = UInt16.Parse(envSettings["LobbyPort"] as string),
                DBConnectionString = envSettings["DBConnectionString"] as string,
            };
        }

        public static SData ImportSettings(string path)
            => JsonConvert.DeserializeObject<SData>(File.ReadAllText(path));

        public static void ExportSettings(SData data, string path)
            => File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
    }
}
