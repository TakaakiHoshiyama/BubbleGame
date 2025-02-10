using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 m_Velocity;

    public void SetParam(Vector2 velocity)
    {
        m_Velocity = velocity;
    }

    public bool Move()
    {
        var dist = m_Velocity * Time.deltaTime;
        var pos = transform.localPosition;
        pos.x += dist.x;
        pos.y += dist.y;
        transform.localPosition = pos;

        return (pos.y < -5);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
