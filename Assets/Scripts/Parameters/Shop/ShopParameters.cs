using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopParameters", menuName = "CustomParameters/Shop/ShopParameters")]
public class ShopParameters : ScriptableObject
{
    [SerializeField] private List<ShopItem> _items;
    [SerializeField] private List<Sprite> _moneySprites;

    public List<ShopItem> Items => _items;
}
