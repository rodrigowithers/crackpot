using Game.Deck;
using Game.Cards;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Table
{
    public class TableInitializer : MonoBehaviour
    {
        [SerializeField] private List<CardSpace> _tableCardSpaces;
        [SerializeField] private List<GoalSpace> _tableGoalSpaces;
        [SerializeField] private List<CardPile> _mainCardPiles;

        [SerializeField] private DeckScriptableObject _deck;

        private List<Card> _cards;
        
        // Initializes the table by shuffling all cards
        // selecting 8 of them and adding them to card spaces
        private void InitializeTable()
        {
            _cards = new List<Card>(_deck.Cards);
            ShuffleCards(ref _cards);
            
            var firstCards = GetCards(ref _cards, _tableCardSpaces.Count);
            for (int i = 0; i < firstCards.Count; i++)
            {
                var card = Instantiate(firstCards[i], _tableCardSpaces[i].transform.position, Quaternion.identity);
                _tableCardSpaces[i].Put(card);
            }
            
            int remainingCards = _cards.Count;
            
            for (var i = 0; i < _mainCardPiles.Count; i++)
            {
                _mainCardPiles[i].Cards = GetCards(ref _cards, remainingCards / _mainCardPiles.Count);
            }
        }

        private void ShuffleCards(ref List<Card> cards)
        {
            // Fisher-Yates shuffle algorithm
            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = Random.Range(i, cards.Count);
                (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
            }
        }

        private List<Card> GetCards(ref List<Card> cards, int count)
        {
            if (count > cards.Count)
            {
                Debug.LogError("Not enough cards in the deck!");
                return null;
            }

            // Take first 'count' cards from the shuffled list
            List<Card> selectedCards = cards.Take(count).ToList();

            // Remove the selected cards from the available list
            cards.RemoveRange(0, count);

            return selectedCards;
        }
        
        private void Start()
        {
            InitializeTable();
        }
    }
}