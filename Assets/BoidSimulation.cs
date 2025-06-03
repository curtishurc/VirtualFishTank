using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class BoidSimulationControl : MonoBehaviour
{
    public GameObject boidprefab = null;
    public int boidsToSpawn = 10;
    public List<Boid> boids = null;
    public Rigidbody rigidBody;
    public GameObject targetObject;
   
    public enum ControlMode
    {
        Seek,
        Pursue,
        Food,
        Obstacle
    }
    public ControlMode controlMode = ControlMode.Seek;
    public ControlMode controlModePursue= ControlMode.Pursue;
    
    private void Start()
    {
        boids = new List<Boid>();
        
        targetObject = GameObject.Find("target");
        for (int i = 0; i < boidsToSpawn; i++)
        {
     
            GameObject spawnedBoid = Instantiate(boidprefab, new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(0, 0.7f), Random.Range(-0.4f, 0.4f)), Random.rotation);
            Boid boidComponent = spawnedBoid.GetComponent<Boid>();
            boids.Add(boidComponent);
        }

    }
    private void FixedUpdate()
    {
        for (int i = 0; i < boids.Count;i++)
        {
            float foodSeekRadius = 0.5f;
            Collider[] colliders = Physics.OverlapSphere(boids[i].transform.position, foodSeekRadius);
            foreach (Collider collider in colliders)
            {
                Food food = collider.GetComponent<Food>();
                if (food != null)
                {
                    Vector3 accel = boids[i].Seek(collider.transform.position, boids[i].accelMax);
                    boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
                }
            }

        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo; //will store information if any intersection
        bool didHit = Physics.Raycast(ray, out hitInfo, 100);//Raycast will returm false if miss, true if hits.
                                                             //will also fill information in the passed perimiter wiht info about what it hit
        if (didHit)
        {
            targetObject.transform.position = hitInfo.point;
        }
    }

    private void Update()
    {
        control();
        AlignToVelocity();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            controlMode = ControlMode.Seek;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            controlMode = ControlMode.Pursue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            controlMode = ControlMode.Food;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            controlMode = ControlMode.Obstacle;
        }
    }
    void control()
    {
        switch (controlMode)
        {
            case ControlMode.Seek:
                {
                    SeekModeControl();
                    break;
                }
            case ControlMode.Pursue:
                {
                    PursueModeControl();
                    break;
                }
        }
    }
    private void SeekModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Seek(targetObject.transform.position, boids[i].accelMax);
            Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            if (Input.GetMouseButtonDown(0))
            {
                boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                boids[i].rigidBody.linearVelocity -= accel * Time.fixedDeltaTime;
            }
        }
    }
    private void PursueModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Pursue (targetObject.transform.position, boids[i].accelMax, boids[i].speedMax);
            boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
            Debug.DrawRay(boids[i].transform.position, accel, Color.green);
        }
    }
    
    public void AlignToVelocity()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, rigidBody.linearVelocity.normalized, Mathf.Deg2Rad * 1800 * Time.deltaTime, 100);   
    }
    
}



