using Godot;

public class ItemHelper
{
    public static Texture2D TextureFromItem(ClosetItemType itemType)
    {
        string itemName = "";
        if (itemType == ClosetItemType.PILLZ)
        {
            itemName = "Pills";
        }
        if (itemType == ClosetItemType.SERINGE)
        {
            itemName = "Syringe";
        }
        if (itemType == ClosetItemType.BANDAGE)
        {
            itemName = "BandAid";
        }
        return (Texture2D)GD.Load($"res://Assets/items/Asset_PickUp_{itemName}.png");
    }
}