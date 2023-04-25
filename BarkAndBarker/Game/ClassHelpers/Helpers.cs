using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Game
{
    public static class CreationHelpers
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElementUsing<T>(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
    public static class ClassHelpers
    {
        public static bool OnCharacterCreation(ModelCharacter createdCharacter, Database dbInstance)
        {
            var characterClassPerks = dbInstance.Select<ModelPresetPerkList>(ModelPresetPerkList.QuerySelectClassPerks, new { CID = createdCharacter.Class });
            // create 4 perks slot, assign the first one a random perk
            var randomPerk = characterClassPerks.RandomElement();
            for (var perkIndex = 1; perkIndex < 5; perkIndex++)
            {
                var minLevelSlot = 1;
                if (perkIndex != 1)
                    minLevelSlot = (perkIndex - 1) * 5;

                var createdSlotInstances = dbInstance.Execute(ModelPerks.QueryCreateSlots, new
                {
                    CID = createdCharacter.CharID,
                    Index = perkIndex,
                    MinLevel = minLevelSlot,
                    PerkID = perkIndex == 1 ? randomPerk.PerkID : null,
                    Type = 1,
                });

                if (createdSlotInstances < 0)
                    throw new Exception("Could not create perk slots for " +  createdCharacter.CharID);
            }

            var characterClassSkills = dbInstance.Select<ModelPresetSkillList>(ModelPresetSkillList.QuerySelectClassSkills, new { CID = createdCharacter.Class });
            for (var skillIndex = 5; skillIndex < 7; skillIndex++)
            {
                var randomSkill = characterClassSkills.RandomElement();
                var createdSlotInstances = dbInstance.Execute(ModelPerks.QueryCreateSlots, new
                {
                    CID = createdCharacter.CharID,
                    Index = skillIndex,
                    MinLevel = 1,
                    PerkID = randomSkill.SkillID,
                    Type = 2,
                });

                if (createdSlotInstances < 0)
                    throw new Exception("Could not create skill slots for " + createdCharacter.CharID);
            }

            // Create the default kit for the class
            switch (createdCharacter.Class)
            {
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Barbarian":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Rogue":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Ranger":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Cleric":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Wizard":
                    break;
                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Bard":
                    break;

                case "DesignDataPlayerCharacter:Id_PlayerCharacter_Warlock":
                default:
                    break;
            }

            return true;
        }
    }
}
