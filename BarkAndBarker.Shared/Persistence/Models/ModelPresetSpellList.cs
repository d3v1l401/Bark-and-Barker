using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelPresetSpellList : IModel
    {
        public string ClassID { get; set; }
        public string SpellID { get; set; }

        public static readonly string QuerySelectClassSpells = "SELECT * FROM barker.preset_spell_list WHERE ClassID = @CID";
        public static readonly string QuerySpellList = "SELECT * FROM barker.preset_spell_list;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `barker`.`preset_spell_list` (
                                                          `ClassID` VARCHAR(100) NOT NULL,
                                                          `SpellID` VARCHAR(100) NOT NULL,
                                                          PRIMARY KEY (`ClassID`, `SpellID`));
                                                        ";

        public static readonly List<string> QueryFillPresets = new List<string>()
        {
            
        };

        public static readonly int TableCreationOrder = 93;
    }
}
