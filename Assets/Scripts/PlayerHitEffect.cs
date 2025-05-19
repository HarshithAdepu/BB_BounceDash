using UnityEngine;
using DG.Tweening;

public class PlayerHitEffect : MonoBehaviour
{
    public float hitDuration;
    
    public GameObject player;
    public SpriteRenderer playerSpriteRenderer;
    public Sprite hitOutlineSprite;

    private Sprite _originalSprite;

    void Awake()
    {
        _originalSprite = playerSpriteRenderer.sprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Hit();
    }
    
    void Hit()
    {
        player.transform.DOKill();
        playerSpriteRenderer.DOKill();
        
        playerSpriteRenderer.sprite = hitOutlineSprite;
        
        player.transform.DOPunchScale(Vector3.one * 0.25f, hitDuration, 20, 1f)
        .OnComplete(() =>
        {
            playerSpriteRenderer.sprite = _originalSprite;
        });
    }
}
