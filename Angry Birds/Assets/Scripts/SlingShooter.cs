using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;
    private Vector2 _startPos; // menyimpan titik awal sebelum karet ditarik

    [SerializeField]
    private float _radius = 0.75f; // panjang maksimal dari tali yang bisa ditarik

    [SerializeField]
    private float _throwSpeed = 30f; // kecepatan awal yang diberikan ketapel pada saat melontarkan burung

    private Bird _bird;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        Debug.Log("posisi awal " + _startPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject); //move position bird to ketapel
        Collider.enabled = true;
    }

    private void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        Debug.Log((Vector2)transform.position);
        Debug.Log("velocity(arah) " + velocity);
        float distance = Vector2.Distance(_startPos, transform.position);
        Debug.Log("distance " + distance);

        _bird.Shoot(velocity, distance, _throwSpeed);//bird

        // Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    private void OnMouseDrag()
    {
        // Mengubah posisi maouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mouse position " + p);
        // Hitung supaya 'karet' beradaa dalam radius yang ditentukan
        Vector2 dir = p - _startPos;
        if(dir.sqrMagnitude > _radius)
        {
            dir = dir.normalized * _radius;
        }
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position); 

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplaTrajectory(distance);
    }

    private void DisplaTrajectory(float distance)
    {
        if(_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        // Posisi awal trajectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // Velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for(int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }
}
