using System;
using DG.Tweening;
using Game.Table;
using UnityEngine;

namespace Game.Cards
{
    [SelectionBase]
    public class Card : MonoBehaviour
    {
        public enum CardColor
        {
            Red,
            Blue
        }

        public enum CardSuit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }
        
        [Header("Config")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CardColor _cardColor;
        [SerializeField] private CardSuit _cardSuit;
        [SerializeField] private int _number;
        
        public CardSpace CurrentCardSpace { get; set; }
        
        public CardColor Color => _cardColor;
        public CardSuit Suit => _cardSuit;
        
        public Vector3 OriginalPosition { get; set; }
        public CardSpace OriginalCardSpace { get; set; }
        public CardPile CurrentPile { get; set; }
        
        public int Number => _number;
        public int OrderInLayer => _spriteRenderer.sortingOrder;

        /// <summary>
        /// Cards On Goal cannot be moved
        /// </summary>
        public bool OnGoal { get; set; }

        public int LastOrderInLayer { get; set; }


        public void Pick()
        {
            if (CurrentCardSpace != null)
            {
                // If card is in a card space, clear it from there
                CurrentCardSpace.Take(this);
            }
            
            CurrentCardSpace = null;
            transform.DOKill(true);

            transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutBack);
            _spriteRenderer.sortingOrder = 999;
        }

        public void Drop(int newSortingOrder = 0)
        {
            transform.DOKill(true);
        
            transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            _spriteRenderer.sortingOrder = newSortingOrder;
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
