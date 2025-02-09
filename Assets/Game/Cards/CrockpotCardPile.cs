namespace Game.Cards
{
    public class CrockpotCardPile : CardPile
    {
        public override Card PickCard()
        {
            var card = base.PickCard();

            // Disable Card Pile when cards end
            if (StockCards.Count <= 0)
                gameObject.SetActive(false);
            
            return card;
        }
    }
}