using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Shape { None = -1,  Cube = 0, Sphere, Capsule, Cylinder, Cone, Pyramid }
public enum gameState{ Idle, PlacingObjects, MovingObjects}
public class GameManager : MonoBehaviour
{
    public GameObject Indicator;
    public LayerMask FloorLayer;
    public LayerMask ObjectsLayer;

    public GameObject[] ShapesPrefab;

    private GameObject indicatorMesh;

    int SelectedShape = 0;

    bool PlacingObject = false;

    public static GameManager instance;

    public GameObject Content;

    public GameObject SelectedObject;

    private gameState CurrentState = gameState.Idle;

    public bool isColliding;

    public Material IndicatorMat;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        IndicatorMat.color = Color.green;
    }
    private void Update()
    {
        switch (CurrentState)
        {
            case gameState.Idle:
                UpdateIdle();               
                break;

            case gameState.PlacingObjects:
                UpdatePlacingObjects();                
                break;

            case gameState.MovingObjects:
                UpdateMovingObjects();
                break;
            default:
                break;
        }

        if (Input.GetMouseButtonDown(0) && PlacingObject)
        {
            placeObject();
        }

       
    }

    private void UpdateMovingObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 200, FloorLayer))
        {
            SelectedObject.transform.position = hitInfo.point;
        }
        if (Input.GetMouseButtonDown(0) && !isColliding)
        {
            //SelectedObject = null;
            SwitchState(gameState.Idle);
        }
        if(Input.GetKey(KeyCode.Q))
        {
            SelectedObject.transform.Rotate(Vector3.up);
        }
        if(Input.GetKey(KeyCode.E))
        {
            SelectedObject.transform.Rotate(-Vector3.up);
        }
    }

    private void UpdateIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200, ObjectsLayer))
            {
                SelectedObject = hitInfo.transform.parent.gameObject;

                SwitchState(gameState.MovingObjects);
            }
        }
    }

    private void UpdatePlacingObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 200, FloorLayer))
        {
            Debug.Log("FloorHit");
            Indicator.transform.position = hitInfo.point;
        }
        if (Input.GetMouseButtonDown(0) && !isColliding)
        {
            placeObject();
            SwitchState(gameState.Idle);
        }
    }

    void SwitchState(gameState state)
    {
        if(CurrentState != state)
            CurrentState = state;
    }

    public void placeObject()
    {
        //PlacingObject = false;
        if(indicatorMesh)
            Destroy(indicatorMesh);
        
        GameObject item = Instantiate(ShapesPrefab[SelectedShape], Indicator.transform.position, Quaternion.identity);
        item.transform.parent = Content.transform;
        item.tag = "Player";
        SelectedShape = (int)Shape.None;

    }
    public void ObjectSelected(Shape shape)
    {
        SelectedShape = (int)shape;
        //PlacingObject = true;
        indicatorMesh = Instantiate(ShapesPrefab[SelectedShape], Indicator.transform);
        indicatorMesh.tag = "Indicator";
        indicatorMesh.transform.GetChild(0).tag = "Indicator";
        indicatorMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = IndicatorMat;
        SwitchState(gameState.PlacingObjects);
    }

    public void DeleteSelected()
    {
        Destroy(SelectedObject.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Debug.LogError("OBJECT HIT");
        }
    }

    internal void Colliding(bool val)
    {
        if(val)
            IndicatorMat.color = Color.red;
        else
            IndicatorMat.color = Color.green;

        isColliding = val;
    }
}
