using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float playerNearRadius = 2;
    [SerializeField, Range(0, 1)] private float turnAnimateThreshold = 0.8f;

    private const string kTurnParameter = "TurnDirection";

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


    }
}
