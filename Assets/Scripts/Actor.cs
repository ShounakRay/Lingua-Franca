using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Threading.Tasks;

public class Actor : MonoBehaviour
{
    private enum InteractionState
    {
        None,
        Prompting,
        ResponseRequested,
        Responding,
        ResponseInterpreting,
    }

    [Header("Info")]
    [SerializeField] private ActorInfo info;
    public ActorInfo Info => info;

    [Header("UI References")]
    [SerializeField] private Transform uiRoot;
    [SerializeField] private float uiFaceSpeed = 1;
    [SerializeField] private RectTransform hoverGreeting;
    [SerializeField] private RectTransform dialogue;
    [SerializeField] private RectTransform suggestions;
    [SerializeField] private TextMeshProUGUI hoverName;

    [Header("Dialogue References")]
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private RectTransform dialogueDivider;
    [SerializeField] private TextMeshProUGUI dialoguePrompt;
    [SerializeField] private TextMeshProUGUI dialogueReply;
    [SerializeField] private CanvasGroup dialogueButtonsGroup;
    [SerializeField] private RectTransform dialogueVoiceIndicator;
    [SerializeField] private Button dialogueSuggestionsButton;
    [SerializeField] private Button dialogueLeaveButton;

    [Header("Suggestions References")]
    [SerializeField] private TextMeshProUGUI suggestionsText;

    public bool IsInteracting => interactionState != InteractionState.None;

    private InteractionState interactionState;

    private Vector3 hoverInitialScale;
    private Vector3 dialogueInitialScale;
    private Vector3 suggestionsInitialScale;

    private Tween hoverTween;
    private Sequence dialogueTween;
    private Sequence suggestionsTween;

    private string dialogueLastReply;
    private string currentPrompt;
    private IResponseProvider responseProvider;
    private IResponseProvider suggestionsProvider;
    private IRecordingProvider recordingProvider;
    private bool suggestionsOpen;

    private XRInteractableSnapVolume snapVolume;

    private async void Awake()
    {
        Debug.Log("[Actor.Awake] Inside Awake Functions");
        if (info == null) Debug.LogWarning("Actor is missing info!");

        hoverInitialScale = hoverGreeting.localScale;
        hoverGreeting.localScale = Vector3.zero;

        dialogueInitialScale = dialogue.localScale;
        dialogue.localScale = Vector3.zero;

        suggestionsInitialScale = suggestions.localScale;
        suggestions.localScale = Vector3.zero;

        responseProvider = GetResponseProvider();
        suggestionsProvider = GetSuggestionsProvider();

        // LLM_ResponseProvider provider = new(info, null);
        // Debug.Log(await ResponseLLM_Test(provider, null));
        // Debug.Log(await ResponseLLM_Test(provider, "Tell me about the taj mahal."));
        // Debug.Log(await SuggestionsLLM_Test("Hello, how are you?"));
        // ResponseSuggestionJoint_Test(cycles: 10);
        // string output = await SuggestionsLLM_Test("Hello, how are you?");

        snapVolume = GetComponentInChildren<XRInteractableSnapVolume>();

        interactionState = InteractionState.None;

        dialogueLeaveButton.onClick.AddListener(Leave);
        dialogueSuggestionsButton.onClick.AddListener(() => SetSuggestionsVisibility(!suggestionsOpen));

        dialogueName.text = info.FirstName;
        hoverName.text = info.FirstName;
    }

    private void Start()
    {
        recordingProvider = Game.Instance.Player.GetRecordingProvider();
        recordingProvider.OnRecordingUpdated += OnRecordingUpdated;
    }

    private void Update()
    {
        // Make UI gradually look towards player
        Vector3 targetDirection = Game.Instance.Player.xrOrigin.transform.position - uiRoot.position;
        targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
        float singleStep = uiFaceSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(uiRoot.forward, targetDirection, singleStep, 0.0f);
        uiRoot.rotation = Quaternion.LookRotation(newDirection);

        // Check response status and trigger callbacks accordingly
        if (interactionState == InteractionState.ResponseRequested && Game.Instance.Player.Grabbing) BeginReply();
        if (interactionState == InteractionState.Responding && !Game.Instance.Player.Grabbing) FinishReply();
    }

    public IResponseProvider GetResponseProvider()
    {
        return new LLM_ResponseProvider(actor_info: info, parameters: null);
    }

    public IResponseProvider GetSuggestionsProvider()
    {
        return new LLM_SuggestionProvider(parameters: null);
    }


    private void OnRecordingUpdated(string transcription)
    {
        if (interactionState != InteractionState.Responding) return;
        dialogueReply.text = transcription;
    }

    /// <summary>
    /// Begins an interaction with the actor.
    /// </summary>
    public void Interact()
    {
        if (IsInteracting) return;
        HoverExit();

        Debug.Log($"Interacting with {gameObject.name}!");

        snapVolume.gameObject.SetActive(false);
        Game.Instance.Player.SetMovementState(false);

        NextPrompt(true);
    }

