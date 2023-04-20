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
        public List<ModelProperty> Properties { get; set; }


        public static readonly string QuerySelectAllItemsForCharacter = "SELECT * FROM `barker`.`inventory_items` WHERE `barker`.`inventory_items`.`OwnerID` = @OID;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `inventory_items` (
                                                              `OwnerID` varchar(45) NOT NULL,
                                                              `UniqueID` int NOT NULL AUTO_INCREMENT,
                                                              `ItemBlueprint` varchar(120) NOT NULL,
                                                              `ItemCount` int NOT NULL DEFAULT '1',
                                                              `InventoryID` int NOT NULL,
                                                              `SlotID` int NOT NULL,
                                                              PRIMARY KEY (`UniqueID`),
                                                              KEY `itemOwner_idx` (`OwnerID`),
                                                              CONSTRAINT `itemOwner` FOREIGN KEY (`OwnerID`) REFERENCES `characters` (`CharID`) ON DELETE CASCADE
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 98;


        // TODO: Move to another helper class, keep Models away from methods
        public static Dictionary<ModelInventoryItem, List<ModelProperty>> GetAllUserItems(Database instance, string charId)
        {
            var output = new Dictionary<ModelInventoryItem, List<ModelProperty>>();

            var items = instance.Select<ModelInventoryItem>(QuerySelectAllItemsForCharacter, new { OID = charId });
            foreach (var item in items)
            {
                var props = instance.Select<ModelProperty>(ModelProperty.QueryGetItemProperties, new { IID = item.UniqueID });

                output.Add(item, props.ToList());
            }

            return output;
        }
    }
}
