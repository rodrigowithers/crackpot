namespace Game.Cards
{
    public class CrockpotCardPile : CardPile
    {
        public override Card PickCard()
        {
            if (PickedCards.Count > 0)
                return null;
            
            var card = base.PickCard();
            card.CurrentPile = this;
            
            // Disable Card Pile when cards end
            if (StockCards.Count <= 0)
                gameObject.SetActive(false);
            
            return card;
        }
    }
}