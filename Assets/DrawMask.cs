using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMask : MonoBehaviour
{
    [SerializeField] public float radius = 0.5f; 
    [SerializeField] public GameObject brush;

    bool twoPoints = false;

    Vector2 lastPos;
    Vector2 penultPos; 
    List<GameObject> brushesPool = new List<GameObject>(), activeBrushes = new List<GameObject>(); 

    public delegate void DrawHandler(Vector2 pos);
    public event DrawHandler onStartDraw;
    public event DrawHandler onEndDraw;
    public event DrawHandler drawing;

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (onStartDraw != null)
            {
                onStartDraw(VectorTransfer(Input.mousePosition));
            }
            penultPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (twoPoints && Vector2.Distance(Input.mousePosition, lastPos) > 0.5f) 
            {
                Vector2 pos = Input.mousePosition;
                float dis = Vector2.Distance(lastPos, pos);
                int segments = (int)(dis / radius); 
                segments = segments < 1 ? 1 : segments;
                Vector2[] points = Beizier(penultPos, lastPos, pos, segments); 
                for (int i = 0; i < points.Length; i++)
                {
                    InstanceBrush(VectorTransfer(points[i]));
                }
                if (drawing != null)
                {
                    drawing(VectorTransfer(Input.mousePosition));
                }
                lastPos = pos;
                penultPos = points[points.Length - 2];
            }
            else
            {
                twoPoints = true;
                lastPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (onEndDraw != null)
            {
                onEndDraw(VectorTransfer(Input.mousePosition));
            }
            twoPoints = false;
        }
    }

    private void OnPostRender()
    {
        InitBrushes();
    }

    void InitBrushes()
    {
        for (int i = 0; i < activeBrushes.Count; i++)
        {
            activeBrushes[i].SetActive(false);
            brushesPool.Add(activeBrushes[i]);
        }
        activeBrushes.Clear();
    }

    void InstanceBrush(Vector2 pos)
    {
        GameObject brushClone;
        if (brushesPool.Count > 0)
        {
            brushClone = brushesPool[brushesPool.Count - 1];
            brushesPool.RemoveAt(brushesPool.Count - 1);
        }
        else
        {
            brushClone = Instantiate(brush, pos, Quaternion.identity);
        }
        brushClone.transform.position = pos;

        brushClone.transform.localScale = Vector3.one * radius;
        brushClone.SetActive(true);
        activeBrushes.Add(brushClone);
    }

    public Vector2[] Beizier(Vector2 start, Vector2 mid, Vector2 end, int segments)
    {
        float d = 1f / segments;
        Vector2[] points = new Vector2[segments - 1];
        for (int i = 0; i < points.Length; i++)
        {
            float t = d * (i + 1);
            points[i] = (1 - t) * (1 - t) * mid + 2 * t * (1 - t) * start + t * t * end;
        }
        List<Vector2> rps = new List<Vector2>();
        rps.Add(mid);
        rps.AddRange(points);
        rps.Add(end);
        return rps.ToArray();
    }

    Vector2 VectorTransfer(Vector2 point)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(point.x, point.y, 0));
    }
}
