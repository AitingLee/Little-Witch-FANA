using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { consumable, equipment, loot, gem }
public enum ItemName
{
    empty, apple, roastChicken, carrot, broccoli, honey, shavedIce, rubyRing, agateNecklace, jadeEarrings, silverRing, silverNecklace, silverEarrings,
    jar, silk, wood, rock, ice, popsicle, meat, egg, flower, weeds, amethyst, cryolite, moonStone, money
}

public class ItemManager : MonoBehaviour
{

    private static ItemManager _instance;
    public static ItemManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public ItemSO empty, apple, roastChicken, carrot, broccoli, honey, shavedIce, rubyRing, agateNecklace, jadeEarrings, silverRing, silverNecklace, silverEarrings,
                  jar, silk, wood, rock, ice, popsicle, meat, egg, flower, weeds, amethyst, cryolite, moonStone, money;

    public TeleportPlatform[] teleportPlatforms;
    public Totem[] totems;
    public TreasureChest[] treasureChests;
    public ItemSO GetItemSO(ItemName itemName)
    {
        switch (itemName)
        {
            case ItemName.empty:
                return empty;
            case ItemName.apple:
                return apple;
            case ItemName.roastChicken:
                return roastChicken;
            case ItemName.carrot:
                return carrot;
            case ItemName.broccoli:
                return broccoli;
            case ItemName.honey:
                return honey;
            case ItemName.shavedIce:
                return shavedIce;
            case ItemName.rubyRing:
                return rubyRing;
            case ItemName.agateNecklace:
                return agateNecklace;
            case ItemName.jadeEarrings:
                return jadeEarrings;
            case ItemName.silverRing:
                return silverRing;
            case ItemName.silverNecklace:
                return silverNecklace;
            case ItemName.silverEarrings:
                return silverEarrings;
            case ItemName.jar:
                return jar;
            case ItemName.silk:
                return silk;
            case ItemName.wood:
                return wood;
            case ItemName.rock:
                return rock;
            case ItemName.ice:
                return ice;
            case ItemName.popsicle:
                return popsicle;
            case ItemName.meat:
                return meat;
            case ItemName.egg:
                return egg;
            case ItemName.flower:
                return flower;
            case ItemName.weeds:
                return weeds;
            case ItemName.amethyst:
                return amethyst;
            case ItemName.cryolite:
                return cryolite;
            case ItemName.moonStone:
                return moonStone;
            case ItemName.money:
                return money;
        }
        return null;
    }


}
