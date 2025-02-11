using UnityEngine;

namespace Game.Cards
{
    [SelectionBase]
    public class MainCardPile : CardPile
    {
        public override Card PickCard()
        {
            var card = base.PickCard();
            card.OriginalPosition = transform.position + _pickedCardOffset;
            card.CurrentPile = this;
            
            // Check if we picked the last card from Stock
            if (StockCards.Count <= 0)
            {
                // Here, iterate through all picked cards, check if they are played
                // Played cards are either OnGoal or have an active CardSpace

                for (var i = 0; i < PickedCards.Count; i++)
                {
                    if(PickedCards[i].OnGoal)
                        PickedCards.Remove(PickedCards[i]);
                    
                    if(PickedCards[i].CurrentCardSpace != null)
                        PickedCards.Remove(PickedCards[i]);
                }
                
                // Now, put back Picked Cards into Stock Cards
                StockCards.AddRange(PickedCards);
                
                for (var i = 0; i < PickedCards.Count; i++)
                {
                    PickedCards[i].gameObject.SetActive(false);
                }
                
                PickedCards.Clear();
            }
            
            return card;
        }
    }
}