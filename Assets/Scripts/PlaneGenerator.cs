using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField] 
    private int planeSizeX, planeSizeY = 0;

    private GameObject[,] planes;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlanes();
        SetCameraPosition();
    }

    void CreatePlanes()
    {
        planes = new GameObject[planeSizeX, planeSizeY];
        
        for(int x = 0; x < planeSizeX; ++x)
        {
            for(int y = 0; y < planeSizeY; ++y)
            {
                GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.SetParent(this.transform);
                plane.GetComponent<Renderer>().material.color = new Color(255 - (x + y) , x + y, x + y);
                plane.transform.position = new Vector3(x * 10, 0, y * 10);
                planes[x, y] = plane;
            }
        }
    }

    void SetCameraPosition()
    {
        Renderer thisRenderer = GetComponent<Renderer>();
        var combinedBounds = thisRenderer.bounds;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var render in renderers) {
            if (render != thisRenderer) combinedBounds.Encapsulate(render.bounds);
        }

        float cameraDistance = 0.50f; // Constant factor
        Vector3 objectSizes = combinedBounds.max - combinedBounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * mainCamera.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.25f * objectSize; // Estimated offset from the center to the outside of the object
        mainCamera.transform.position = combinedBounds.center - distance * mainCamera.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Debug.Log(mouse.x + " , " + mouse.y + " , " + mouse.z);
            Debug.Log((int)mouse.x / 10 + " , " + (int)mouse.y / 10);
            var plane = planes[(int)mouse.x / 10, (int)mouse.y / 10];
            plane.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }
            


        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Pressed secondary button.");
        }
            

        if (Input.GetMouseButtonDown(2))
        {   
            Debug.Log("Pressed middle click.");
        }
            
    }
}
