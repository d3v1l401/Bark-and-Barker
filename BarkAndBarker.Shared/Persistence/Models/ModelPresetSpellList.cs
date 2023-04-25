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
            // Wizard
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Zap');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Light');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Lazy');", // Slow?
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_IceBolt');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Ignite');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_MagicMissile');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Haste');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_LightningStrike');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Invisibility');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_Fireball');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSpell:Id_Spell_ChainLightning');",
            // Cleric
            
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Protection');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Bless');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_DivineStrike');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Cleanse');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_LesserHeal');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_BindEvil');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_HolyStrike');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_HolyLight');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Sanctuary');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Resurrection');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_LocustsSwarm');",
            "INSERT INTO `barker`.`preset_spell_list` (`ClassID`, `SpellID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSpell:Id_Spell_Earthquake');",
        };

        public static readonly int TableCreationOrder = 93;
    }
}
