using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Player : MonoBehaviour
{
    public XROrigin xrOrigin { get; private set; }
    private DynamicMoveProvider moveProvider;

    private void Awake()
    {
        xrOrigin = GetComponentInChildren<XROrigin>();
        moveProvider = GetComponentInChildren<DynamicMoveProvider>();
    }

    /// <summary>
    /// Toggles whether the player can move or not.
    /// </summary>
    public void SetMovementState(bool canMove)
    {
        moveProvider.gameObject.SetActive(canMove);
    }
}
