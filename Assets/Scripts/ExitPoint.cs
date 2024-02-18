using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] Transform uiRoot;
    [SerializeField] float uiFaceSpeed = 1f;
    [SerializeField] CanvasGroup fadeCg;
    [SerializeField] string startScene;

    private Tween fadeTween = null;

    private void Update()
    {
        // Turn UI towards player
        Vector3 targetDirection = Game.Instance.Player.xrOrigin.transform.position - uiRoot.position;
        targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
        float singleStep = uiFaceSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(uiRoot.forward, targetDirection, singleStep, 0.0f);
        uiRoot.rotation = Quaternion.LookRotation(newDirection);
    }

    public void GrabRelease()
    {
        if (fadeTween != null) return;
        fadeTween = fadeCg.DOFade(1f, 5f).OnComplete(() =>
        {
            SceneManager.LoadScene(startScene, LoadSceneMode.Single);
        });
    }
}
