using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item")]
public class ItemSO : ScriptableObject
{
    [HeaderAttribute("Item Basic Info")]
    public string itemNamestring;
    public Sprite itemSprite;
    public ItemType itemType;
    [TextArea]
    public string itemCaption;

    [HeaderAttribute("Item Shopping Info")]
    public int buyPrice;
    public int sellPrice;

    [HeaderAttribute("Equipment Info")]
    public int increaseAttack;
    public int increaseMaxHP;
    public int increaseMaxMP;

    [HeaderAttribute("Comsumable Info")]
    public int healHp;
    public int healMp;
}
