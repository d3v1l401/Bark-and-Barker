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
            // Rogue
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_DaggerMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Ambush');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_DaggerExpert');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_BackAttack');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Creep');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_HiddenPocket');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_LockpickingMastery');", // ?
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_LockpickSet');", // ?
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Pickpocket');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_PoisonedWeapon');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Stealth');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_TrapDetection');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_DoubleJump');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Jokester');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_ShadowRunner');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataPerk:Id_Perk_Thrust');",
            // Barbarian
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_AxeSpecialization');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Berserker');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Carnage');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_IronWill');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_MoraleBoost');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Savage');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Crush');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Robust');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_TwoHander');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_PotionChugger');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataPerk:Id_Perk_Executioner');",
            // Ranger
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_CrossbowMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_EnhancedHearing');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_Kinesthesia');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_NimbleHands');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_RangedWeaponsExpert');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_RangedWeaponsMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_Sharpshooter');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_SpearProficiency');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_Tracking');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_TrapExpert');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_CripplingShot');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_QuickReload');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_Chase');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataPerk:Id_Perk_TrapMastery');",
            // Wizard
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ArcaneFeedback');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ArcaneMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ArcaneFeedback');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ArcaneMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_FireMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_IceShield');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ManaSurge');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_Melt');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_QuickChant');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_ReactiveShield');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataPerk:Id_Perk_Sage');",
            // Cleric
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_AdvancedHealer');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_BluntWeaponMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_Brewmaster');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_Kindness');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_Perseverance');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_ProtectionfromEvil');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_Requiem');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_UndeadSlaying');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataPerk:Id_Perk_HolyAura');",
            // Bard
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataPerk:Id_Perk_LoreMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataPerk:Id_Perk_MelodicProtection');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataPerk:Id_Perk_RapierMastery');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataPerk:Id_Perk_WanderersLuck');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataPerk:Id_Perk_WarSong');",
            // Warlock & Unassigned
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_DefenseExpert');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_Malice');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_ShieldExpert');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_Smash');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_SurvivalistTongue');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_Toughness');",
            "INSERT INTO `barker`.`preset_perk_list` (`ClassID`, `PerkID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataPerk:Id_Perk_TwoHandedWeaponExpert');",
        };

        public static readonly int TableCreationOrder = 95;
    }
}
