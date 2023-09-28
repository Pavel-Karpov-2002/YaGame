using UnityEngine;

public class PlayerItemUpdate : MonoBehaviour
{
    [SerializeField] private Skin _skin;
    [SerializeField] private Hand _hand;

    public void SetItem(ShopItem item, int itemNum)
    {
        if (item.ShopItemBuy.GetComponent<Skin>() != null)
        {
            SetSkin(item);
            GameInformation.Instance.Information.SkinEquip = itemNum;
            GameInformation.OnInformationChange?.Invoke();
            return;
        }

        if (item.ShopItemBuy.GetComponent<Weapon>() != null)
        {
            if (_hand.Weapon != null)
                Destroy(_hand.Weapon.gameObject);

            SetWeapon(item.ShopItemBuy.gameObject);
            GameInformation.Instance.Information.WeaponEquip = itemNum;
            GameInformation.OnInformationChange?.Invoke();
            return;
        }
    }

    private void SetWeapon(GameObject item)
    {
        if (item == null)
            return;

        GameObject weapon = Instantiate(item);
        weapon.transform.parent = _hand.transform;
        weapon.transform.localScale = Vector3.one;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        SetObjectLayer(weapon, gameObject.layer);
        _hand.SetHandWeapon(weapon.GetComponent<Weapon>());
    }

    private void SetSkin(ShopItem item)
    {
        Skin skin = item.ShopItemBuy.GetComponent<Skin>();
        ChangeClothes(skin);
    }

    private void ChangeClothes(Skin skin)
    {
        for (int i = 0; i < _skin.Clothes.Count; i++)
        {
            if (skin.Clothes.Count > i)
            {
                _skin.Clothes[i].sharedMesh = skin.Clothes[i].sharedMesh;
                _skin.Clothes[i].sharedMaterials = skin.Clothes[i].sharedMaterials;
            }
            else
                _skin.Clothes[i].sharedMesh = null;
        }
    }

    private void SetObjectLayer(GameObject item, int layer)
    {
        foreach (var obj in item.GetComponentsInChildren<Transform>())
        {
            obj.gameObject.layer = layer;
        }
    }
}
