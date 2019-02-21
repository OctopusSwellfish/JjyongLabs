using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

    private Text textValue;
    private Slider slider;
    private GameObject handleSlideArea;
    private GameObject handle;

	// Use this for initialization
	void Start () {
        handleSlideArea = transform.Find("Handle Slide Area").gameObject;
        handle = handleSlideArea.transform.Find("Handle").gameObject;
        textValue = handle.transform.Find("Value").GetComponent<Text>();

        slider = transform.GetComponent<Slider>();
        
        UpdateValue();
    }
    public void UpdateValue ()
    {
        slider.value = ((int)slider.value / 5) * 5;
        textValue.text = slider.value.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
