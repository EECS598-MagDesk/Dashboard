using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;

    private LineRenderer lineIndicator;
    private LineRenderer drawingCountainer;

    // Start is called before the first frame update
    void Start()
    {
        drawingCountainer = GameObject.Find("TestLineDraw").GetComponent<LineRenderer>();
        lineIndicator = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float DistanceToLine(Vector3 s, Vector3 e, Vector3 p)
    {
        // Calculate the projection of the point (px, py, pz) onto the x-z plane
        float projX = p.x;
        float projY = 0;
        float projZ = p.z;

        // Calculate the direction vector of the line
        float dirX = e.x - s.x;
        float dirY = 0;
        float dirZ = e.z - s.z;

        // Calculate the vector from the first point of the line to the projection of the point onto the line
        float fromX = projX - s.x;
        float fromY = 0;
        float fromZ = projZ - s.z;

        // Calculate the distance between the projection of the point onto the x-z plane and the line
        float distance = Mathf.Abs(fromX * dirZ - dirX * fromZ) / Mathf.Sqrt(dirX * dirX + dirZ * dirZ);

        return distance;
    }

    public float Get()
    {
        float ret = 0f;
        for (int i = 0; i < drawingCountainer.positionCount; i++)
        {
            ret += DistanceToLine(startPoint, endPoint, drawingCountainer.GetPosition(i));
        }
        return ret / drawingCountainer.positionCount;
    }

    public void SetLineOn(bool state)
    {
        if (state)
        {
            lineIndicator.positionCount = 2;
            lineIndicator.SetPositions(new Vector3[] { startPoint, endPoint });
        }
        else
        {
            lineIndicator.positionCount = 0;
            lineIndicator.SetPositions(new Vector3[] {});
        }
    }
}
