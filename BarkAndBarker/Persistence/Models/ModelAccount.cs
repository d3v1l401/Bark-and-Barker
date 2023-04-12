namespace BarkAndBarker.Persistence.Models;

public class ModelAccount : IModel
{
    public string SteamID;
    public int State;
    public DateTime LastLogin;
    public string HWID;
    public string LastIP;

    public static readonly string QuerySelectAllAccounts = "SELECT * FROM barker.accounts;";
    public static readonly string QuerySelectAccount = "SELECT * FROM barker.accounts WHERE SteamID = @SID";
    public static readonly string QueryUpdateLastLogin = "UPDATE barker.accounts SET barker.accounts.LastLogin = CURRENT_TIMESTAMP;";
    public static readonly string QueryUpdateHWID = "UPDATE barker.accounts SET barker.accounts.HWID = @HWID;";
    public static readonly string QueryUpdateIP = "UPDATE barker.accounts SET barker.accounts.LastIP = @IP;";
    public static readonly string QueryFindDuplicateHWID = "SELECT * FROM barker.accounts WHERE HWID = @HWID;";
    public static readonly string QueryCreateAccount = "INSERT INTO barker.accounts (`SteamID`, `State`) VALUES (@SID, '1');";

    public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `accounts` (
                                                              `SteamID` varchar(50) NOT NULL,
                                                              `State` int NOT NULL DEFAULT '1',
                                                              `LastLogin` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `HWID` varchar(120) DEFAULT NULL,
                                                              `LastIP` varchar(60) DEFAULT NULL,
                                                              PRIMARY KEY (`SteamID`)
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
}