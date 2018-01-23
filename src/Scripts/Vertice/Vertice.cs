using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Vertice : MonoBehaviour, IPointerClickHandler
{


    public List<int> visibleNeighbours { get; set; }
    public void OnPointerClick(PointerEventData eventData)
    {
        Color new_color = Color.black;
        var button = gameObject.GetComponent<Button>();
        var color = button.colors;



        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (button.colors.normalColor == Color.red)
            {

                if (GraphBuilder.foodPoint != null)
                {
                    var prev = GraphBuilder.foodPoint.GetComponent<Button>().colors;
                    prev.normalColor = Color.red;
                    GraphBuilder.foodPoint.GetComponent<Button>().colors = prev;
                }

                GraphBuilder.foodPoint = gameObject;
                color.normalColor = Color.yellow;
                button.colors = color;
                GraphBuilder.verticeFrom = null;
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {


            if (button.colors.normalColor == Color.red)
            {

                if (GraphBuilder.antHillPoint != null)
                {
                    var prev = GraphBuilder.antHillPoint.GetComponent<Button>().colors;
                    prev.normalColor = Color.red;
                    GraphBuilder.antHillPoint.GetComponent<Button>().colors = prev;
                }
                GraphBuilder.antHillPoint = gameObject;
                color.normalColor = Color.blue;
                button.colors = color;
                GraphBuilder.verticeFrom = null;
            }
        }
    }
    public void Initialize(float x, float y, double radius)
    {

        GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
        var t = gameObject.transform.position;
        t.z = -0.1f;
    }
    void Awake()
    {
        visibleNeighbours = new List<int>();
     //   visibleEdges = new List<int>();
    }

}
