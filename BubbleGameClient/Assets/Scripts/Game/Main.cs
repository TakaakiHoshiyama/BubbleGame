using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    [SerializeField] private GameObject m_BubblePrefab;
    [SerializeField] private GameObject m_EnemyPrefab;

    [SerializeField] private Camera m_UiCamera;
    [SerializeField] private GameObject m_Player1;
    [SerializeField] private GameObject m_Player2;
    [SerializeField] private GameObject m_Boss;
    [SerializeField] private Transform m_StageBase;
    [SerializeField] private Transform m_BubbleParent;
    [SerializeField] private Transform m_EnemyParent;
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    private float m_Player1Speed;
    private float m_Player1Angle;
    private float m_Player1Counter;
    private List<Bubble> m_Player1Bubbles = new();
    private float m_Player2Speed;
    private float m_Player2Angle;
    private float m_Player2Counter;
    private List<Bubble> m_Player2Bubbles = new();

    private int m_Score;

    private List<Enemy> m_Enemies = new();
    private float m_EnemySpawnCounter;
    private List<Bubble> m_AttackBubbles = new();

    private void Start()
    {
        GlobalObject.Instance.SetupUICamera(m_UiCamera);

        m_Player1Counter = 0;
        m_Player2Counter = 0;
        m_Player1Bubbles.Clear();
        m_Player1Bubbles.Clear();
        m_AttackBubbles.Clear();
        m_Enemies.Clear();
        m_Score = 0;

        GlobalObject.Instance.Fader.FadeIn(1);
    }

    private void Update()
    {
        GameMain();
    }

    private void GameMain()
    {
        InputProc();

        m_Player1.transform.localRotation = Quaternion.Euler(0, 0, m_Player1Angle);
        m_Player2.transform.localRotation = Quaternion.Euler(0, 0, -m_Player2Angle);

        // ñAê∂ê¨
        m_Player1Counter -= Time.deltaTime;
        if (m_Player1Counter <= 0)
        {
            CreateBubble(0, m_Player1.transform.localPosition, m_Player1Angle, m_Player1Speed);
            m_Player1Counter = 0.2f / (m_Player1Speed / 4);
        }
        m_Player2Counter -= Time.deltaTime;
        if (m_Player2Counter <= 0)
        {
            CreateBubble(1, m_Player2.transform.localPosition, m_Player2Angle, m_Player2Speed);
            m_Player2Counter = 0.2f / (m_Player2Speed / 4);
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

        // éGãõà⁄ìÆ
        m_EnemySpawnCounter -= Time.deltaTime;
        if (m_EnemySpawnCounter < 0)
        {
            var go = Instantiate(m_EnemyPrefab, m_EnemyParent);
            var pos = new Vector3(Random.Range(-7.5f, 7.5f), 7, 0);
            go.transform.localPosition = pos;
            var comp = go.GetComponent<Enemy>();
            comp.SetParam(new Vector2(0, -1));
            m_Enemies.Add(comp);
            m_EnemySpawnCounter = 1;
        }
        for (var i = (m_Enemies.Count - 1); i >= 0; i--)
        {
            if (m_Enemies[i].Move())
            {
                Destroy(m_Enemies[i].gameObject);
                m_Enemies.RemoveAt(i);
            }
        }


        // ñAìØémÇÃè’ìÀîªíË
        for (var i = (m_Player1Bubbles.Count - 1); i >= 0; i--)
        {
            var p1bubble = m_Player1Bubbles[i];
            for (var j = (m_Player2Bubbles.Count - 1); j >= 0; j--)
            {
                var p2bubble = m_Player2Bubbles[j];
                var diff = p2bubble.transform.position - p1bubble.transform.position;
                var dist = diff.magnitude;
                var conflict = dist <= 0.5f;
                if (conflict)
                {
                    var pos = p1bubble.transform.localPosition + diff * 0.5f;
                    var velocity = p1bubble.Velocity + p2bubble.Velocity;
                    velocity *= 2;
                    Destroy(p1bubble.gameObject);
                    Destroy(p2bubble.gameObject);
                    m_Player1Bubbles.RemoveAt(i);
                    m_Player2Bubbles.RemoveAt(j);
                    if (velocity.magnitude >= 0.1f)
                    {
                        CreateBubble(2, pos, velocity);
                    }
                    break;
                }
            }
        }

        // çUåÇñAÇÃè’ìÀîªíË
        for (var i = (m_AttackBubbles.Count - 1); i >= 0; i--)
        {
            var diff = m_Boss.transform.position - m_AttackBubbles[i].transform.position;
            var dist = diff.magnitude;
            var conflict = dist <= 1.0f;
            if (conflict)
            {
                m_Score++;
                m_ScoreText.text = "Score:" + m_Score;
                Destroy(m_AttackBubbles[i].gameObject);
                m_AttackBubbles.RemoveAt(i);
            }
            else
            {
                for (var j = (m_Enemies.Count - 1); j >= 0; j--)
                {
                    diff = m_Enemies[j].transform.position - m_AttackBubbles[i].transform.position;
                    dist = diff.magnitude;
                    conflict = dist <= 0.8f;
                    if (conflict)
                    {
                        Destroy(m_AttackBubbles[i].gameObject);
                        m_AttackBubbles.RemoveAt(i);
                        Destroy(m_Enemies[j].gameObject);
                        m_Enemies.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }

    private void InputProc()
    {
        // åªç›ÇÃÉLÅ[É{Å[ÉhèÓïÒ
        m_Player1Speed = 4;
        m_Player2Speed = 4;
        m_Player1Angle = 5;
        m_Player2Angle = 5;
        var current = Keyboard.current;
        if (current != null)
        {
            if (current.zKey.isPressed)
            {
                m_Player1Speed = 6;
            }
            else if (current.xKey.isPressed)
            {
                m_Player1Speed = 8;
            }
            else if (current.aKey.isPressed)
            {
                m_Player1Speed = 6;
                m_Player1Angle = 10;
            }
            else if (current.sKey.isPressed)
            {
                m_Player1Speed = 8;
                m_Player1Angle = 10;
            }
            else if (current.qKey.isPressed)
            {
                m_Player1Speed = 6;
                m_Player1Angle = 20;
            }
            else if (current.wKey.isPressed)
            {
                m_Player1Speed = 8;
                m_Player1Angle = 20;
            }

            if (current.oem2Key.isPressed)
            {
                m_Player2Speed = 6;
            }
            else if (current.slashKey.isPressed)
            {
                m_Player2Speed = 8;
            }
            else if (current.backslashKey.isPressed)
            {
                m_Player2Speed = 6;
                m_Player2Angle = 10;
            }
            else if (current.quoteKey.isPressed)
            {
                m_Player2Speed = 8;
                m_Player2Angle = 10;
            }
            else if (current.rightBracketKey.isPressed)
            {
                m_Player2Speed = 6;
                m_Player2Angle = 20;
            }
            else if (current.leftBracketKey.isPressed)
            {
                m_Player2Speed = 8;
                m_Player2Angle = 20;
            }
        }
    }

    private void CreateBubble(int side, Vector3 pos, float angle, float speed)
    {
        Vector2 velocity;
        if (side == 0)
        {
            velocity = Vector2.right * speed;
            velocity = Quaternion.Euler(0, 0, angle) * velocity;
        }
        else if (side == 1)
        {
            velocity = Vector2.left * speed;
            velocity = Quaternion.Euler(0, 0, -angle) * velocity;
        }
        else
        {
            velocity = Vector2.right * speed;
            velocity = Quaternion.Euler(0, 0, angle) * velocity;
        }
        CreateBubble(side, pos, velocity);
    }

    private void CreateBubble(int side, Vector3 pos, Vector2 velocity)
    {
        var go = Instantiate(m_BubblePrefab, m_BubbleParent);
        go.transform.localPosition = pos;
        var comp = go.GetComponent<Bubble>();
        if (side == 0)
        {
            m_Player1Bubbles.Add(comp);
        }
        else if (side == 1)
        {
            m_Player2Bubbles.Add(comp);
        }
        else
        {
            m_AttackBubbles.Add(comp);
        }
        comp.SetParam(side, velocity);
    }
}
