namespace BarkAndBarker.Persistence.Models;

    internal class ModelMerchants : IModel
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
        public static readonly string QueryInsertMerchants = "INSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Surgeon');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Woodsman');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_GoblinMerchant');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Treasurer');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_TheCollector');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Valentine');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Tailor');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Armourer');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Leathersmith');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Alchemist');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_Weaponsmith');\r\nINSERT INTO `barker`.`merchants` (`MerchantID`) VALUES ('DesignDataMerchant:Id_Merchant_TavernMaster');\r\n";

        
        public static readonly int TableCreationOrder = 101;

    }


