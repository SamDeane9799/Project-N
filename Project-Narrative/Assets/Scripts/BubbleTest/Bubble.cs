using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Animator m_animator;

    private BubbleTextAnimator m_bubbleTextAnimator = new BubbleTextAnimator();

    public void PlayAnim(string trigger, Action onFinish)
    {
        gameObject.SetActive(true);
        m_animator.SetTrigger(trigger);
        m_bubbleTextAnimator.SetText(m_text);
        List<DialogueCommand> commands = BubbleUtil.ProcessInputString(m_text.text, out string totalTextMessage);
        StartCoroutine(m_bubbleTextAnimator.AnimateTextIn(commands, totalTextMessage, onFinish));
    }
}