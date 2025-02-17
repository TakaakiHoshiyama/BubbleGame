using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Camera m_UiCamera;

    private List<Joycon> m_Joycons;

    private void Start()
    {
        GlobalObject.Instance.SetupUICamera(m_UiCamera);
        m_StartButton.onClick.AddListener(() => OnClickStartButton());
        m_Joycons = JoyconManager.Instance.j;
        GlobalObject.Instance.Fader.FadeIn(1);
    }

    private void Update()
    {
        if (!GlobalObject.Instance.Fader.IsFade)
        {
            if (m_Joycons.Count >= 1)
            {
                if (m_Joycons[0].GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    OnClickStartButton();
                    return;
                }
            }
            if (m_Joycons.Count >= 2)
            {
                if (m_Joycons[1].GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    OnClickStartButton();
                    return;
                }
            }
        }
    }

    private void OnClickStartButton()
    {
        GlobalObject.Instance.Fader.FadeOut(1, () =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}
