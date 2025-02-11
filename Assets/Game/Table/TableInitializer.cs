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
        [SerializeField] private List<CardPile> _crockpotCardPiles;

        [SerializeField] private DeckScriptableObject _deck;

        [Header("Debug")]
        [SerializeField] private int _seed;
        
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
                var card = firstCards[i];
                
                card.transform.position = _tableCardSpaces[i].transform.position;
                card.gameObject.SetActive(true);
                
                _tableCardSpaces[i].Put(card);
            }
            
            for (var i = 0; i < _crockpotCardPiles.Count; i++)
            {
                _crockpotCardPiles[i].StockCards = GetCards(ref _cards, 14);
                
                var initialCard = _crockpotCardPiles[i].PickCard();
                initialCard.OriginalPosition = _crockpotCardPiles[i].transform.position;
            }
            
            int remainingCards = _cards.Count;
            for (var i = 0; i < _mainCardPiles.Count; i++)
            {
                _mainCardPiles[i].StockCards = GetCards(ref _cards, remainingCards / _mainCardPiles.Count);
            }
        }

        private void ShuffleCards(ref List<Card> cards)
        {
            // Fisher-Yates shuffle algorithm, using seed
            if (_seed == 0)
                _seed = Random.Range(0, int.MaxValue);
            
            Random.InitState(_seed);
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
            // Instantiate them and pass them disabled
            List<Card> selectedCards = new List<Card>();
            var cardsToInstantiate = cards.Take(count).ToList();
            for (var i = 0; i < cardsToInstantiate.Count; i++)
            {
                var instance = Instantiate(cardsToInstantiate[i], transform);
                instance.gameObject.SetActive(false);

                selectedCards.Add(instance);
            }

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