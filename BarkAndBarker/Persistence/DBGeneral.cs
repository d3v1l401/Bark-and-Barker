﻿using BarkAndBarker.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using BarkAndBarker.Persistence.Models;

namespace BarkAndBarker.Persistence
{
    internal static class DBExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class DBGeneral
    {
        private static readonly string
            SchemaName = "barker"; // Editing this means you're going to break all the queries below.

        public static readonly string QuerySelectDBMSVersion = "SELECT @@VERSION;";
        public static readonly string QuerySelectSchemas = "SHOW DATABASES;";
        public static readonly string QueryCreateSchema = "CREATE DATABASE " + SchemaName;

        public static void CheckAndCreateDatabase(Database databaseIntance)
        {
            if (databaseIntance == null)
                throw new Exception("database instance not parsed");
           
            // Get all implementors of IModel
            // All table models should start with "Model*"

            var modelsClasses = typeof(IModel).GetImplementors(Assembly.GetAssembly(typeof(IModel))).Reverse();
            foreach (var model in modelsClasses
                         .Where(el => el.Name.StartsWith("Model"))
                         .OrderByDescending(el => (int)el.GetMember(nameof(IModel.TableCreationOrder)).First().GetValue(el)))
            {
                // Get the QueryCreateTable static field
                var createQuery = model.GetMembers().First(x => x.Name == nameof(IModel.QueryCreateTable));
                string? queryExecute =
                    createQuery.GetValue(createQuery) as string; // Gets its value, THIS ONLY WORKS FOR STATIC FIELDS
                if (queryExecute != null)
                {
                    databaseIntance.Execute(queryExecute, null); // Execute the creation, if the table doesn't exists.
                }
            }


            // Check & Insert Merchants 

            var merchants = databaseIntance.Select<ModelMerchants>(ModelMerchants.QueryMerchantList, null);

            if (merchants.Count() <= 0)
            {
                foreach (var query in ModelMerchants.QueryInsertMerchants)
                {
                    databaseIntance.Execute(query, null);
                }
            }


            // Check & insert skill, spell and perks class presets
            var perksList = databaseIntance.Select<ModelPresetPerkList>(ModelPresetPerkList.QueryPerkList, null);
            if (perksList.Count() <= 0)
                foreach (var query in ModelPresetPerkList.QueryFillPresets)
                    databaseIntance.Execute(query, null);

            var skillsList = databaseIntance.Select<ModelPresetSkillList>(ModelPresetSkillList.QuerySkillList, null);
            if (skillsList.Count() <= 0)
                foreach (var query in ModelPresetSkillList.QueryFillPresets)
                    databaseIntance.Execute(query, null);

            var spellsList = databaseIntance.Select<ModelPresetSpellList>(ModelPresetSpellList.QuerySpellList, null);
            if (spellsList.Count() <= 0)
                foreach (var query in ModelPresetSpellList.QueryFillPresets)
                    databaseIntance.Execute(query, null);

        }
    }
}