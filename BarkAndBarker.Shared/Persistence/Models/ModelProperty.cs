using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelProperty : IModel
    {
        public long ItemID { get; set; }
        public string PropertyID { get; set; }
        public short PropertyValue { get; set; }

        public static readonly string QueryGetItemProperties = "SELECT * FROM barker.item_properties WHERE barker.item_properties.ItemID = @IID;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `item_properties` (
                                                              `ItemID` int NOT NULL,
                                                              `PropertyID` varchar(150) NOT NULL DEFAULT 'DesignDataItemPropertyType:Id_ItemPropertyType_Effect_ArmorRating',
                                                              `PropertyValue` int NOT NULL DEFAULT '0',
                                                              PRIMARY KEY (`ItemID`,`PropertyID`),
                                                              CONSTRAINT `affectedItem` FOREIGN KEY (`ItemID`) REFERENCES `inventory_items` (`UniqueID`) ON DELETE CASCADE
                                                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 97;
    }
}
