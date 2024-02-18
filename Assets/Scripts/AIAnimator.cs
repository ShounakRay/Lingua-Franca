using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float playerNearRadius = 2;
    [SerializeField, Range(0, 1)] private float turnAnimateThreshold = 0.8f;
    [SerializeField] private float idleTriggerMin = 5f;
    [SerializeField] private float idleTriggerMax = 30f;

    private float idleRemainingSeconds = 0f;

    private const string kTurnParameter = "TurnDirection";
    private static readonly string[] kIdleTriggers = new[] { "Idle1", "Idle2", "Idle3", "Idle4" };

    private void Update()
    {
        Vector3 playerDelta = Game.Instance.Player.xrOrigin.transform.position - transform.position;
        if (playerDelta.magnitude > playerNearRadius) animator.SetInteger(kTurnParameter, 0);
        else
        {
            // Have AI gradually turn towards player when nearby (and animate when they are doing so)
            playerDelta = new Vector3(playerDelta.x, 0, playerDelta.z);
            float singleStep = turnSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, playerDelta, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            float turnMagnitude = Vector3.Dot(playerDelta.normalized, transform.forward);
            if (turnMagnitude < turnAnimateThreshold)
            {
                float cross = Vector3.Cross(playerDelta, transform.forward).y;
                if (cross > 0) animator.SetInteger(kTurnParameter, 1);
                else animator.SetInteger(kTurnParameter, -1);
            }
            else animator.SetInteger(kTurnParameter, 0);
        }

        idleRemainingSeconds -= Time.deltaTime;
        if (idleRemainingSeconds <= 0) OnIdleTrigger();
    }

    private void OnIdleTrigger()
    {
        idleRemainingSeconds = Random.Range(idleTriggerMin, idleTriggerMax);
        var trigger = kIdleTriggers[Random.Range(0, kIdleTriggers.Length)];
        animator.SetTrigger(trigger);
    }
}
