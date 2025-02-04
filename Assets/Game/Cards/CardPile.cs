using System;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Cards
{
    public class CardPile : MonoBehaviour
    {
        public List<Card> Cards = new List<Card>();
        
        public virtual Card PickCard()
        {
            var card = Instantiate(Cards[0], transform.position, Quaternion.identity);
            
            return card;
        }
    }
}