using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_OrbitingObjects : MonoBehaviour
{
    [System.Serializable]
    public class OrbitingObject
    {
        public GameObject obj;        // The object to orbit
        public Vector3 orbitCenter;   // Custom orbit center for this object
        public float orbitRadius = 5f;  // Custom radius for this object
        public float orbitSpeed = 10f;  // Custom speed for this object
        public bool clockwise = true;   // Custom orbit direction for this object (true for clockwise, false for counterclockwise)
    }

    public OrbitingObject[] orbitingObjects;

    private void Start()
    {
        //foreach (OrbitingObject orbitingObject in orbitingObjects)
        //{
        //    if (orbitingObject.obj != null)
        //    {
        //        orbitingObject.orbitCenter = transform.position;
        //    }
        //}

        //foreach (OrbitingObject orbitingObject in orbitingObjects)
        //{
        //    if (orbitingObject.obj != null)
        //    {
        //        orbitingObject.obj.SetActive(true);
        //    }
        //}
    }


    void Update()
    {
        foreach (OrbitingObject orbitingObject in orbitingObjects)
        {
            if (orbitingObject.obj != null)
            {
                OrbitObject(orbitingObject);
            }
        }
    }


    private void OrbitObject(OrbitingObject orbitingObject)
    {
        // Calculate the current angle based on time, speed, and direction
        float directionMultiplier = orbitingObject.clockwise ? 1f : -1f;
        float angle = orbitingObject.orbitSpeed * directionMultiplier * Time.time;

        // Calculate the new position using sine and cosine for circular movement
        float x = Mathf.Cos(angle) * orbitingObject.orbitRadius;
        float z = Mathf.Sin(angle) * orbitingObject.orbitRadius;

        // Get the new position relative to the object's custom orbit center
        Vector3 newPosition = new Vector3(x, orbitingObject.orbitCenter.y, z) + orbitingObject.orbitCenter;

        // Calculate the direction of movement (from the current position to the new one)
        Vector3 direction = (newPosition - orbitingObject.obj.transform.position).normalized;

        // Rotate the object to face the direction it is moving and add 90 degrees to the rotation
        orbitingObject.obj.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);

        // Update the object's position
        orbitingObject.obj.transform.position = newPosition;
    }
}
