using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    [SerializeField] GameObject m_Bubble;
    [SerializeField] GameObject m_Player1;
    [SerializeField] GameObject m_Player2;
    [SerializeField] GameObject m_Boss;
    [SerializeField] Transform m_StageBase;
    [SerializeField] TextMeshProUGUI m_ScoreText;

    private float m_Player1Speed;
    private float m_Player1Angle;
    private float m_Player2Speed;
    private float m_Player2Angle;
    private float m_Counter;
    private int m_Score;

    private List<Bubble> m_Player1Bubbles = new();
    private List<Bubble> m_Player2Bubbles = new();
    private List<Bubble> m_AttackBubbles = new();

    private void Start()
    {
        m_Player1Speed = 2.0f;
        m_Player2Speed = 2.0f;
        m_Player1Angle = 10;
        m_Player2Angle = 10;
        m_Counter = 0;
        m_Player1Bubbles.Clear();
        m_Player1Bubbles.Clear();
        m_AttackBubbles.Clear();
        m_Score = 0;
    }

    private void Update()
    {
        // 現在のキーボード情報
        m_Player1Speed = 2;
        m_Player2Speed = 2;
        m_Player1Angle = 10;
        m_Player2Angle = 10;
        var current = Keyboard.current;
        if (current != null)
        {
            if (current.zKey.isPressed)
            {
                m_Player1Speed = 4;
            }
            else if (current.aKey.isPressed)
            {
                m_Player1Speed = 4;
                m_Player1Angle = 20;
            }
            else if (current.qKey.isPressed)
            {
                m_Player1Speed = 4;
                m_Player1Angle = 30;
            }
            if (current.oem2Key.isPressed)
            {
                m_Player2Speed = 4;
            }
        }

        m_Counter -= Time.deltaTime;
        if (m_Counter <= 0)
        {
            CreateBubble(0, m_Player1.transform.localPosition, m_Player1Angle, m_Player1Speed);
            CreateBubble(1, m_Player2.transform.localPosition, m_Player2Angle, m_Player2Speed);
            m_Counter = 0.1f;
        }

        for (var i = (m_Player1Bubbles.Count - 1); i >= 0; i--)
        {
            if (m_Player1Bubbles[i].Move())
            {
                Destroy(m_Player1Bubbles[i].gameObject);
                m_Player1Bubbles.RemoveAt(i);
            }
        }
        for (var i = (m_Player2Bubbles.Count - 1); i >= 0; i--)
        {
            if (m_Player2Bubbles[i].Move())
            {
                Destroy(m_Player2Bubbles[i].gameObject);
                m_Player2Bubbles.RemoveAt(i);
            }
        }
        for (var i = (m_AttackBubbles.Count - 1); i >= 0; i--)
        {
            if (m_AttackBubbles[i].Move())
            {
                Destroy(m_AttackBubbles[i].gameObject);
                m_AttackBubbles.RemoveAt(i);
            }
        }

        for (var i = (m_Player1Bubbles.Count - 1); i >= 0; i--)
        {
            var p1bubble = m_Player1Bubbles[i].gameObject;
            for (var j = (m_Player2Bubbles.Count - 1); j >= 0; j--)
            {
                var p2bubble = m_Player2Bubbles[j].gameObject;
                var diff = p2bubble.transform.position - p1bubble.transform.position;
                var dist = diff.magnitude;
                var conflict = dist <= 0.5f;
                if (conflict)
                {
                    var pos = p1bubble.transform.localPosition + diff * 0.5f;
                    CreateBubble(2, pos, 90, 5);
                    Destroy(p1bubble);
                    Destroy(p2bubble);
                    m_Player1Bubbles.RemoveAt(i);
                    m_Player2Bubbles.RemoveAt(j);
                    break;
                }
            }
        }

        for (var i = (m_AttackBubbles.Count - 1); i >= 0; i--)
        {
            var diff = m_Boss.transform.position - m_AttackBubbles[i].transform.position;
            var dist = diff.magnitude;
            var conflict = dist <= 0.5f;
            if (conflict)
            {
                m_Score++;
                m_ScoreText.text = "Score:" + m_Score;
                Destroy(m_AttackBubbles[i].gameObject);
                m_AttackBubbles.RemoveAt(i);
            }
        }
    }

    private void CreateBubble(int side, Vector3 pos, float angle, float speed)
    {
        var go = Instantiate(m_Bubble, m_StageBase);
        go.transform.localPosition = pos;
        var comp = go.GetComponent<Bubble>();
        Vector2 velocity;
        if (side == 0)
        {
            velocity = Vector2.right * speed;
            velocity = Quaternion.Euler(0, 0, angle) * velocity;
            m_Player1Bubbles.Add(comp);
        }
        else if (side == 1)
        {
            velocity = Vector2.left * speed;
            velocity = Quaternion.Euler(0, 0, -angle) * velocity;
            m_Player2Bubbles.Add(comp);
        }
        else
        {
            velocity = Vector2.right * speed;
            velocity = Quaternion.Euler(0, 0, angle) * velocity;
            m_AttackBubbles.Add(comp);
        }
        comp.SetParam(side, velocity);
    }
}
