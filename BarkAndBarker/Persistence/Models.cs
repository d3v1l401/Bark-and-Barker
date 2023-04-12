using BarkAndBarker.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    interface IModel {}

    public class DBGeneral
    {
        private static readonly string SchemaName = "barker"; // Editing this means you're going to break all the queries below.

        public static readonly string QuerySelectDBMSVersion = "SELECT @@VERSION;";
        public static readonly string QuerySelectSchemas = "SHOW DATABASES;";
        public static readonly string QueryCreateSchema = "CREATE DATABASE " + SchemaName;

        public static void CheckAndCreateDatabase(Database databaseIntance)
        {
            if (databaseIntance == null)
                throw new Exception("database instance not parsed");

            // Get all implementors of IModel
            var modelsClasses = typeof(IModel).GetImplementors(Assembly.GetExecutingAssembly()).Reverse();
            foreach (var model in modelsClasses)
            {
                // All table models should start with "Model*"
                if (model.Name.StartsWith("Model"))
                {
                    // Get the QueryCreateTable static field
                    var createQuery = model.GetMembers().First(x => x.Name == "QueryCreateTable");
                    string? queryExecute = createQuery.GetValue(createQuery) as string; // Gets its value, THIS ONLY WORKS FOR STATIC FIELDS
                    if (queryExecute != null)
                    {
                        databaseIntance.Execute(queryExecute, null); // Execute the creation, if the table doesn't exists.
                    }
                }
            }
        }
    }
}
