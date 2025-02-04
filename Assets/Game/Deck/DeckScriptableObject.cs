using UnityEngine;

namespace Game.Deck
{
    [CreateAssetMenu(menuName = "Game/Deck/Create New Deck", order = 0)]
    public class DeckScriptableObject : ScriptableObject
    {
        public Card[] Cards;
    }
}