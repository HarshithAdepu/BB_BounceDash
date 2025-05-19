using UnityEngine;
using DG.Tweening;

public class PlayerDeathEffect : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer playerSpriteRenderer;
    
    public float popDuration = 0.1f;
    public float fadeDuration = 0.3f;
    public float sinkDuration = 0.4f;
    
    public Vector3 popScale = new Vector2(1.3f, 1.3f);
    public float sinkDistance = 1f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Die();
        }
    }

    void Die()
    {
        enabled = false;
        player.transform.DOKill();
        playerSpriteRenderer.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        //Popped
        seq.Append(player.transform.DOScale(popScale, popDuration).SetEase(Ease.OutFlash));
        //Shrunk + Faded
        seq.Append(player.transform.DOScale(Vector2.zero, fadeDuration).SetEase(Ease.InQuad));
        seq.Join(playerSpriteRenderer.DOFade(0, fadeDuration));
        seq.Join(player.transform.DOMoveY(player.transform.position.y - sinkDistance, sinkDuration).SetEase(Ease.InQuad));
        
        seq.OnComplete(() => Debug.Log("Destroyed!"));
    }
}
