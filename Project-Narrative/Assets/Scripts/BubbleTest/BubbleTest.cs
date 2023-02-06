using System;
using System.Collections;
using UnityEngine;

public class BubbleTest : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Bubble[] m_waveBubbles;
    [SerializeField] private Bubble[] m_distraughtBubbles;
    [SerializeField] private Bubble[] m_angerBubbles;
    [SerializeField] private Bubble[] m_sorrowBubbles;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            HideBubbles(m_waveBubbles);
            HideBubbles(m_distraughtBubbles);
            HideBubbles(m_angerBubbles);
            HideBubbles(m_sorrowBubbles);
            StartCoroutine(StartAnimSeq());
        }
    }

    private void PlayBubbles(Bubble[] bubbles, string trigger)
    {
        RecurseBubbles(bubbles, trigger, 0);
    }

    private void RecurseBubbles(Bubble[] bubbles, string trigger, int i)
    {
        if (i >= bubbles.Length)
        {
            return;
        }

        Action onFinish = delegate ()
        {
            RecurseBubbles(bubbles, trigger, i + 1);
        };
        bubbles[i].PlayAnim(trigger, onFinish);
    }

    private void HideBubbles(Bubble[] bubbles)
    {
        foreach (Bubble b in bubbles)
        {
            b.gameObject.SetActive(false);
        }
    }

    IEnumerator StartAnimSeq()
    {
        m_animator.SetTrigger("Wave");
        PlayBubbles(m_waveBubbles, "Neutral");
        yield return new WaitForSeconds(3.0f);

        HideBubbles(m_waveBubbles);
        m_animator.SetTrigger("Distraught");
        PlayBubbles(m_distraughtBubbles, "Sad");
        yield return new WaitForSeconds(6.0f);

        HideBubbles(m_distraughtBubbles);
        m_animator.SetTrigger("Anger");
        PlayBubbles(m_angerBubbles, "Angry");
        yield return new WaitForSeconds(5.0f);

        HideBubbles(m_angerBubbles);
        m_animator.SetTrigger("Sorrow");
        PlayBubbles(m_sorrowBubbles, "Sad");
    }
}
