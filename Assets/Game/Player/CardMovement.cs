using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class CardMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _cardLayerMask;
        [SerializeField] private LayerMask _tableLayerMask;
        
        [Header("Configuration")]
        [SerializeField] private float _cardMovementTime = 0.2f;
        
        public Card CurrentCard;

        private Vector2 _cardVelocity;
        
        private void Update()
        {
            Mouse mouse = Mouse.current;
            
            var mousePos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            mousePos.z = 0;
            
            DebugExtension.DebugCircle(mousePos, Vector3.forward, Color.red, 0.2f);

            if (mouse.leftButton.wasPressedThisFrame && CurrentCard == null)
            {
                var hit = Physics2D.OverlapCircle(mousePos, 0.2f, _cardLayerMask);
                if (hit != null)
                {
                    PickCard(hit);
                }
            }

            if (mouse.leftButton.isPressed && CurrentCard != null)
            {
                var hit = Physics2D.OverlapCircle(mousePos, 0.1f, _tableLayerMask);
                if (hit != null)
                {
                    CurrentCard.transform.position = Vector2.SmoothDamp(
                        CurrentCard.transform.position, mousePos, ref _cardVelocity, _cardMovementTime);
                }
            }

            if (mouse.leftButton.wasReleasedThisFrame && CurrentCard != null)
            {
                var hit = Physics2D.OverlapCircle(mousePos, 0.1f, _tableLayerMask);
                if (hit != null)
                    DropCard(mousePos);
            }
        }

        private void DropCard(Vector3 pos)
        {
            // For now, simply set card height back to 1
            CurrentCard.transform.DOMove(pos, 0.25f);
            CurrentCard.Drop();

            // Release card from reference
            CurrentCard = null;
        }

        private void PickCard(Collider2D hit)
        {
            CurrentCard = hit.GetComponent<Card>();
            CurrentCard.Pick();
        }
    }
}
