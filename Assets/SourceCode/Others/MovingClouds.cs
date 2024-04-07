using System.Collections;
using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1.0f;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);

        if(fractionOfJourney >= 1)
        {
            // Swap the points when target is reached
            Transform temp = pointA;
            pointA = pointB;
            pointB = temp;
            startTime = Time.time;
        }
    }
}


