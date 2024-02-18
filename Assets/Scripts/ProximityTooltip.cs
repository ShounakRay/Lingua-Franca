using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTooltip : MonoBehaviour
{
    [SerializeField] private float proximityThreshold;

    private RectTransform root;
    private Vector3 initScale;
    private bool? wasInProximity = null;
    private Tween scaleTween;

    private void Awake()
    {
        root = GetComponent<RectTransform>();
        initScale = transform.localScale;
    }

    private void Update()
    {
        float dist = (Game.Instance.Player.xrOrigin.transform.position - root.transform.position).magnitude;
        bool inProximity = dist < proximityThreshold;
        if (inProximity != wasInProximity)
        {
            scaleTween.Kill();
            scaleTween = root.DOScale(inProximity ? initScale : Vector3.zero, 0.3f);
        }

        wasInProximity = inProximity;
    }
}
