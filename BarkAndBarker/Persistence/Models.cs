using BarkAndBarker.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Persistence
{
    internal static class DBExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    throw new NotImplementedException();
            }
        }
    }
    interface IModel {}

    public class DBGeneral
    {
        private static readonly string SchemaName = "barker"; // Editing this means you're going to break all the queries below.

        public static readonly string QuerySelectDBMSVersion = "SELECT @@VERSION;";
        public static readonly string QuerySelectSchemas = "SHOW DATABASES;";
        public static readonly string QueryCreateSchema = "CREATE DATABASE " + SchemaName;

        public static void CheckAndCreateDatabase(Database databaseIntance)
        {
            if (databaseIntance == null)
                throw new Exception("database instance not parsed");

            // Get all implementors of IModel
            var modelsClasses = typeof(IModel).GetImplementors(Assembly.GetExecutingAssembly()).Reverse();
            foreach (var model in modelsClasses)
            {
                // All table models should start with "Model*"
                if (model.Name.StartsWith("Model"))
                {
                    // Get the QueryCreateTable static field
                    var createQuery = model.GetMembers().First(x => x.Name == "QueryCreateTable");
                    string? queryExecute = createQuery.GetValue(createQuery) as string; // Gets its value, THIS ONLY WORKS FOR STATIC FIELDS
                    if (queryExecute != null)
                    {
                        databaseIntance.Execute(queryExecute, null); // Execute the creation, if the table doesn't exists.
                    }
                }
            }
        }
    }
    public class ModelAccount : IModel
    {
        public string   SteamID;
        public int      State;
        public DateTime LastLogin;
        public string   HWID;
        public string   LastIP;

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

    public class ModelCharacter : IModel
    {
        public int      accountID; // Foreign Key, PK
        public string   CharID; // PK
        public string   Nickname;
        public string   Class;
        public int      Level;
        public DateTime CreatedAt;
        public int      Gender;
        public DateTime LastLogin;
        public int      KarmaScore;

        public static readonly string QuerySelectAllByUserAccount = "SELECT * FROM barker.characters WHERE barker.characters.accountID = @AID";
        public static readonly string QuerySelectCharacterByID = "SELECT * FROM barker.characters WHERE barker.characters.CharID = @CID";
        public static readonly string QueryCreateCharacter = "INSERT INTO barker.characters (`accountID`, `CharID`, `Nickname`, `Class`, `Level`, `Gender`) VALUES (@AID, @CID, @Nickname, @Class, @Level, @Gender);";
        public static readonly string QueryOwnerAccountForCharacterID = "SELECT barker.accounts.* FROM barker.accounts, barker.characters WHERE barker.characters.CharID = @CID";
                      
        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `characters` (
                                                              `accountID` varchar(50) NOT NULL,
                                                              `CharID` varchar(45) NOT NULL,
                                                              `Nickname` varchar(20) NOT NULL,
                                                              `Class` varchar(60) NOT NULL,
                                                              `Level` int NOT NULL DEFAULT '1',
                                                              `CreatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `Gender` int NOT NULL,
                                                              `LastLogin` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `KarmaScore` int NOT NULL DEFAULT '0',
                                                              `IsDeleted` timestamp NULL DEFAULT NULL,
                                                              PRIMARY KEY (`accountID`,`CharID`),
                                                              CONSTRAINT `charOwner` FOREIGN KEY (`accountID`) REFERENCES `accounts` (`SteamID`)
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
    }
}
