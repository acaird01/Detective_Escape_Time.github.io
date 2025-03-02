using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_NumberHint : MonoBehaviour
{
    public Renderer paintingRenderer;
    public Material leftMaterial;
    public Material rightMaterial;
    public Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }


    private void Update()
    {
        Vector3 playerDirection = player.position - transform.position;
        Vector3 paintingDirection = transform.forward;

        float dotProduct = Vector3.Dot(playerDirection.normalized, paintingDirection.normalized);

        // Add 90 degrees to the range of direction
        if (dotProduct > -0.9f)
        {
            paintingRenderer.material = rightMaterial;
        }
        else
        {
            paintingRenderer.material = leftMaterial;
        }
    }
}
