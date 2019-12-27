using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    private int time = 0;
    private GameObject end_effetor, CollisionSphere;
    private GameObject[] pos = new GameObject[6];
    private List<SaveDataObject> objs;

    // Use this for initialization
    void Start()
    {
        objs = new List<SaveDataObject>();
        end_effetor = GameObject.Find("End_Effetor");
        CollisionSphere = GameObject.Find("TestSphere");
        for (int i = 0; i < 6; i++)
        {
            pos[i] = GameObject.Find("save_pos" + (i+1));
        }

        if (end_effetor != null && CollisionSphere != null)
        {
            StartCoroutine(Save());
        }
        else
        {
            Debug.Log("Not Find End_Effetor");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Save()
    {
        var obj = new SaveDataObject(time, end_effetor.transform.position, CollisionSphere.transform.position,
            pos[0].transform.position, pos[1].transform.position, pos[2].transform.position,
            pos[3].transform.position, pos[4].transform.position, pos[5].transform.position);
        time++;
        objs.Add(obj);
        Sinbad.CsvUtil.SaveObjects(objs, "test.csv");
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Save());
    }
}
