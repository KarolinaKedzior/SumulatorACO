using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextUpdater : MonoBehaviour {

    Text textValue;
	void Start () {
        textValue = GetComponent<Text>();
	}
	
    public void textUpdate(float value)
    {
        textValue.text =  value.ToString();
    }
}
