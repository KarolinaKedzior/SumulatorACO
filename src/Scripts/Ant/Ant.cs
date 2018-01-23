using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Ant : MonoBehaviour
{

    public void PrintDebug()
    {
        Debug.Log("AntID: " + gameObject.GetInstanceID() + "\nVisited Edges: ");
        for (int i = 0; i < VisitedEdges.Count; i++)
        {
            Debug.Log("EdgeID: " + VisitedEdges[i] + ",");
        }
        Debug.Log("antScriptEND");
    }
    public bool animationStopped;
    private bool _foundFood = false;
    public bool FoundFood {
        get
        {
            return _foundFood;
        }
        set
        {
            if (value)
            {
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Ant2");
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Ant");
            }
            _foundFood = value;
        }
    }


  //  public int _previousVerticeID;
  //  public int _currentVerticeID;

    //public int CurrentVerticeID { get
    //    {
    //        return _currentVerticeID;
    //    }
    //    set
    //    {
    //        previousVerticeID = _currentVerticeID;
    //        _currentVerticeID = value;
          
    //    }
    //}



    public List<int> VisitedEdges;
    public List<int> VisitedVertices;
    private RectTransform objectPosition;
    private GameObject _currentPoint;
    public GameObject CurrentPoint
    {
        get
        {
            return _currentPoint;
        }

         set
        {
            _currentPoint = value;
          //  CurrentVerticeID = _currentPoint.GetInstanceID();
        }
    }
    private float speed = 1.0F;


    public List< Tuple< Tuple<int,int>, int> > visited;

    public List<int> GetNeighbours()
    {
        return CurrentPoint.GetComponent<Vertice>().visibleNeighbours;
    }



  
     void Awake()
    {
        objectPosition = GetComponent<RectTransform>();
        
    }

    public IEnumerator MoveAnt(Vector2 targetPosition)
    {
        UpdateRotation(targetPosition);
        while (  !animationStopped &&  Vector3.Distance(objectPosition.anchoredPosition,
            targetPosition) > 0.5f)
        {
            float step = speed * Time.deltaTime;
            objectPosition.anchoredPosition = Vector3.MoveTowards(objectPosition.anchoredPosition, targetPosition, step);
            yield return null;

        }
    }
    public void Initialize(int id ,GameObject point, float speedIN = 1.0f)
    {
        this.VisitedEdges = new List<int>();
        VisitedVertices = new List<int>();
        this.speed = speedIN;
        this._foundFood = false;
        this.VisitedEdges = new List<int>();
        this.CurrentPoint = point;
        this.animationStopped = false;
        this.AddVisitedVertice(point.GetInstanceID());
        
        GetComponent<RectTransform>().anchoredPosition = point.GetComponent<RectTransform>().anchoredPosition;


    }

    public bool AtAntHill()
    {
        return CurrentPoint.GetComponent<Button>().colors.normalColor == Color.blue;
    }
    public bool AtFood()
    {
        return CurrentPoint.GetComponent<Button>().colors.normalColor == Color.yellow;
    }
    public void ClearVisitedVertices()
    {

        VisitedEdges.Clear();
        VisitedVertices.Clear();
        AddVisitedVertice(CurrentPoint.GetInstanceID());
       // Debug.Log("Reste, current ID " + CurrentPoint.GetInstanceID());
    }
 
    public bool VerticeVisited(int verticeID)
    {
        return VisitedVertices.Contains(verticeID);
    }

    
    public void AddVisitedVertice(int verticeID)
    {
        if (!VisitedVertices.Contains(verticeID) || VisitedVertices.Count == 0)
        {
            VisitedVertices.Add(verticeID);
        //    Debug.Log("Dodano VisibleVertica");
        }
    }

    public void AddVisitedEdge(int edgeID)
    {
        if (!VisitedEdges.Contains(edgeID))
        {
            VisitedEdges.Add(edgeID);
        }
    }

    public bool NotVisitedVerticeExists()
    {
        List<int> visibleVertices = GetNeighbours();
        for (int i = 0; i < visibleVertices.Count; i++)
        {
            if ( !VisitedVertices.Contains(visibleVertices[i] ) )
            {
                return true;
            }
        }
        return false;
    }


    public void UpdateRotation(Vector2 targetPosition)
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(targetPosition) - transform.position;
        diff.Normalize();
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90);

    }
}

