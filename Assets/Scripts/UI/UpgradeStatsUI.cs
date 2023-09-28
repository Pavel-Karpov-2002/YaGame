public class UpgradeStatsUI : UpgradeUICard
{
    public override void UpdateInformation()
    {
        if (Type == UpgradeTypeParameters.Int)
        {
            InfoText.text = CardInformation.LastValue;
            return;
        }

        InfoText.text = System.Math.Round(System.Convert.ToDouble(CardInformation.LastValue), DeafaultNumOfDecimalPlaces).ToString();
    }
}
