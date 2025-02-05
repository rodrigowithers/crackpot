using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Cards.Editor
{
    public class CreateCardsTool : EditorWindow
    {
        [MenuItem("Tools/Create Cards")]
        private static void CreateCards()
        {
            // Get all selected sprites
            Sprite[] selectedSprites = Selection.GetFiltered<Sprite>(SelectionMode.Editable);
        
            if (selectedSprites.Length == 0)
            {
                Debug.LogWarning("No sprites selected!");
                return;
            }

            foreach (Sprite sprite in selectedSprites)
            {
                // Create parent GameObject
                var splittedName = sprite.name.Split('_');
                var spriteName = splittedName[1] + " " + splittedName[2];
                GameObject cardObject = new GameObject($"Card - {spriteName}");
                cardObject.layer = LayerMask.NameToLayer("Cards");
            
                // Add components with undo support
                Undo.RegisterCreatedObjectUndo(cardObject, "Create Card");
                Undo.AddComponent<Card>(cardObject);
                BoxCollider2D collider = Undo.AddComponent<BoxCollider2D>(cardObject);

                // Create child sprite GameObject
                GameObject spriteChild = new GameObject("Sprite");
                Undo.RegisterCreatedObjectUndo(spriteChild, "Create Sprite Child");
            
                // Parent and position child
                spriteChild.transform.SetParent(cardObject.transform);
                spriteChild.transform.localPosition = Vector3.zero;

                // Add and configure sprite renderer
                SpriteRenderer renderer = Undo.AddComponent<SpriteRenderer>(spriteChild);
                renderer.sprite = sprite;

                // Configure collider based on sprite size
                collider.size = sprite.bounds.size;
            }
        }

        [MenuItem("Tools/Create Cards", true)]
        private static bool ValidateCreateCards()
        {
            // Only enable menu item if sprites are selected
            return Selection.GetFiltered<Sprite>(SelectionMode.Editable).Any();
        }
    }
}