using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceIndicator : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float spread = 1;

    private void Update()
    {
        float mid = transform.childCount / 2f;
        foreach (Transform child in transform)
        {
            float scale = Evaluate(Mathf.Abs(child.GetSiblingIndex() - mid) * spread + Time.time * speed);
            child.localScale = new Vector3(1, scale, 1);
        }
    }

    private float Evaluate(float ts)
    {
        return (Mathf.Sin(ts + Mathf.PI) +
            Mathf.Sin(2 * ts + Mathf.PI) +
            Mathf.Sin(3 * ts) +
            Mathf.Sin(4 * ts)) / 3.078f;
    }
}
