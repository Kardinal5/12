﻿namespace ExtendedItemDataFramework
{
    public class CrafterNameData : BaseExtendedItemComponent
    {
        public const string TypeID = "c";

        public string CrafterName = "";

        public CrafterNameData(ExtendedItemData parent) 
            : base(TypeID, parent)
        {
        }

        public override string Serialize()
        {
            return CrafterName;
        }

        public override void Deserialize(string data)
        {
            CrafterName = data;
        }

        public override BaseExtendedItemComponent Clone()
        {
            return MemberwiseClone() as BaseExtendedItemComponent;
        }
    }

    public static partial class ItemDataExtensions
    {
        public static string GetCrafterName(this ItemDrop.ItemData itemData)
        {
            if (itemData is ExtendedItemData extendedItemData)
            {
                return extendedItemData.GetComponent<CrafterNameData>().CrafterName;
            }
            else
            {
                return itemData.m_crafterName;
            }
        }

        public static void SetCrafterName(this ItemDrop.ItemData itemData, string crafterName)
        {
            if (itemData is ExtendedItemData extendedItemData)
            {
                extendedItemData.GetComponent<CrafterNameData>().CrafterName = crafterName;
            }
            else
            {
                itemData.m_crafterName = crafterName;
            }
        }
    }
}
