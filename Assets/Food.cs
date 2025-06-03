using UnityEngine;
using static BoidSimulationControl;

public class Food : MonoBehaviour
{
    public ControlMode controlMode = ControlMode.Food;
    public GameObject targetObject;
    public GameObject foodPrefab;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && controlMode == ControlMode.Food)
        {
            SpawnFood();
        }
    }
    private void SpawnFood()
    {
        Instantiate(foodPrefab, targetObject.transform.position, Random.rotation);
    }
   
}
