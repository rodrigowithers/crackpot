using DG.Tweening;
using Game.Cards;
using Game.Table;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class CardMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _cardLayerMask;
        [SerializeField] private LayerMask _tableLayerMask;
        [SerializeField] private LayerMask _cardPileLayerMask;
        [SerializeField] private LayerMask _cardSpaceLayerMask;
        
        [Header("Configuration")]
        [SerializeField] private float _cardMovementTime = 0.2f;
        
        public Card CurrentCard;

        private Vector2 _cardVelocity;
        
        private void Update()
        {
            Mouse mouse = Mouse.current;
            
            var mousePos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            mousePos.z = 0;
            
            DebugExtension.DebugCircle(mousePos, Vector3.forward, Color.red, 0.1f);

            if (mouse.leftButton.wasPressedThisFrame && CurrentCard == null)
            {
                // Getting cards from table
                var cardHit = Physics2D.OverlapCircle(mousePos, 0.1f, _cardLayerMask);
                if (cardHit != null)
                {
                    // Check if card is not in a CardSpace, if it is, only pick the last card from that space
                    var card = cardHit.GetComponent<Card>();

                    // Dont pick up cards on goal
                    if (card.OnGoal)
                        return;
                    
                    if (card.CurrentCardSpace != null)
                    {
                        if(card.CurrentCardSpace.TopCard == card)
                            PickCard(card);
                        else
                        {
                            card.transform.DOKill(true);
                            card.transform.DOShakeRotation(0.5f, 10);
                        }
                    }
                    else
                    {
                        PickCard(card);
                    }
                }
                
                // Getting cards from Pile
                var pileHit = Physics2D.OverlapCircle(mousePos, 0.2f, _cardPileLayerMask);
                if (pileHit != null)
                {
                    var cardPile = pileHit.GetComponent<CardPile>();
                    PickCard(cardPile.PickCard());
                    
                    CurrentCard.OriginalPosition = cardPile.CardPosition;
                }
            }
            
            // Handle Card Movement
            if (CurrentCard != null)
            {
                if (mouse.leftButton.isPressed)
                {
                    var hit = Physics2D.OverlapCircle(mousePos, 0.1f, _tableLayerMask);
                    if (hit != null)
                    {
                        CurrentCard.transform.position = Vector2.SmoothDamp(
                            CurrentCard.transform.position, mousePos, ref _cardVelocity, _cardMovementTime);
                    }
                }

                if (mouse.leftButton.wasReleasedThisFrame)
                {
                    var hit = Physics2D.OverlapCircle(mousePos, 0.1f, _tableLayerMask);
                    if (hit != null)
                        DropCard(mousePos);
                }
            }
        }

        private void DropCard(Vector3 pos)
        {
            // Check if player hovered on top of CardSpace
            var cardSpaceHit = Physics2D.OverlapCircle(pos, 0.2f, _cardSpaceLayerMask);
            if (cardSpaceHit != null)
            {
                var cardSpace = cardSpaceHit.GetComponent<CardSpace>();

                if (cardSpace is GoalSpace space)
                {
                    TryPlaceOnGoalSpace(space);
                }
                else
                {
                    TryPlaceOnCardSpace(cardSpace);
                }
            }
            else
            {
                ReturnCardToOrigin();
            }
        }

        private void TryPlaceOnGoalSpace(GoalSpace goalSpace)
        {
            // Check first if cardSpace is empty
            if (goalSpace.TopCard == null)
            {
                // No card at Card Space, if this is an Ace (1), drop
                if (CurrentCard.Number == 1)
                {
                    // Set card as OnGoal
                    CurrentCard.OnGoal = true;
                    DropAtCardSpace(goalSpace);
                }
                else
                {
                    ReturnCardToOrigin();
                }
            }
            else
            {
                // If not, check if top card is same type and next number
                if (goalSpace.TopCard.Suit == CurrentCard.Suit && CurrentCard.Number == goalSpace.TopCard.Number + 1)
                {
                    CurrentCard.OnGoal = true;
                    DropAtCardSpace(goalSpace);
                }
                else
                {
                    ReturnCardToOrigin();
                }
            }
        }

        private void TryPlaceOnCardSpace(CardSpace cardSpace)
        {
            // Check first if cardSpace is empty
            if (cardSpace.TopCard == null)
            {
                // No card at Card Space, put this card in it
                DropAtCardSpace(cardSpace);
            }
            else
            {
                // If not, check if colors are not the same
                if (cardSpace.TopCard.Color != CurrentCard.Color)
                {
                    // Check if card number is correct
                    bool canDrop = cardSpace.TopCard.Number == CurrentCard.Number + 1;

                    if (canDrop)
                    {
                        DropAtCardSpace(cardSpace);
                    }
                    else
                    {
                        ReturnCardToOrigin();
                    }
                }
                else
                {
                    ReturnCardToOrigin();
                }
            }
        }

        private void ReturnCardToOrigin()
        {
            if (CurrentCard.OriginalCardSpace != null)
            {
                DropAtCardSpace(CurrentCard.OriginalCardSpace);
            }
            else
            {
                // Return card to Original Position
                CurrentCard.transform.DOMove(CurrentCard.OriginalPosition, 0.25f);
                CurrentCard.Drop();

                // Release card from reference
                CurrentCard = null;
            }
        }

        private void DropAtCardSpace(CardSpace cardSpace)
        {
            CurrentCard.transform.DOMove(cardSpace.TopPosition, 0.25f);

            // Drop and update sorting order
            CurrentCard.Drop(cardSpace.Cards.Count + 1);
            cardSpace.Put(CurrentCard);
            
            CurrentCard = null;
        }

        private void PickCard(Card card)
        {
            CurrentCard = card;
            CurrentCard.OriginalPosition = CurrentCard.transform.position;
            
            // Check if this card came from a Card Space, if so, keep it stored to return it 
            if(card.CurrentCardSpace != null)
                CurrentCard.OriginalCardSpace = card.CurrentCardSpace;
            
            CurrentCard.Pick();
        }
    }
}
