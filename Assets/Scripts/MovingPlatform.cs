using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointA, pointB;
    [SerializeField] float speed = 10f;
    [SerializeField] float delay = 1f;
    [SerializeField] GameObject platform;
    [SerializeField] bool active = true;

    private Vector3 targetPosition;

    private Vector3 lastPosition;

    private void Start()
    {
        platform.transform.position = pointA.transform.position;
        targetPosition = pointB.transform.position;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (active)
        {
            while ((targetPosition - platform.transform.position).sqrMagnitude > 0.01f)
            {
                lastPosition = platform.transform.position;
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // Pauses untill next frame
            }
            targetPosition = targetPosition == pointA.transform.position ? pointB.transform.position : pointA.transform.position;
            yield return new WaitForSeconds(delay);
        }
    }

    public Vector3 GetVelocity()
    {
        return platform.transform.position - lastPosition * Time.deltaTime;
    }
}
