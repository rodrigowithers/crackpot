using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Pick()
    {
        transform.DOKill(true);

        transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutBack);
        _spriteRenderer.sortingOrder = 999;
    }

    public void Drop()
    {
        transform.DOKill(true);
        
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        _spriteRenderer.sortingOrder = 0;
    }
}
