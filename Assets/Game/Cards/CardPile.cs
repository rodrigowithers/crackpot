using UnityEngine;
using System.Collections.Generic;

namespace Game.Cards
{
    public class CardPile : MonoBehaviour
    {
        [SerializeField] protected Vector3 _pickedCardOffset;
        
        public List<Card> Cards = new List<Card>();

        public Vector3 CardPosition => transform.position + _pickedCardOffset;
        
        public virtual Card PickCard()
        {
            var cardToInstantiate = Cards[0];
            Cards.RemoveAt(0);
            
            var card = Instantiate(cardToInstantiate, transform.position, Quaternion.identity);
            return card;
        }
    }
}