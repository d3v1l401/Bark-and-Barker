using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Persistence
{
    public static class InventoryHelpers
    {
        public static Dictionary<ModelInventoryItem, List<ModelProperty>> GetAllUserItems(Database instance, string charId)
        {
            var output = new Dictionary<ModelInventoryItem, List<ModelProperty>>();

            var items = instance.Select<ModelInventoryItem>(ModelInventoryItem.QuerySelectAllItemsForCharacter, new { OID = charId });
            foreach (var item in items)
            {
                var props = instance.Select<ModelProperty>(ModelProperty.QueryGetItemProperties, new { IID = item.UniqueID });

                output.Add(item, props.ToList());
            }

            return output;
        }
    }
}
