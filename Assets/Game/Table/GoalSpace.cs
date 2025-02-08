using Game.Cards;
using UnityEngine;

namespace Game.Table
{
    public class GoalSpace : CardSpace
    {
        public override Vector2 TopPosition => transform.position;

        public override void Put(Card card)
        {
            // Check if Goal is cleared
            base.Put(card);
        }

        public override void Take(Card card)
        {
            // Do nothing, cant take cards from Goal Spaces
        }
    }
}