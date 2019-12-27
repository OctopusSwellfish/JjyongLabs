using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

    private Text textValue;
    private Slider slider;
    private GameObject handleSlideArea;
    private GameObject handle;

    private Text titleText;
    private Slider[] ArtSlider = new Slider[6];
    private Text[] ArtSliderTitle = new Text[6];
    private Text[] ArtSliderMaxValue = new Text[6];
    private Text[] ArtSliderMinValue = new Text[6];

    // Use this for initialization
    void Start () {
        slider = transform.GetComponent<Slider>();

        if (transform.gameObject.name.Equals("Slider"))
        {
            titleText = GameObject.Find("TitleText").GetComponent<Text>();
            for (int i = 0; i < ArtSlider.Length; i++)
                ArtSlider[i] = GameObject.Find("Axis" + (i + 1) + "Slider").transform.GetComponent<Slider>();
            for (int i = 0; i < ArtSliderTitle.Length; i++)
            {
                ArtSliderTitle[i] = ArtSlider[i].transform.Find("Title").gameObject.GetComponent<Text>();
                ArtSliderMaxValue[i] = ArtSlider[i].transform.Find("MaxValue").gameObject.GetComponent<Text>();
                ArtSliderMinValue[i] = ArtSlider[i].transform.Find("MinValue").gameObject.GetComponent<Text>();
            }
        }
        else
        {
            handleSlideArea = transform.Find("Handle Slide Area").gameObject;
            handle = handleSlideArea.transform.Find("Handle").gameObject;
            textValue = handle.transform.Find("Value").GetComponent<Text>();

            UpdateValue();
        }
    }
    public void UpdateValue ()
    {
        
        //slider.value = (int)slider.value;
        textValue.text = slider.value.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnChangedKinematics()
    {
        switch ((int)slider.value)
        {
            case 0:
                titleText.text = "Forward Kinematics";
                for (int i = 0; i < ArtSliderTitle.Length; i++)
                {
                    ArtSliderTitle[i].text = "Articulation " + (i + 1);
                }
                for (int i = 0; i < ArtSlider.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 3:
                        case 5:
                            ArtSlider[i].maxValue = 180;
                            ArtSliderMaxValue[i].text = "180°";
                            ArtSlider[i].minValue = -180;
                            ArtSliderMinValue[i].text = "-180°";
                            break;
                        default:
                            ArtSlider[i].maxValue = 90f;
                            ArtSliderMaxValue[i].text = "90°";
                            ArtSlider[i].minValue = -90f;
                            ArtSliderMinValue[i].text = "-90°";
                            break;

                    }
                }
                /*
                // default value
                ArtSlider[0].value = 0;
                ArtSlider[1].value = -45;
                ArtSlider[2].value = 90;
                ArtSlider[3].value = 0;
                ArtSlider[4].value = 45;
                ArtSlider[5].value = 0;
                */
                
                // test value
                ArtSlider[0].value = 0;
                ArtSlider[1].value = 0;
                ArtSlider[2].value = 0;
                ArtSlider[3].value = 0;
                ArtSlider[4].value = 0;
                ArtSlider[5].value = 0;
                
                
                break;

            case 1:
                titleText.text = "Inverse Kinematics";
                for (int i = 0; i < ArtSlider.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            ArtSlider[i].maxValue = 14f;
                            ArtSliderMaxValue[i].text = "14m";
                            ArtSlider[i].minValue = 3.01f;
                            ArtSliderMinValue[i].text = "3m";
                            ArtSlider[i].value = 8;
                            break;
                        case 1:
                            ArtSlider[i].maxValue = 4f;
                            ArtSliderMaxValue[i].text = "4m";
                            ArtSlider[i].minValue = -4f;
                            ArtSliderMinValue[i].text = "4m";
                            ArtSlider[i].value = 0;
                            break;
                        case 2:
                            ArtSlider[i].maxValue = 14f;
                            ArtSliderMaxValue[i].text = "14m";
                            ArtSlider[i].minValue = 0f;
                            ArtSliderMinValue[i].text = "0m";
                            ArtSlider[i].value = 8;
                            break;
                        default:
                            ArtSlider[i].maxValue = 90f;
                            ArtSliderMaxValue[i].text = "90°";
                            ArtSlider[i].minValue = -90f;
                            ArtSliderMinValue[i].text = "-90°";
                            ArtSlider[i].value = 0;
                            break;

                    }
                }
                ArtSliderTitle[0].text = "  PX";
                ArtSliderTitle[1].text = "  PY";
                ArtSliderTitle[2].text = "  PZ";
                ArtSliderTitle[3].text = "  RX";
                ArtSliderTitle[4].text = "  RY";
                ArtSliderTitle[5].text = "  RZ";

                break;
        }
    }
}
