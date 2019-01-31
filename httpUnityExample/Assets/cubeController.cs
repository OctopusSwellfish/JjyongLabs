using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cubeController : MonoBehaviour {
    float speed = 20.0f;
    OldClass previousObject = new OldClass();
    // Use this for initialization
    void Start () {

        // StartCoroutine(Upload());
        Upload();

        previousObject.x = transform.position.x;
        previousObject.y = transform.position.y;
        previousObject.z = transform.position.z;
        previousObject.testString = "테스트!!!";
    }
	
	// Update is called once per frame
	void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        h = h * speed * Time.deltaTime;
        v = v * speed * Time.deltaTime;

        transform.Translate(Vector3.right * h);
        transform.Translate(Vector3.forward * v);

        StartCoroutine(Upload());
    }

    IEnumerator Upload() {

        MyClass myObject = new MyClass();
        myObject.x = transform.position.x;
        myObject.y = transform.position.y;
        myObject.z = transform.position.z;
        myObject.testString = "테스트!!!";

        string tempX = myObject.x.ToString("N4");
        string tempY = myObject.y.ToString("N4");
        string tempZ = myObject.z.ToString("N4");
       /*
        Debug.Log(tempX);
        Debug.Log(tempY);
        Debug.Log(tempZ);
       
        Debug.Log("aaaaaaaaa");
       */       
        string tempOX = previousObject.x.ToString("N4");
        string tempOY = previousObject.y.ToString("N4");
        string tempOZ = previousObject.z.ToString("N4");
        /*
        Debug.Log(tempOX);
        Debug.Log(tempOY);
        Debug.Log(tempOZ);
            */

           if (tempOX.Equals(tempX) && tempOY.Equals(tempY) && tempOZ.Equals(tempZ))
           {
              // Debug.Log("0");
               yield break;

           }
           else
           {
            previousObject.x = transform.position.x;
            previousObject.y = transform.position.y;
            previousObject.z = transform.position.z;
           // Debug.Log("1");
               
               string jsonStringTrial = JsonUtility.ToJson(myObject);

               //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
               //formData.Add(new MultipartFormDataSection("aaaaa"));
               UnityWebRequest www = UnityWebRequest.Put("http://127.0.0.1:3000/test", jsonStringTrial);

               www.SetRequestHeader("Content-Type", "application/json");


               yield return www.SendWebRequest();

               if (www.isNetworkError || www.isHttpError)
               {
                   Debug.Log(www.error);
               }
               else
               {
                   Debug.Log("Form upload complete!");
               }

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
public class OldClass
{
    public float x, y, z;
    public string testString;

}