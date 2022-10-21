using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Shape { None = -1,  Cube = 0, Sphere, Capsule, Cylinder, Cone, Pyramid }

public class GameManager : MonoBehaviour
{
    public GameObject Indicator;
    public LayerMask FloorLayer;

    public GameObject[] ShapesPrefab;

    private GameObject indicatorMesh;

    int SelectedShape = 0;

    bool PlacingObject = false;

    public static GameManager instance;

    public GameObject Content;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (PlacingObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200, FloorLayer))
            {
                Debug.Log("FloorHit");
                Indicator.transform.position = hitInfo.point;
            }
        }

        if (Input.GetMouseButtonDown(0) && PlacingObject)
        {
            placeObject();
        }
    }

    public void placeObject()
    {
        PlacingObject = false;
        if(indicatorMesh)
            Destroy(indicatorMesh);
        
        Instantiate(ShapesPrefab[SelectedShape], Indicator.transform.position, Quaternion.identity);
        SelectedShape = (int)Shape.None;

    }
    public void ObjectSelected(Shape shape)
    {
        SelectedShape = (int)shape;
        PlacingObject = true;
        indicatorMesh = Instantiate(ShapesPrefab[SelectedShape], Indicator.transform);
    }
}
