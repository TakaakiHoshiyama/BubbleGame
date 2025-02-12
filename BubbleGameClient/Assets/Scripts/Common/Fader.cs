using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image m_FaderImage;

    public void FadeIn(float time, Action onFinish = null)
    {
        m_FaderImage.gameObject.SetActive(true);
        m_FaderImage.color = Color.black;
        m_FaderImage.DOColor(new Color(0, 0, 0, 0), time).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_FaderImage.gameObject.SetActive(false);
            onFinish?.Invoke();
        });
    }

    public void FadeOut(float time, Action onFinish = null)
    {
        m_FaderImage.gameObject.SetActive(true);
        m_FaderImage.color = new Color(0, 0, 0, 0);
        m_FaderImage.DOColor(Color.black, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            onFinish?.Invoke();
        });
    }

    private void Start()
    {
        m_FaderImage.gameObject.SetActive(false);
    }
}
