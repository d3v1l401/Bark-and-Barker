using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelPerks : IModel
    {
        public string OwnerID { get; set; }
        public int Index { get; set; }
        public int IsAvailableSlot { get; set; }
        public int RequiredLevel { get; set; }
        public string EquipID { get; set; }
        public int Type { get; set; } // 1 - Perk; 2 - Skill

        public static readonly string QuerySelectCharacterSkills = "SELECT * FROM barker.character_perks WHERE OwnerID = @CID";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `barker`.`character_perks` (
                                                          `OwnerID` VARCHAR(45) NOT NULL,
                                                          `Index` INT NOT NULL,
                                                          `IsAvailableSlot` INT NOT NULL DEFAULT 1,
                                                          `RequiredLevel` INT NOT NULL DEFAULT 1,
                                                          `EquipID` VARCHAR(100) NOT NULL,
                                                          `Type` INT NOT NULL DEFAULT 1,
                                                          PRIMARY KEY (`OwnerID`, `Index`),
                                                          CONSTRAINT `ownerChar`
                                                            FOREIGN KEY (`OwnerID`)
                                                            REFERENCES `barker`.`characters` (`CharID`)
                                                            ON DELETE NO ACTION
                                                            ON UPDATE NO ACTION);";

        public static readonly int TableCreationOrder = 96;
    }
}
