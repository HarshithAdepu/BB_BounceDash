using UnityEngine;
using DG.Tweening;

public class PlayerSquashStretch : MonoBehaviour
{
    public GameObject player;
    
    public Vector2 squashScale;
    public Vector2 stretchScale;
    public float totalDuration = 0.3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoSquashStretch();
        }
    }

    void DoSquashStretch()
    {
        player.transform.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        float duration = totalDuration / 3f;
        
        //Squash
        seq.Append(player.transform.DOScale(new Vector2(squashScale.x, squashScale.y), duration).SetEase(Ease.OutQuad));
        //Stretch
        seq.Append(player.transform.DOScale(new Vector2(stretchScale.x, stretchScale.y), duration).SetEase(Ease.OutQuad));
        //Reset
        seq.Append(player.transform.DOScale(1f, duration).SetEase(Ease.OutBack));
    }
}