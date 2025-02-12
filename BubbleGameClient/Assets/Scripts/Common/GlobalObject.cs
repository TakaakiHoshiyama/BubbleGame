using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour
{
    [SerializeField] private Fader m_Fader;

    private static GlobalObject m_Instance;

    public static GlobalObject Instance => m_Instance;

    public Fader Fader => m_Fader;

    public void SetupUICamera(Camera uiCamera)
    {
        var canvas = m_Fader.GetComponent<Canvas>();
        canvas.worldCamera = uiCamera;
    }

    private void Awake()
    {
        m_Instance = this;
    }
}
