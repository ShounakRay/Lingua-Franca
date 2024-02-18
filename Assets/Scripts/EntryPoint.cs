using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EntryPoint : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CanvasGroup>().DOFade(0f, 3f);
    }
}
