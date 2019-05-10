using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBarBtn : MonoBehaviour {

    GameObject FileSideBar, ViewSideBar, ControlSideBar;

    void Awake()
    {
        FileSideBar = GameObject.Find("FileSideBar").gameObject;
        ViewSideBar = GameObject.Find("ViewSideBar").gameObject;
        ControlSideBar = GameObject.Find("ControlSideBar").gameObject;
    }

	// Use this for initialization
	void Start () {
        OnClickedFileBtn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickedFileBtn()
    {
        FileSideBar.SetActive(true);
        ControlSideBar.SetActive(false);
        ViewSideBar.SetActive(false);
    }

    public void OnClickedControlBtn()
    {
        FileSideBar.SetActive(false);
        ControlSideBar.SetActive(true);
        ViewSideBar.SetActive(false);
    }
    public void OnClickedViewBtn()
    {
        FileSideBar.SetActive(false);
        ControlSideBar.SetActive(false);
        ViewSideBar.SetActive(true);
    }
}
