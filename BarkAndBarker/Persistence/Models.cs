using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Persistence
{
    public class General
    {
        public static string QuerySelectDBMSVersion = "SELECT @@VERSION";
    }
    public class ModelAccount
    {
        public string SteamID;
        public int State;
        public DateTime LastLogin;
        public string HWID;

        public static string QuerySelectAllAccounts = "SELECT * FROM barker.accounts;";
        public static string QuerySelectAccount = "SELECT * FROM barker.accounts WHERE SteamID = @SID";
        public static string QueryUpdateLastLogin = "UPDATE barker.accounts SET barker.accounts.LastLogin = CURRENT_TIMESTAMP;";
        public static string QueryUpdateHWID = "UPDATE barker.accounts SET barker.accounts.HWID = @HWID;";
        public static string QueryFindDuplicateHWID = "SELECT * FROM barker.accounts WHERE HWID = @HWID;";
    }

    public class ModelCharacter
    {
        public int accountID; // Foreign Key
        public string CharID;
        public string Nickname;
        public string Class;
        public int Level;
        public DateTime CreatedAt;
        public int Gender;
        public DateTime LastLogin;
        public int KarmaScore;

        public static string QuerySelectAllByUserAccount = "SELECT * FROM barker.characters WHERE barker.characters.accountID = @AID";
        public static string QuerySelectCharacterByID = "SELECT * FROM barker.characters WHERE barker.characters.CharID = @CID";
        public static string QueryCreateCharacter = "INSERT INTO barker.characters (`accountID`, `CharID`, `Nickname`, `Class`, `Level`, `Gender`) VALUES (@AID, @CID, @Nickname, @Class, @Level, @Gender);";
        public static string QueryOwnerAccountForCharacterID = "SELECT barker.accounts.* FROM barker.accounts, barker.characters WHERE barker.characters.CharID = @CID";
    }
}
