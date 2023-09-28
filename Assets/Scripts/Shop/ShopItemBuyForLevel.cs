public class ShopItemBuyForLevel : ShopItemBuy
{
    public override bool Buy()
    {
        if (GameInformation.Instance.Information.PassedLevel >= ItemCost)
        {
            return true;
        }
        return false;
    }
}
