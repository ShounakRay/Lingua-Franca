using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TweenExtensions
{
    public static Tween DOTypeWriter(this TextMeshProUGUI text, string copy, float durationPerChar = 0.02f)
    {
        int maxVisible = text.maxVisibleCharacters;
        float duration = durationPerChar * copy.Length;
        return DOTween.To(
                value => text.text = copy.Substring(0, Mathf.FloorToInt(value * copy.Length)), 
                0, 1, duration
            )
            .OnKill(() => text.text = copy)
            .SetEase(Ease.Linear);
    }
}