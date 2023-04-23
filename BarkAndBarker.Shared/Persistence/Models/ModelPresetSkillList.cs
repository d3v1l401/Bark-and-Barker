using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelPresetSkillList : IModel
    {
        public string ClassID { get; set; }
        public string SkillID { get; set; }

        public static readonly string QuerySelectClassSkills = "SELECT * FROM barker.preset_skill_list WHERE ClassID = @CID";
        public static readonly string QuerySkillList = "SELECT * FROM barker.preset_skill_list;";

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS `barker`.`preset_skill_list` (
                                                          `ClassID` VARCHAR(100) NOT NULL,
                                                          `SkillID` VARCHAR(100) NOT NULL,
                                                          PRIMARY KEY (`ClassID`, `SkillID`));
                                                        ";

        public static readonly List<string> QueryFillPresets = new List<string>()
        {
            // Fighter
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_PerfectBlock');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_SecondWind');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_AdrenalineRush');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_Breakthrough');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_ShieldSlam');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_Sprint');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_Taunt');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter', 'DesignDataSkill:Id_Skill_VictoryStrike');",
            // Barbarian
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_Rage');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_RecklessAttack');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_SavageRoar');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_WarCry');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_AchillesStrike');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_BloodExchange');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian', 'DesignDataSkill:Id_Skill_LifeSiphon');",
            // Rogue
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_Caltrop');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_Hide');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_Rupture');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_SmokeBomb');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_WeakpointAttack');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_CutThroat');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue', 'DesignDataSkill:Id_Skill_Tumbling');",
            // Ranger
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_FieldRation');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_MultiShot');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_QuickFire');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_QuickShot');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_TrueShot');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_ForcefulShot');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger', 'DesignDataSkill:Id_Skill_PenetratingShot');",
            // Wizard
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSkill:Id_Skill_IntenseFocus');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSkill:Id_Skill_Meditation');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSkill:Id_Skill_SpellMemory1');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSkill:Id_Skill_SpellMemory2');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard', 'DesignDataSkill:Id_Skill_ArcaneShield');",
            // Cleric
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_HolyPurification');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_Judgement');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_Smite');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_SpellMemory1');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_SpellMemory2');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric', 'DesignDataSkill:Id_Skill_DivineProtection');",
            // Bard
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataSkill:Id_Skill_Dissonance');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataSkill:Id_Skill_Encore');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataSkill:Id_Skill_MusicMemory1');",
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Bard', 'DesignDataSkill:Id_Skill_MusicMemory2');",
            // Warlock & Unassigned
            "INSERT INTO `barker`.`preset_skill_list` (`ClassID`, `SkillID`) VALUES ('DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock', 'DesignDataSkill:Id_Skill_SmokePot');",
        };

        public static readonly int TableCreationOrder = 94;
    }
}
