using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TweenExtensions
{
    public static Tween DOTypeWriter(this TextMeshProUGUI text, float durationPerChar = 0.02f)
    {
        int maxVisible = text.maxVisibleCharacters;
        text.maxVisibleCharacters = 0;
        float duration = durationPerChar * text.text.Length;
        return DOTween.To(value => text.maxVisibleCharacters = Mathf.FloorToInt(value * text.text.Length), 0, 1, duration)
            .OnKill(() => text.maxVisibleCharacters = maxVisible)
            .SetEase(Ease.Linear);
    }
}