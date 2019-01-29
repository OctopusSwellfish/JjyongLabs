using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cubeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Upload());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Upload() {
        MyClass myObject = new MyClass();
        //myObject.level = 1;
        //myObject.timeElapsed = 47.5f;
        //myObject.playerName = "Dr Charles Francis";
        myObject.x = transform.position.x;
        myObject.y = transform.position.y;
        myObject.z = transform.position.z;
        myObject.testString = "테스트!!!";

        string jsonStringTrial = JsonUtility.ToJson(myObject);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("aaaaa"));
        UnityWebRequest www = UnityWebRequest.Put("http://127.0.0.1:3000/test", jsonStringTrial);

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }else
        {
            Debug.Log("Form upload complete!");
        }
    }

}
public class MyClass
{
    /* public int level;
     public float timeElapsed;
     public string playerName;
     */
    public float x, y, z;
    public string testString;
}