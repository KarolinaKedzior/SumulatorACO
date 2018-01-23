using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

public class AntAlgorithm : MonoBehaviour {
    private bool started = false;
    public GameObject AntPrefab;
    GraphBuilder myGraph;
    private System.Random my_random;
    public Slider antNumber;
    public Slider evaporateTime;
    public Slider antSpeed;
    GameObject antHillPoint = null;
     private List<Coroutine> antCoroutinesList;
    private Coroutine evaporateCoroutine;
    public List<GameObject> allAntsList;
    
    
    void Start () {
        
        myGraph = gameObject.GetComponent<GraphBuilder>();
        my_random = new System.Random();
        antCoroutinesList = new List<Coroutine>();
        allAntsList = new List<GameObject>();
    }
    public virtual void StopAllAnts()
    {
        for (int i = 0; i < antCoroutinesList.Count; i++)
        {
            StopCoroutine(antCoroutinesList[i]);
        }
        StopAllCoroutines();
    }
    public void SimulationStart()
    {
        if (!started)
        {
            started = true;
            allAntsList = new List<GameObject>();
            for (int m = 0; m < myGraph.edges.Count; m++)
            {
                myGraph.edges[m].GetComponent<Edge>().ResetEvaporation();
            }

            bool cave = false;
            
            foreach (var point in myGraph.vertices)
            {
                if (point.GetComponent<Button>().colors.normalColor == Color.blue) // if cave
                {
                    cave = true;
                    this.antHillPoint = point;
                    break;
                }
                if (cave)
                {
                    break;
                }
            }
           
        
        }
        myGraph.pheromoneAmmount.enabled = false;
        var slider = myGraph.myDrawPanel.GetComponentsInChildren<Slider>();
        foreach (var sl in slider)
        {
            sl.enabled = false;
        }
        GameObject ant;
        for (int m = 0; m < antNumber.value; m++)
        {
            ant = Instantiate(AntPrefab) as GameObject;
            ant.transform.parent = myGraph.myDrawPanel.transform;
            ant.transform.localScale = new Vector3(1, 1, 1);
            ant.GetComponent<Ant>().Initialize(antHillPoint.GetInstanceID(), antHillPoint, antSpeed.value);
            allAntsList.Add(ant);
            antCoroutinesList.Add(StartCoroutine(AlgorithmCalculation(ant)));
        }
        evaporateCoroutine = StartCoroutine(EvaporatekCoroutine());

    }
    IEnumerator EvaporatekCoroutine()
    {
        for (;;)
        {
            EvaporatePheromone();
            yield return new WaitForSeconds(evaporateTime.value);
        }
    }
    IEnumerator AlgorithmCalculation(GameObject ant)
    {
        List<Tuple<double, int>> verticeProbability = new List<Tuple<double, int>>();
        List<int> visibleVertices;
        Ant antScript = ant.GetComponent<Ant>();
        int previousVerticeID = -1, currentVerticeID = antScript.CurrentPoint.GetInstanceID(), targetVerticeID = -1;
        double totalProbability;
        bool AVerticeExists;
        int a, b ,targetEdgeID = -1; 
        while (started)
        {
          
            verticeProbability.Clear();
            visibleVertices = antScript.GetNeighbours();
            visibleVertices.Shuffle<int>();

            CheckAntPosition(antScript);
            totalProbability = CalculateProbabilities(visibleVertices, verticeProbability, currentVerticeID);
            int i =0;
            if(verticeProbability.Count == 1)
            {
                targetVerticeID = verticeProbability[0].Item2;
            }
            else
            {
                AVerticeExists = antScript.NotVisitedVerticeExists();
                do
                {
                    i++;
                    if (i > 100){break;}
                    targetVerticeID = RandomElementByWeight(verticeProbability, totalProbability);
                } while (antScript.VerticeVisited(targetVerticeID) && AVerticeExists || targetVerticeID == currentVerticeID);
            }

            Vector2 targetVerticePosition = gameObject.transform.localPosition;
            targetEdgeID = -1;
            for (int j = 0; j < myGraph.edges.Count; j++)
            {
                a = myGraph.edges[j].GetComponent<Edge>().startP.GetInstanceID();
                b = myGraph.edges[j].GetComponent<Edge>().endP.GetInstanceID();
                if ( a ==currentVerticeID && b == targetVerticeID ||
                    b == currentVerticeID && a == targetVerticeID)
                    {
                        targetEdgeID = myGraph.edges[j].GetInstanceID();
                         break;
                    }
            }

            for (int j = 0; j < myGraph.vertices.Count; j++)
            {
                if (myGraph.vertices[j].GetInstanceID() == targetVerticeID)
                {
                    antScript.CurrentPoint = myGraph.vertices[j];
                    targetVerticePosition = antScript.CurrentPoint.GetComponent<RectTransform>().anchoredPosition;
                    break;
                }
            }
            antScript.AddVisitedVertice(targetVerticeID);
            antScript.AddVisitedEdge(targetEdgeID);
            previousVerticeID = currentVerticeID;
            currentVerticeID = targetVerticeID;
            targetVerticeID = -1;
            yield return StartCoroutine(antScript.MoveAnt(targetVerticePosition));
        }
    }
    public void StopAnts()
    {

        var slider = myGraph.myDrawPanel.GetComponents<Slider>();
        foreach (var sl in slider)
        {
            sl.enabled = true;
        }

        GameObject tmp;
        this.started = false;
        StopCoroutine(evaporateCoroutine);
        StopAllAnts();
        for (int i = 0; i < allAntsList.Count; i++)
        {
            tmp = allAntsList[i];
            Destroy(tmp);
            
        };
        for (int i = 0; i < myGraph.edges.Count; i++)
        {

        }
        for (int m = 0; m < myGraph.edges.Count; m++)
        {
            myGraph.edges[m].GetComponent<Edge>().ResetEvaporation();
           
        }
        allAntsList.Clear();
    }
    private void EvaporatePheromone()
    {
        for (int m = 0; m < myGraph.edges.Count; m++)
        {
            myGraph.edges[m].GetComponent<Edge>().EvaporatePheromoneE();
        }
    }
    private void LeavePheromone2(Ant ant)
    {
        List<int> temp = ant.VisitedEdges;
        int totalLen = 0;
        for (int k = 0; k < myGraph.edges.Count; k++)
        {
            if (temp.Contains(myGraph.edges[k].GetInstanceID()))
            {
                totalLen += (int)myGraph.edges[k].GetComponent<Edge>().LineLength;
            }
        }

        for (int k = 0; k < myGraph.edges.Count; k++)
        {
            if (temp.Contains(myGraph.edges[k].GetInstanceID()))
            {
                myGraph.edges[k].GetComponent<Edge>().LeavePheromoneE(myGraph.pheromoneAmmount.value, totalLen);
            }

        }
    }
    private void LeavePheromone(Ant ant)
    {
        List<int> temp = ant.VisitedEdges;
        int totalLen = 0;
        for (int k = 0; k < myGraph.edges.Count; k++)
        {
            if (temp.Contains(myGraph.edges[k].GetInstanceID()))
            {
                totalLen += (int)myGraph.edges[k].GetComponent<Edge>().LineLength;
            }
        }

        for (int k = 0; k < myGraph.edges.Count; k++)
        {
            if (temp.Contains(myGraph.edges[k].GetInstanceID()))
            {
                myGraph.edges[k].GetComponent<Edge>().LeavePheromoneE(myGraph.pheromoneAmmount.value, totalLen);
            }

        }
    }
    private double CalculateProbabilities(List<int> visibleVertices, List<Tuple<double, int>> verticeProbabilityList, int currentVerticeID)
    {
        double verticeProbability, probabilitySum = 0.0;
        int id1, id2;
        for (int i = 0; i < visibleVertices.Count; i++)
        {
            int targetVerticeID = visibleVertices[i];
            for (int j = 0; j < myGraph.edges.Count; j++)
            {
                
                id1 = myGraph.edges[j].GetComponent<Edge>().startP.GetInstanceID();
                id2 = myGraph.edges[j].GetComponent<Edge>().endP.GetInstanceID();
                if (id1 == currentVerticeID && id2 == targetVerticeID ||
                    id2 == currentVerticeID && id1 == targetVerticeID)
                {
                    Edge e = myGraph.edges[j].GetComponent<Edge>();
                    verticeProbability = Math.Pow(e.Pheromone, myGraph.alpha.value) *
                        Math.Pow( (1.0/e.LineLength) , myGraph.beta.value);
                    probabilitySum += verticeProbability;
                    verticeProbabilityList.Add(
                        new Tuple<double, int>(verticeProbability, targetVerticeID) );
                }
            }
        }
        if (probabilitySum == 0)
        {
            for (int i = 0; i < verticeProbabilityList.Count; i++)
            {
                verticeProbabilityList[i].Item1 /= probabilitySum;
            }
        }
        return probabilitySum;

    }
    private void CheckAntPosition(Ant antScript)
    {
        if (antScript.AtFood())
        {

            if (!antScript.FoundFood)
            {
                LeavePheromone(antScript);
                antScript.ClearVisitedVertices();
            }
            antScript.FoundFood = true;
        }
        else if (antScript.AtAntHill())
        {
            if (antScript.FoundFood)
            {
                LeavePheromone(antScript);
                antScript.ClearVisitedVertices();
            }
            antScript.FoundFood = false;
        }
    }
    private int RandomElementByWeight(List<Tuple<double, int>> sequence, double totalWeight)
    {
        double itemWeightIndex = my_random.NextDouble() * totalWeight;
        double currentWeightIndex = 0.0;

        foreach (var item in sequence)
        {
            currentWeightIndex += item.Item1;
            if (currentWeightIndex >= itemWeightIndex)
                return item.Item2;
        }
        return default(int);
    }
}

public class Tuple<T, U>
{
    public T Item1 { get; set; }
    public U Item2 { get; private set; }

    public Tuple(T item1, U item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}

public static class Tuple
{
    public static Tuple<T, U> Create<T, U>(T item1, U item2)
    {
        return new Tuple<T, U>(item1, item2);
    }
}

public static class ListShuffle
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
