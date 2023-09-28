using UnityEngine;

[DisallowMultipleComponent]
public abstract class ShopItemBuy : MonoBehaviour
{
    [SerializeField] private Sprite _moneySprites;
    [SerializeField] private int _itemCost;

    public Sprite MoneySprites => _moneySprites;
    public int ItemCost => _itemCost;

    public abstract bool Buy();
}
