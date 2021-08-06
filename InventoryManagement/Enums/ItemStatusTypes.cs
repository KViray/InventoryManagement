using InventoryManagement.Features.Items.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Enums
{
    public enum ItemStatusTypes
    {
        [ItemStatusTypes(ItemStatusTypesDescriptions.RegisterItem)]
        RegisterItem,
        [ItemStatusTypes(ItemStatusTypesDescriptions.UnregisterItem)]
        UnregisterItem,
        [ItemStatusTypes(ItemStatusTypesDescriptions.LendItem)]
        LendItem,
        [ItemStatusTypes(ItemStatusTypesDescriptions.UnlendItem)]
        UnlendItem
    }
    public static class ItemStatusTypesDescriptions
    {
        public const string RegisterItem = "registeritem";
        public const string UnregisterItem = "unregisteritem";
        public const string LendItem = "lenditem";
        public const string UnlendItem = "unlenditem";
    }
}
