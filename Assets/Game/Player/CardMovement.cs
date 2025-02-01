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

        private Vector3 _cardVelocity;
        
        private void Update()
        {
            Mouse mouse = Mouse.current;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (CurrentCard == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 10, _cardLayerMask))
                    {
                        PickCard(hit);
                    }
                }
            }

            if (mouse.leftButton.isPressed)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10, _tableLayerMask))
                {
                    var pos = hit.point;
                    pos.y = 1.2f;
                    
                    CurrentCard.transform.position = 
                        Vector3.SmoothDamp(CurrentCard.transform.position, pos, ref _cardVelocity, _cardMovementTime);
                }
            }

            if (mouse.leftButton.wasReleasedThisFrame && CurrentCard != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10, _tableLayerMask))
                {
                    var pos = hit.point;
                    pos.y = 1.0f;
                    
                    DropCard(pos);
                }
            }
        }

        private void DropCard(Vector3 pos)
        {
            // For now, simply set card height back to 1
            CurrentCard.transform.position = pos;
            
            // Release card from reference
            CurrentCard = null;
        }

        private void PickCard(RaycastHit hit)
        {
            CurrentCard = hit.collider.GetComponent<Card>();
        }
    }
}
