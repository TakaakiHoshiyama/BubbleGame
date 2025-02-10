using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_sprtRendere;

    private int m_Side;
    private Vector2 m_Velocity;

    public void SetParam(int side, Vector2 velocity)
    {
        m_Side = side;
        m_Velocity = velocity;
        m_sprtRendere.color = m_Side == 0 ? Color.blue : m_Side == 1 ? Color.red : Color.yellow;
    }

    public bool Move()
    {
        var dist = m_Velocity * Time.deltaTime;
        var pos = transform.localPosition;
        pos.x += dist.x;
        pos.y += dist.y;
        transform.localPosition = pos;

        return (pos.y > 5);
    }
}
