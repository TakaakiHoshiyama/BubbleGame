using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    private int m_Step;
    private float m_Timer;

    private void Start()
    {
        m_Step = 1;
        m_Timer = 10;
    }

    private void Update()
    {
        switch (m_Step)
        {
            case 1:
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0)
                {
                    var pos = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(1.5f, 3.5f), 0);
                    transform.DOLocalMove(pos, 1).SetEase(Ease.InOutCubic);
                    m_Timer = 10;
                }
                break;
        }
    }
}
