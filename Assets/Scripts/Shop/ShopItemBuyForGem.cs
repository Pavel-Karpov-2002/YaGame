public class ShopItemBuyForGem : ShopItemBuy
{
    public override bool Buy()
    {
        if (GameInformation.Instance.Information.Gems >= ItemCost)
        {
            GameInformation.Instance.Information.Gems -= ItemCost;
            return true;
        }
        return false;
    }
}
