/*This code will capture the accelerometer data from the 3 axis 
 * each 0.2 seconds and save it as .txt file in the installation 
 * folder of the app, with  the format
 * 'Sample number: acc in x,acc in y, acc in z'*/


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AccCapturer : MonoBehaviour
{
    string pathData;
    string pathTemplate;
    string data = "";
    bool cap = false;
    int capNum = 0;

    // Use this for initialization
    void Start ()
    {
        pathData = Application.persistentDataPath + "/AccData.txt";
        pathTemplate = Application.persistentDataPath + "/Template.txt";
        print(Application.persistentDataPath);
    }

    public void StartCapturing(float offset)
    {
        if (!cap)
        {
            Debug.Log("Capturing start");
            InvokeRepeating("MarchCaptureAcc", offset, 0.1f);
            cap = true;
            File.WriteAllText(pathData, data + capNum.ToString()+":");
            capNum++;
        }
        else {
            CancelInvoke();
            cap = false;
        }
    }

    void MarchCaptureAcc()
    {
        string xAcc = Input.acceleration.x.ToString();
        string yAcc = Input.acceleration.y.ToString();
        string zAcc = Input.acceleration.z.ToString();
        data = File.ReadAllText(pathData);
        data = data + ";" + xAcc + "," + yAcc + "," + zAcc;
        File.WriteAllText(pathData, data);
        Debug.Log("X= " + xAcc + " Y= " + yAcc + " Z= " + zAcc);
    }

    public void SaveTemplate(string templ)
    {
        File.WriteAllText(pathTemplate, templ);
    }
}
