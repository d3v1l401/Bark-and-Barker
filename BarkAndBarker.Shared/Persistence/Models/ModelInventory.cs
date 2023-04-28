using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelInventoryItem : IModel
    {
        public string OwnerID { get; set; }
        public long UniqueID { get; set; }
        public string ItemBlueprint { get; set; }
        public int ItemCount { get; set; }
        public int InventoryID { get; set; }
        public int SlotID { get; set; }
        public int ItemContentsCount { get; set; }
        public int ItemAmmoCount { get; set; }
        public List<ModelProperty> Properties { get; set; }


        public static readonly string QuerySelectAllItemsForCharacter = "SELECT * FROM `barker`.`inventory_items` WHERE `barker`.`inventory_items`.`OwnerID` = @OID;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `inventory_items` (
                                                              `OwnerID` varchar(45) NOT NULL,
                                                              `UniqueID` int NOT NULL AUTO_INCREMENT,
                                                              `ItemBlueprint` varchar(120) NOT NULL,
                                                              `ItemCount` int NOT NULL DEFAULT '1',
                                                              `InventoryID` int NOT NULL,
                                                              `SlotID` int NOT NULL,
                                                              `ItemContentsCount` int,
                                                              `ItemAmmoCount` int,
                                                              PRIMARY KEY (`UniqueID`),
                                                              KEY `itemOwner_idx` (`OwnerID`),
                                                              CONSTRAINT `itemOwner` FOREIGN KEY (`OwnerID`) REFERENCES `characters` (`CharID`) ON DELETE CASCADE
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly string QueryUpdateInventory = "UPDATE `barker`.`inventory_items` SET `SlotID` = @SlotID," +
                                                              "`ItemBlueprint` = @ItemBlueprint, `ItemCount` = @ItemCount," +
                                                              "`ItemContentsCount` = @ItemContentsCount, `ItemAmmoCount` = @ItemAmmoCount, " +
                                                              "`InventoryID` = @InventoryID WHERE (`UniqueID` = @UniqueID);\r\n";

        public static readonly string QueryDeleteInventoryGold = "DELETE FROM `barker`.`inventory_items` WHERE (`UniqueID` = @UniqueID);";
        public static readonly string QueryInventoryGold = "SELECT * FROM `barker`.`inventory_items` WHERE (`UniqueID` = @UniqueID);";
        public static readonly string QueryUpdateInventoryGold = "UPDATE `barker`.`inventory_items` SET `ItemCount` = @ItemCount, `SlotID` = @SlotID WHERE (`UniqueID` = @UniqueID);";
        public static readonly string QueryInsertInventoryGold = "INSERT INTO `barker`.`inventory_items` (`OwnerID`, `UniqueID`, `ItemBlueprint`, `ItemCount`, `InventoryID`, `SlotID`) VALUES (@OwnerID, @UniqueID, @ItemID, @ItemCount, @InventoryID, @SlotID);";

        public static readonly int TableCreationOrder = 98;
    }
}
