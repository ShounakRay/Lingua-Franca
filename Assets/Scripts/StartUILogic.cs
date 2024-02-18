using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUILogic : MonoBehaviour
{
    public GameObject spanishToggle;
    public GameObject frenchToggle;
    public CanvasGroup startButton;
    public GameObject selectionWarning;

    public CanvasGroup menuCg;
    public CanvasGroup faderCg;
    public TextMeshProUGUI faderText;

    public string gameScene;
    public string[] introLines;

    private Language? targetLanguage = null;

    void Start()
    {
        menuCg.alpha = 0f;
        menuCg.blocksRaycasts = true;
        menuCg.DOFade(1f, 4f);

        startButton.alpha = 0f;
        startButton.blocksRaycasts = false;

        Game.Instance.Player.SetMovementState(false);
        spanishToggle.GetComponent<Toggle>().onValueChanged.AddListener(ClickSpanish);
        frenchToggle.GetComponent<Toggle>().onValueChanged.AddListener(ClickFrench);
        startButton.GetComponent<Button>().onClick.AddListener(ClickStart);
    }

    private void ClickSpanish(bool toggled)
    {
        if (toggled)
        {
            targetLanguage = Language.Spanish;
            startButton.DOFade(1f, 0.3f).OnComplete(() => startButton.blocksRaycasts = true);
        }
    }

    private void ClickFrench(bool toggled)
    {
        if (toggled)
        {
            targetLanguage = Language.French;
            startButton.DOFade(1f, 0.3f).OnComplete(() => startButton.blocksRaycasts = true);
        }
    }

    private void ClickStart()
    {
        Game.Instance.targetLanguage = targetLanguage ?? Language.Spanish;

        faderCg.blocksRaycasts = true;
        faderText.alpha = 0f;
        menuCg.blocksRaycasts = false;

        var s = DOTween.Sequence();
        s.Insert(0f, faderCg.DOFade(1f, 0.7f));
        s.Insert(0f, menuCg.DOFade(0f, 0.3f));

        for (int i = 0; i < introLines.Length; i++)
        {
            var t = s.Duration();
            int tmp = i;
            s.AppendCallback(() => faderText.text = introLines[tmp]);
            s.Insert(t, faderText.DOFade(1f, 0.3f));
            s.Insert(t, faderText.DOTypeWriter());
            s.AppendInterval(4f);
            s.Append(faderText.DOFade(0f, 0.3f));
        }

        s.AppendInterval(2f);
        s.AppendCallback(() => LoadGame());
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }
}
