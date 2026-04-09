using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float platformSpeed;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;

    private Vector3 lastPosition;
    void FixedUpdate()
    {
        if (true)
        {
            lastPosition = transform.position;
            float pingPong = Mathf.PingPong(Time.fixedTime * this.platformSpeed, 1.0f);
            var newPosition = Vector3.Lerp(this.start.transform.position, this.end.transform.position, pingPong);
            this.transform.localPosition = newPosition;
        }
    }

    public Vector3 GetVelocity()
    {
        Debug.Log(this.transform.position - lastPosition * Time.deltaTime);
        return this.transform.position - lastPosition * Time.deltaTime;
    }
}
