using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Renderer boidrenderer;
    public float speedMax = 2;
    public float accelMax = 3;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.linearVelocity = Random.insideUnitSphere;
        GetComponent<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1));
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, rigidBody.linearVelocity, Color.red);
        
    }
    private void FixedUpdate()
    {
        float speed = rigidBody.linearVelocity.magnitude;

        if (speed > speedMax)
        {
            rigidBody.linearVelocity = rigidBody.linearVelocity * (speedMax / speed);
        }
        
    }
    public Vector3 Seek(Vector3 target, float acceleration)
    {
        Vector3 toTarget = target - transform.position;

        Vector3 toTargetNormalized = toTarget.normalized;

        Vector3 accel = toTargetNormalized * acceleration;

        return accel;
    }
    public Vector3 Pursue (Vector3 target, float acceleration, float desiredSpeed)
    {
        Vector3 toTarget = target - transform.position;

        Vector3 toTargetNormalized = toTarget.normalized;

        Vector3 desiredVelocity = toTargetNormalized * desiredSpeed;

        Vector3 deltaVel = desiredVelocity - rigidBody.linearVelocity;

        Vector3 acecel = deltaVel.normalized * acceleration;

        return acecel;
    }
}