    /// <summary>
    /// Stops the running interaction with the actor.
    /// </summary>
    public void Leave()
    {
        if (!IsInteracting) return;
        interactionState = InteractionState.None;

        dialogueTween.Kill();
        dialogueTween = DOTween.Sequence();
        dialogueTween.Append(dialogue.DOScale(Vector3.zero, 0.3f));
        Game.Instance.Player.SetMovementState(true);
        SetSuggestionsVisibility(false);
    }

    private async void SetSuggestionsVisibility(bool visible)
    {
        suggestionsTween.Kill();
        suggestionsTween = DOTween.Sequence();
        if (visible)
        {
            string suggestion = await suggestionsProvider.GetResponse(currentPrompt);
            suggestionsText.text = suggestion;
            suggestionsTween.Append(suggestions.DOScale(suggestionsInitialScale, 0.3f));
            suggestionsTween.Append(suggestionsText.DOTypeWriter());
        }
        else
        {
            suggestionsTween.Append(suggestions.DOScale(Vector3.zero, 0.3f));
        }

        suggestionsOpen = visible;
    }

    public void HoverEnter()
    {
        if (IsInteracting) return;
        hoverTween.Kill();
        hoverTween = hoverGreeting.DOScale(hoverInitialScale, 0.3f);
        snapVolume.gameObject.SetActive(true);
    }

    public void HoverExit()
    {
        if (IsInteracting) return;
        hoverTween.Kill();
        hoverTween = hoverGreeting.DOScale(0f, 0.3f);
    }

    private void NextPrompt(bool isEntry = false)
    {
        interactionState = InteractionState.Prompting;
        SetSuggestionsVisibility(false);

        dialogueTween.Kill();
        dialogueTween = DOTween.Sequence();

        if (isEntry)
        {
            dialogueTween.Append(dialogue.DOScale(dialogueInitialScale, 0.3f));
            dialogueDivider.localScale = new Vector3(0, 1, 1);
            dialogueTween.Append(dialogueDivider.DOScaleX(1, 0.6f));
            dialogueLastReply = null;
        }

        dialogueReply.text = "";
        dialogueReply.alpha = 0;
        dialoguePrompt.text = "";
        dialoguePrompt.alpha = 0;
        dialogueButtonsGroup.alpha = 0;
        dialogueButtonsGroup.blocksRaycasts = false;
        dialogueVoiceIndicator.localScale = new Vector3(1, 0, 1);
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogue);

        var t = dialogueTween.Duration();
        dialogueTween.Insert(t, dialoguePrompt.DOFade(1f, 0.3f));
        dialogueTween.Insert(t, dialogueButtonsGroup.DOFade(0f, 0.3f));
        dialogueButtonsGroup.blocksRaycasts = false;

        dialogueTween.OnComplete(async () =>
        {
            string prompt = await responseProvider.GetResponse(dialogueLastReply);
            dialogueTween.Kill();
            dialoguePrompt.text = prompt;
            dialogueTween = DOTween.Sequence();
            dialogueTween.Append(dialoguePrompt.DOTypeWriter());
            dialogueTween.AppendInterval(1f);
            dialogueTween.OnComplete(() => NextReply());
        });
    }

    private void NextReply()
    {
        dialogueReply.alpha = 0;
        dialogueReply.text = "Hold <b>[grab]</b> and speak to reply";

        dialogueTween.Kill();
        dialogueTween = DOTween.Sequence();
        dialogueTween.Insert(0f, dialogueReply.DOFade(1f, 0.3f));
        dialogueTween.Insert(0.5f, dialogueButtonsGroup.DOFade(1f, 0.3f));
        dialogueButtonsGroup.blocksRaycasts = true;
        dialogueTween.OnComplete(() =>
        {
            interactionState = InteractionState.ResponseRequested;
        });
    }

    private void BeginReply()
    {
        interactionState = InteractionState.Responding;
        dialogueReply.text = "";
        recordingProvider.StartRecording();
        dialogueTween.Kill();
        dialogueTween = DOTween.Sequence();
        dialogueTween.Insert(0f, dialogueVoiceIndicator.DOScaleY(1f, 0.3f));
        dialogueTween.Insert(0f, dialogueButtonsGroup.DOFade(0f, 0.3f));
        dialogueButtonsGroup.blocksRaycasts = false;
    }

    private async void FinishReply()
    {
        void ResetToPrompt(bool hidePrompt = true)
        {
            var t = dialogueTween.Duration();
            dialogueTween.Insert(t, dialogueReply.DOFade(0, 0.3f));
            if (hidePrompt) dialogueTween.Insert(t, dialoguePrompt.DOFade(0, 0.3f));
        }

        interactionState = InteractionState.ResponseInterpreting;
        string reply = await recordingProvider.StopRecording();


        dialogueTween.Kill();
        dialogueTween = DOTween.Sequence();
        dialogueTween.Insert(0f, dialogueVoiceIndicator.DOScaleY(0f, 0.3f));

        if (string.IsNullOrEmpty(reply))
        {
            // If grabbing audio failed, try again
            ResetToPrompt(false);
            dialogueTween.OnComplete(() => NextReply());
            return;
        }

        dialogueReply.text = reply;
        dialogueLastReply = reply;
        dialogueTween.AppendInterval(2f);
        ResetToPrompt();
        dialogueTween.OnComplete(() => NextPrompt());
    }
}
