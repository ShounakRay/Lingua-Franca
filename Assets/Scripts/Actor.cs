using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Actor : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform uiRoot;
    [SerializeField] private RectTransform hoverGreeting;
    [SerializeField] private float uiFaceSpeed = 1;

    private Tween hoverTween;
    private Vector3 hoverInitialScale;

    private void Awake()
    {
        hoverInitialScale = hoverGreeting.localScale;
        hoverGreeting.localScale = Vector3.zero;
    }

    private void Update()
    {
        // Make UI gradually look towards player
        Vector3 targetDirection = Game.Instance.Player.xrOrigin.transform.position - uiRoot.position;
        targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
        float singleStep = uiFaceSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(uiRoot.forward, targetDirection, singleStep, 0.0f);
        uiRoot.rotation = Quaternion.LookRotation(newDirection);
    }

    /// <summary>
    /// Begins an interaction with the actor.
    /// </summary>
    public void Interact()
    {
        Debug.Log($"Interacting with {gameObject.name}!");
    }

    public void HoverEnter()
    {
        hoverTween.Kill();
        hoverTween = hoverGreeting.DOScale(hoverInitialScale, 0.3f);
    }

    public void HoverExit()
    {
        hoverTween.Kill();
        hoverTween = hoverGreeting.DOScale(0f, 0.3f);
    }
}
