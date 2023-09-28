public class ShopItemBuyForGold : ShopItemBuy
{
    public override bool Buy()
    {
        if (GameInformation.Instance.Information.Golds >= ItemCost)
        {
            GameInformation.Instance.Information.Golds -= ItemCost;
            return true;
        }
        return false;
    }
}
