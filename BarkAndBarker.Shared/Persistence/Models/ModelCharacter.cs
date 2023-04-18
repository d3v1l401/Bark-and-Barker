namespace BarkAndBarker.Shared.Persistence.Models;

public class ModelCharacter : IModel
{
    public int accountID; // Foreign Key, PK
    public string CharID; // PK
    public string Nickname;
    public string Class;
    public int Level;
    public DateTime CreatedAt;
    public int Gender;
    public DateTime LastLogin;
    public int KarmaScore;

    public static readonly string QuerySelectAllCharacters = "SELECT * FROM barker.characters;";
    public static readonly string QuerySelectAllByUserAccount = "SELECT * FROM barker.characters WHERE barker.characters.accountID = @AID AND barker.characters.IsDeleted IS NULL;";
    public static readonly string QuerySelectAllNicknames = "SELECT COUNT(*) FROM barker.characters WHERE barker.characters.Nickname = @Nickname;";
    public static readonly string QuerySelectCharacterByID = "SELECT * FROM barker.characters WHERE barker.characters.CharID = @CID AND barker.characters.IsDeleted IS NULL;";
    public static readonly string QueryCreateCharacter = "INSERT INTO barker.characters (`accountID`, `CharID`, `Nickname`, `Class`, `Level`, `Gender`) VALUES (@AID, @CID, @Nickname, @Class, @Level, @Gender);";
    public static readonly string QueryOwnerAccountForCharacterID = "SELECT barker.accounts.* FROM barker.accounts, barker.characters WHERE barker.characters.CharID = @CID"; // do not check if IsDeleted is NULL, this is used for account <-> char ownership checks.
    public static readonly string QueryDeleteCharacter = "UPDATE barker.characters SET barker.characters.IsDeleted = CURRENT_TIMESTAMP WHERE (`accountID` = @AID) and (`CharID` = @CID);";

#if USE_STEAM
    public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `characters` (
                                                              `CharID` varchar(45) NOT NULL,
                                                              `accountID` varchar(50) NOT NULL,
                                                              `Nickname` varchar(20) NOT NULL,
                                                              `Class` varchar(60) NOT NULL,
                                                              `Level` int NOT NULL DEFAULT '1',
                                                              `CreatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `Gender` int NOT NULL,
                                                              `LastLogin` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `KarmaScore` int NOT NULL DEFAULT '0',
                                                              `IsDeleted` timestamp NULL DEFAULT NULL,
                                                              PRIMARY KEY (`CharID`),
                                                              CONSTRAINT `charOwner` FOREIGN KEY (`accountID`) REFERENCES `accounts` (`SteamID`)
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
#else
    public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `characters` (
                                                              `CharID` varchar(45) NOT NULL,
                                                              `accountID` INT NOT NULL,
                                                              `Nickname` varchar(20) NOT NULL,
                                                              `Class` varchar(60) NOT NULL,
                                                              `Level` int NOT NULL DEFAULT '1',
                                                              `CreatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `Gender` int NOT NULL,
                                                              `LastLogin` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                              `KarmaScore` int NOT NULL DEFAULT '0',
                                                              `IsDeleted` timestamp NULL DEFAULT NULL,
                                                              PRIMARY KEY (`CharID`),
                                                              CONSTRAINT `charOwner` FOREIGN KEY (`accountID`) REFERENCES `accounts` (`ID`)
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
#endif

    public static readonly int TableCreationOrder = 10;
}