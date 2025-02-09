using UnityEngine;
using System.Collections.Generic;

namespace Game.Cards
{
    public class CardPile : MonoBehaviour
    {
        [SerializeField] protected Vector3 _pickedCardOffset;
        
        public List<Card> StockCards = new List<Card>();
        public List<Card> PickedCards = new List<Card>();

        public Vector3 CardPosition => transform.position + _pickedCardOffset;
        
        public virtual Card PickCard()
        {
            var selectedCard = StockCards[0];
            StockCards.RemoveAt(0);

            selectedCard.transform.position = transform.position;
            selectedCard.gameObject.SetActive(true);

            PickedCards.Add(selectedCard);
            return selectedCard;
        }
    }
}