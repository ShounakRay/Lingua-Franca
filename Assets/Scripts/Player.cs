using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Player : MonoBehaviour
{
    [SerializeField] ActionBasedController leftHand;
    [SerializeField] ActionBasedController rightHand;

    public XROrigin xrOrigin { get; private set; }
    private DynamicMoveProvider moveProvider;
    private ActionBasedContinuousTurnProvider turnProvider;

    private void Awake()
    {
        xrOrigin = GetComponentInChildren<XROrigin>();
        moveProvider = GetComponentInChildren<DynamicMoveProvider>();
        turnProvider = GetComponentInChildren<ActionBasedContinuousTurnProvider>();
    }

    public IRecordingProvider GetRecordingProvider()
    {
        return GetComponentInChildren<RecordingProvider>();
    }

    /// <summary>
    /// Toggles whether the player can move or not.
    /// </summary>
    public void SetMovementState(bool canMove)
    {
        moveProvider.gameObject.SetActive(canMove);
    }

    public void SetRotationState(bool canTurn)
    {
        turnProvider.gameObject.SetActive(canTurn);
    }

    public bool Grabbing => leftHand.selectAction.action.ReadValue<float>() > 0.5f || rightHand.selectAction.action.ReadValue<float>() > 0.5f;
}
