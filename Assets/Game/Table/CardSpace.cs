using Game.Cards;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Table
{
    public class CardSpace : MonoBehaviour
    {
        [SerializeField] private Vector2 _cardStackDirection = Vector2.right;
        
        public List<Card> Cards = new();
        
        public Card TopCard => Cards.Count > 0 ? Cards[^1] : null;

        public Vector2 TopPosition
        {
            get
            {
                if (Cards.Count == 0)
                    return transform.position;

                return (Vector2)transform.position + _cardStackDirection * (Cards.Count * 0.2f);
            }
        }

        public void Put(Card card)
        {
            Cards.Add(card);
            card.CurrentCardSpace = this;
        }

        public void Take(Card card)
        {
            Cards.Remove(card);
        }
    }
}