using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    [SerializeField] private GameObject m_GlobalObject;

    private void Start()
    {
        DontDestroyOnLoad(m_GlobalObject);
        SceneManager.LoadScene("Title");
    }
}
