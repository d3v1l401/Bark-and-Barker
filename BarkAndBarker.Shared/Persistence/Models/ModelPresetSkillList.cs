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

        public static readonly string QuerySelectClassPerks = "SELECT * FROM barker.preset_skill_list WHERE ClassID = @CID";
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
        };

        public static readonly int TableCreationOrder = 94;
    }
}
