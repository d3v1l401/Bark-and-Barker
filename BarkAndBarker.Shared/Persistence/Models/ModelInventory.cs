using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelInventoryItem : IModel
    {
        public int OwnerID { get; set; }
        public long UniqueID { get; set; }
        public string ItemBlueprint { get; set; }
        public List<ModelProperty> Properties { get; set; }

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `inventory_items` (
                                                              `OwnerID` varchar(45) NOT NULL,
                                                              `UniqueID` INT NOT NULL DEFAULT AUTO_INCREMENT,
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
    }
}
