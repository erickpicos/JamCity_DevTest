using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircles : MonoBehaviour
{
    public float power = 10f;
    public Vector2 minPower;
    public Vector2 maxPower;

    public float wallX_Dissipation = 0.7f;
    public float wallY_Dissipation = 0.7f;
    public float forceGravity = 9.8f;
    
    LineTrayectory tl;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    public GameObject prefabCircle;
    public int poolSize = 10;
    private List<GameObject> poolCircleList = new List<GameObject>();

    private GameObject instanceCircle;

    private void Start()
    {
        cam = Camera.main;
        tl = GetComponent<LineTrayectory>();
        AddCirclesToPool(poolSize);
    }
    
    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            instanceCircle = RequestCircleFromPool();
            instanceCircle.transform.position = startPoint;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            tl.RenderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;
            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            CircleObject circleObject = instanceCircle.GetComponent<CircleObject>();
            circleObject.rb.velocity = force * power;
            circleObject.forceGravity = forceGravity;
            circleObject.gravity = true;
            circleObject.wallX_Dissipation = wallX_Dissipation;
            circleObject.wallY_Dissipation = wallY_Dissipation;
            circleObject.ResetObjectCall();
            tl.EndLine();
        }
    }

    private void AddCirclesToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject circle = Instantiate(prefabCircle, transform);
            circle.SetActive(false);
            poolCircleList.Add(circle);
        }
    }
    private GameObject RequestCircleFromPool()
    {
        for (int i = 0; i < poolCircleList.Count; i++)
        {
            if (!poolCircleList[i].activeSelf)
            {
                poolCircleList[i].SetActive(true);
                return poolCircleList[i];
            }
        }
        AddCirclesToPool(1);
        poolCircleList[poolCircleList.Count-1].SetActive(true);
        return poolCircleList[poolCircleList.Count - 1];
    }
}
