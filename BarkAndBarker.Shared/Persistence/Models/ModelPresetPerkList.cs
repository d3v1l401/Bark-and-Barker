using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelPresetPerkList : IModel
    {
        public string ClassID { get; set; }
        public string PerkID { get; set; }

        public static readonly string QuerySelectClassPerks = "SELECT * FROM barker.preset_perk_list WHERE ClassID = @CID";
        public static readonly string QueryPerkList = "SELECT * FROM barker.preset_perk_list;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `barker`.`preset_perk_list` (
                                                          `ClassID` VARCHAR(100) NOT NULL,
                                                          `PerkID` VARCHAR(100) NOT NULL,
                                                          PRIMARY KEY (`ClassID`, `PerkID`));
                                                        ";

        public static readonly List<string> QueryFillPresets = new List<string>()
        {
            // Fighter
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_AdrenalineSpike');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_Barricade');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_DefenseMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_DualWield');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_ProjectileResistance');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_Swift');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_WeaponMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_Slayer');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_Counterattack');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_ShieldMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataPerk:Id_Perk_ComboAttack');",
        };

        public static readonly int TableCreationOrder = 95;
    }
}
