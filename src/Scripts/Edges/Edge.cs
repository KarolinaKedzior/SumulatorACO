
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class Edge : MonoBehaviour
{
    public GameObject startP, endP;
    private double _lineLength;
    public double LineLength { get
        {
            return _lineLength;
        }
    }
    private double  feromonMinValue = 1;
    private double _evaporation;

    public double Evaporation
    {
        get
        {
            return _evaporation;
        }
        set
        {
            this._evaporation = 1 - (value / 100);
        }
    }
    private double _pheromone;
    public double Pheromone { get
        {
            return _pheromone;
        }
        }

    public void EvaporatePheromoneE()
    {
        _pheromone = feromonMinValue + (_pheromone * Evaporation);
    }

    public void Initialize( GameObject start, GameObject end, double evaporation_amount)
    {
        this.startP = start;
        this.endP = end;
        Vector2 s = this.startP.GetComponent<RectTransform>().anchoredPosition;
        Vector2 e = this.endP.GetComponent<RectTransform>().anchoredPosition;
        this.Evaporation = evaporation_amount;
        this._lineLength = Math.Sqrt(Math.Pow(Math.Abs(s.x - e.x), 2) + Math.Pow(Math.Abs(s.y - e.y), 2));
        this._pheromone = this.feromonMinValue;
        var tmp = GetComponent<LineRenderer>();

        tmp.SetWidth(0.1f, 0.1f);
        tmp.SetPosition(0, new Vector3(s.x, s.y, -1f));
        tmp.SetPosition(1, new Vector3(e.x, e.y, -1f));


    }

 
    public void LeavePheromoneE(double pheromoneAmmount, int lineLength)
    {
        _pheromone +=  (double)(pheromoneAmmount / lineLength);
    }
    
    public void ResetEvaporation()
    {
        this._pheromone = feromonMinValue;

    }



}
