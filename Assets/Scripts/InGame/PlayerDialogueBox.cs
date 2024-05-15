using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerDialogueBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _group.alpha = 0;
    }

    public void Pop(string dialogue, float duration)
    {
        _text.SetText(dialogue);
        _group.DOFade(1, .5f);

        StartCoroutine(WaitRoutine(duration));
    }

    IEnumerator WaitRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _group.DOFade(0, .5f);
        _text.SetText(string.Empty);
    }
}
