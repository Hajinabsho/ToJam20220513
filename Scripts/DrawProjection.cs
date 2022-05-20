using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    NewTankController tankController;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    public int numPoints = 50;
    public float timeBetweenPoints = 0.1f;
    public LayerMask CollidableLayers;

    void Start()
    {
        tankController = GetComponent<NewTankController>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
