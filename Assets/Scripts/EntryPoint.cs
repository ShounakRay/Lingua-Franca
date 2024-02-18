using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EntryPoint : MonoBehaviour
{
    private void Start()
    {
        Game.Instance.Player.SetMovementState(false);
        Game.Instance.Player.SetRotationState(false);
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        cg.DOFade(0f, 4f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            Game.Instance.Player.SetMovementState(true);
            Game.Instance.Player.SetRotationState(true);
        });
    }
}
