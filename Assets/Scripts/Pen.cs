using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Pen : MonoBehaviour
{

    public bool draw = false;
    public bool drawSingleLine = true;
    public LineRenderer drawTarget;
    private bool prevDraw = false;
    private float drawPeriod = 0.01f;
    private float drawHeighThreshold = 0.5f;
    //public GameObject drawingContainer;
    public GameObject drawingContainer;
    private Coroutine drawingCoroutine;

    public Material inkMaterial;
    private float inkWidth = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        drawTarget = GameObject.Find("TestLineDraw").GetComponent<LineRenderer>();
    }

    IEnumerator DrawingCoroutine()
    {
        Queue<Vector3> pointQueue = new Queue<Vector3>();
        LineRenderer curLineContainer;
        if (drawSingleLine)
        {
            curLineContainer = drawTarget;
        }
        else
        {
            GameObject spawnedObj = new GameObject();
            curLineContainer = spawnedObj.AddComponent<LineRenderer>();
        }
        curLineContainer.material = this.inkMaterial;
        curLineContainer.widthMultiplier = inkWidth;
        pointQueue.Enqueue(transform.position);
        pointQueue.Enqueue(transform.position);
        while (draw)
        {
            pointQueue.Enqueue(transform.position);
            //Debug.Log(pointQueue.Count);
            curLineContainer.positionCount = pointQueue.Count;
            curLineContainer.SetPositions(pointQueue.ToArray());
            yield return new WaitForSeconds(drawPeriod);
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (transform.position.y < drawHeighThreshold)
        {
            draw = true;
        }
        else
        {
            draw = false;
        }
        */
        if (draw)
        {
            if (!prevDraw)
            {
                drawingCoroutine = StartCoroutine(DrawingCoroutine());
                prevDraw = true;
            }
        }
        else
        {
            if (prevDraw)
            {
                StopCoroutine(drawingCoroutine);
                prevDraw = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DrawingPad")
        {
            draw = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DrawingPad")
        {
            draw = false;
        }
    }
}
