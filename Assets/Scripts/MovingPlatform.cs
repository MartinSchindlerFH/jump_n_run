using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float platformSpeed;
    [SerializeField]
    private Vector3 start;
    [SerializeField]
    private Vector3 end;

    private Vector3 lastPosition;
    void FixedUpdate()
    {
        if (true)
        {
            lastPosition = transform.position;
            float pingPong = Mathf.PingPong(Time.fixedTime * this.platformSpeed, 1.0f);
            var newPosition = Vector3.Lerp(this.start, this.end, pingPong);
            this.transform.localPosition = newPosition;
        }
    }

    public Vector3 GetVelocity()
    {
        Debug.Log(this.transform.position - lastPosition * Time.deltaTime);
        return this.transform.position - lastPosition * Time.deltaTime;
    }
}
