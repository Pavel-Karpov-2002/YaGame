using System;
using UnityEngine;

[Serializable]
public class ShopItem
{
    [SerializeField] private string _wordsKeyTranslatorText;
    [SerializeField] private ShopItemBuy _shopItemBuy;

    public string WordsKeyTranslatorText => _wordsKeyTranslatorText;
    public ShopItemBuy ShopItemBuy => _shopItemBuy;
}

public enum ItemMoneyType
{
    Gem,
    Gold,
    Level
} 