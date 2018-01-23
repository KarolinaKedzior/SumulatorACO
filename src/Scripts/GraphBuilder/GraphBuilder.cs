using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;



public class GraphBuilder : MonoBehaviour
{
    public GameObject LinePrefab;
    public GameObject VerticePrefab;
    public GameObject myDrawPanel;
    public Slider evaporation, alpha, beta, pheromoneAmmount;

    public List<GameObject> vertices;
    public List<GameObject> edges;

    public static GameObject verticeFrom;
    public static GameObject foodPoint;
    public static GameObject antHillPoint;

    private GameObject verticeTo;
    private bool drawingLineStarted = false;


    void Start()
    {
        vertices = new List<GameObject>();
        edges = new List<GameObject>();
        verticeFrom = null;
        verticeTo = null;
        testBuildGrid();

    }
    public void DrawingLine()
    {
         verticeTo =
            EventSystem.current.currentSelectedGameObject;
        Button button = verticeTo.GetComponent<Button>();
        Vertice v_To = verticeTo.GetComponent<Vertice>();


            var button_color = button.colors;
            button_color.normalColor = Color.red;
            button_color.pressedColor = Color.gray;
            button.colors = button_color;


            if (verticeFrom != null && !verticeFrom.Equals(verticeTo)) {

            GameObject tmpEdge = CreateEdge( verticeFrom, verticeTo, evaporation.value);
                edges.Add(tmpEdge);

                verticeFrom.GetComponent<Vertice>().visibleNeighbours.Add(verticeTo.GetInstanceID());
                v_To.visibleNeighbours.Add(verticeFrom.GetInstanceID());
      
            verticeFrom = null;
              
            } else {
                verticeFrom = verticeTo;

            }
        

    }
    public void CreateVerticeOnClick()
    {
        Vector3 pos = Input.mousePosition;

            GameObject newVertice = CreateVertice( pos.x, pos.y);
            vertices.Add(newVertice);

    }
    public GameObject CreateVertice( float pos_x, float pos_y)
    {
        GameObject temp = Instantiate(VerticePrefab) as GameObject;
        temp.GetComponent<Vertice>().Initialize( pos_x, pos_y, 5);
        temp.transform.SetParent(myDrawPanel.transform, false);
        temp.transform.localScale = new Vector3(1, 1, 1);
        temp.GetComponent<Button>().onClick.AddListener(DrawingLine);
        return temp;
    }

    public GameObject CreateEdge(GameObject startP,GameObject endP, double evaporationValue)
    {
        GameObject new_line = Instantiate(LinePrefab) as GameObject;
        new_line.transform.SetParent(myDrawPanel.transform, false);
        new_line.GetComponent<Edge>()
            .Initialize( startP, endP, evaporationValue);
        return new_line;
    }
    public void ResetGraph()
    {
        for (int i = 0; i < edges.Count; i++)
        {
            Destroy(edges[i]);
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            Destroy(vertices[i]);
        }
        edges.Clear();
        vertices.Clear();

        vertices = new List<GameObject>();
        edges = new List<GameObject>();
        GraphBuilder.antHillPoint = null;
        GraphBuilder.foodPoint = null;
        verticeFrom = null;
        verticeTo = null;
    }
    public void  GenerateTestVertices()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                GameObject newVertice = CreateVertice(i * 100 + 100, j*100 +100);
                vertices.Add(newVertice);
                var button = newVertice.GetComponent<Button>();

                var button_color = button.colors;
                button_color.normalColor = Color.red;
                button_color.pressedColor = Color.green;
                button.colors = button_color;
            }
        }
    }
    private void testBuildGrid()
    {

       // GenerateTestVertices();

        //for (int i = 0; i < 4; i++)
        //{
            

        //    GameObject tmpEdge = CreateEdge(vertices[i], vertices[(i+5)*2], evaporation.value);
        //    edges.Add(tmpEdge);
        //    vertices[i].GetComponent<Vertice>().visibleNeighbours.Add(vertices[(i + 1) * 3].GetInstanceID());
        //    vertices[(i + 1) * 3].GetComponent<Vertice>().visibleNeighbours.Add(vertices[i].GetInstanceID());
        //    vertices[i ].GetComponent<Vertice>().visibleEdges.Add(tmpEdge.GetInstanceID());
        //    vertices[(i + 1) * 3].GetComponent<Vertice>().visibleEdges.Add(tmpEdge.GetInstanceID());
        //}


       



  
    }
  

}
