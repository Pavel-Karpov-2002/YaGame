using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopParameters _shopParameters;
    [SerializeField] private ShopItemCard _itemCard;
    [SerializeField] private ItemsPanel _itemsPanel;
    [SerializeField] private PlayerItemUpdate _character;
    [SerializeField] private string _isEquipedKeyText;
    [SerializeField] private string _isEquipKeyText;
    [SerializeField] private ShopType _shopType;

    private delegate void ButtonClickDelegate();

    private int _lastEquipedButton;

    public ShopParameters ShopParameters => _shopParameters;

    private void Awake()
    {
        CreateItemCards();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void CreateItemCards()
    {
        for (int i = 0; i < _shopParameters.Items.Count; i++)
        {
            int saveI = i;

            ShopItemCard card = Instantiate(_itemCard);
            card.transform.SetParent(_itemsPanel.transform);
            SetParametersObject(card.gameObject, card.transform.localRotation, Vector3.zero, Vector3.one);
            card.ItemButton.onClick.AddListener(() => BuyItem(saveI));
            card.CostText.text = _shopParameters.Items[i].ShopItemBuy.ItemCost.ToString();
            card.TextTranslator.key = _shopParameters.Items[i].WordsKeyTranslatorText;

            GameObject objectDemonstration = Instantiate(_shopParameters.Items[i].ShopItemBuy.gameObject);
            objectDemonstration.transform.SetParent(card.ItemDemonstrationObject.transform);
            SetParametersObject(objectDemonstration, 
                _shopParameters.Items[i].ShopItemBuy.transform.localRotation,
                Vector3.zero, 
                new Vector3(
                _shopParameters.Items[i].ShopItemBuy.transform.localScale.x, 
                _shopParameters.Items[i].ShopItemBuy.transform.localScale.y, 
                _shopParameters.Items[i].ShopItemBuy.transform.localScale.z));

            if (_shopType == ShopType.Weapon)
                card.GetComponent<WeaponInfoUI>().DemonstrationWeaponInformation(_shopParameters.Items[i].ShopItemBuy.GetComponent<Weapon>());

            card.MoneyImage.sprite = _shopParameters.Items[i].ShopItemBuy.MoneySprites;
        }
    }

    private void SetParametersObject(GameObject gameObj,Quaternion localRotation, Vector3 localPos, Vector3 localScale)
    {
        gameObj.transform.localRotation = localRotation;
        gameObj.transform.localPosition = localPos;
        gameObj.transform.localScale = localScale;
    }

    public void Equip(int itemNum)
    {
        _character.SetItem(_shopParameters.Items[itemNum], itemNum);
        _itemsPanel.ShopItemCards[_lastEquipedButton].ItemButton.interactable = true;
        _itemsPanel.ShopItemCards[itemNum].ItemButton.interactable = false;
        ChangeItemPanelButtonText(_lastEquipedButton, _isEquipKeyText);
        ChangeItemPanelButtonText(itemNum, _isEquipedKeyText);
        _lastEquipedButton = itemNum;
    }

    public void UnlockItems(List<int> items)
    {
        foreach (int itemNum in items)
            Unlockitem(itemNum, _isEquipKeyText);
    }

    private void BuyItem(int itemNum)
    {
        if (!_shopParameters.Items[itemNum].ShopItemBuy.Buy())
            return;

        switch (_shopType)
        {
            case ShopType.Skin:
                GameInformation.Instance.Information.SkinsBought.Add(itemNum);
                break;

            case ShopType.Weapon:
                GameInformation.Instance.Information.WeaponsBought.Add(itemNum);
                break;
        }

        ChangeButtonClick(_itemsPanel.ShopItemCards[itemNum].ItemButton, () => Equip(itemNum));
        Unlockitem(itemNum, _isEquipKeyText);
        GameInformation.OnInformationChange?.Invoke();
    }

    private void Unlockitem(int itemNum, string buttonText)
    {
        _itemsPanel.ShopItemCards[itemNum].CostText.alpha = 0;
        _itemsPanel.ShopItemCards[itemNum].MoneyImage.color = Color.clear;
        ChangeButtonClick(_itemsPanel.ShopItemCards[itemNum].ItemButton, () => Equip(itemNum));
        ChangeItemPanelButtonText(itemNum, buttonText);
    }

    private void ChangeItemPanelButtonText(int itemNum, string text)
    {
        _itemsPanel.ShopItemCards[itemNum].ButtonKeyText.key = text;
        _itemsPanel.ShopItemCards[itemNum].ButtonKeyText.ReTranslate();
    }

    private void ChangeButtonClick(Button button, ButtonClickDelegate click)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => click?.Invoke());
    }
}

public enum ShopType
{
    Skin,
    Weapon
}
