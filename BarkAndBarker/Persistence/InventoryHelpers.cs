using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Persistence
{
    public static class InventoryHelpers
    {
        public static SItem MakeSItemObject(ModelInventoryItem inventoryItem, bool withProperties = false, Database instance = null)
        {
            var sItem = new SItem()
            {
                ItemUniqueId = (uint)inventoryItem.UniqueID,
                ItemContentsCount = (uint)inventoryItem.ItemContentsCount,
                SlotId = (uint)inventoryItem.SlotID,
                InventoryId = (uint)inventoryItem.InventoryID,
                ItemCount = (uint)inventoryItem.ItemCount,
                ItemId = inventoryItem.ItemBlueprint,
            };

            if (withProperties)
            {
                if (instance == null)
                    throw new Exception("Can not fetch properties without a db session");

                var props = instance.Select<ModelProperty>(ModelProperty.QueryGetItemProperties, new { IID = inventoryItem.UniqueID }).ToArray();
                if (props.Count() > 4)
                    throw new Exception("Illegal item properties count (" + inventoryItem.UniqueID + ")");

                for (var i = 0; i < props.Count(); i++)
                {
                    if (i < 2)
                        sItem.PrimaryPropertyArray.Add(new SItemProperty()
                        {
                            PropertyTypeId = props[i].PropertyID,
                            PropertyValue = props[i].PropertyValue
                        });
                    else
                        sItem.SecondaryPropertyArray.Add(new SItemProperty()
                        {
                            PropertyTypeId = props[i].PropertyID,
                            PropertyValue = props[i].PropertyValue
                        });
                }
            }

            return sItem;
        }

        public static Dictionary<ModelInventoryItem, List<ModelProperty>> GetAllUserItems(Database instance, string charId, bool withProperties)
        {
            var output = new Dictionary<ModelInventoryItem, List<ModelProperty>>();

            var items = instance.Select<ModelInventoryItem>(ModelInventoryItem.QuerySelectAllItemsForCharacter, new { OID = charId });
            if (withProperties)
            {
                foreach (var item in items)
                {
                    var props = instance.Select<ModelProperty>(ModelProperty.QueryGetItemProperties, new { IID = item.UniqueID });

                    output.Add(item, props.ToList());
                }
            } else
                foreach (var item in items)
                    output.Add(item, null);

            return output;
        }
    }
}
