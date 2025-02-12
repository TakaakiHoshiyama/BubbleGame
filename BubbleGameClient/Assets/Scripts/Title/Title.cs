using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Camera m_UiCamera;

    private void Start()
    {
        GlobalObject.Instance.SetupUICamera(m_UiCamera);
        m_StartButton.onClick.AddListener(() => OnClickStartButton());
        GlobalObject.Instance.Fader.FadeIn(1);
    }

    private void OnClickStartButton()
    {
        GlobalObject.Instance.Fader.FadeOut(1, () =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}
