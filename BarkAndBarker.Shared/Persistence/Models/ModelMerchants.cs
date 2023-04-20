using BarkAndBarker.Shared.Persistence.Models;

namespace BarkAndBarker.Persistence.Models;

    public class ModelMerchants : IModel
    {

        public string MerchantID;
        public uint Faction;
        public ulong RemainTime;
        public uint isUnidentified;


        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `merchants` (
                                                              `MerchantID` varchar(50) NOT NULL,
                                                              `Faction` int NOT NULL DEFAULT '0',
                                                              `RemainTime` int NOT NULL DEFAULT '86400',
                                                              `isUnidentified` tinyint NOT NULL DEFAULT '0',       
                                                              PRIMARY KEY (`MerchantID`),
                                                              UNIQUE INDEX `MerchantID_UNIQUE` (`MerchantID` ASC) VISIBLE
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly string QueryMerchantList = "SELECT * FROM `barker`.`merchants`;";

        public static readonly List<string> QueryInsertMerchants = new List<string>() {

        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Surgeon');", 
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Woodsman');",  
        "INSERT INTO `barker`.`merchants` (`MerchantID`, `isUnidentified`) VALUES ('DesignDataMerchant:Id_Merchant_GoblinMerchant', '1');",  
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_TheCollector');", 
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Treasurer');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Valentine');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Tailor');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Armourer');", 
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Leathersmith');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Weaponsmith');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Alchemist');", 
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_TavernMaster');",
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_PumpkinMan');", 
        "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Santa');", 

         };
        
        public static readonly int TableCreationOrder = 101;

    }


