namespace Game.Cards
{
    public class MainCardPile : CardPile
    {
        public override Card PickCard()
        {
            var card = base.PickCard();
            card.OriginalPosition = transform.position + _pickedCardOffset;
            
            return card;
        }
    }
}