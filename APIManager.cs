/*
Titulo: "Wled"
Hecho en el año:2018 
-----
Title: "Wled"
Made in the year: 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using API_WLED;


public class APIManager : MonoBehaviour{

    [HideInInspector]
    public string device_URL =   "https://us-central1-X.cloudfunctions.net/api/dispositivos";
    const string login_URL=    "https://us-central1-X.cloudfunctions.net/api/login";
    public string API_Key = "eyJhXbGciOiJIUzXI1NiXIsInR5cCI6IkXpInJvbX";
    API_DB API_WLED_DB;
    

    [Header("Devices")]
    public MarkerController[] devices;

    void Start()
    {
        API_WLED_DB = GetComponent<API_DB>();
        Login();
    }

    public void Login()
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["x-access-token"] = API_Key;

        form.AddField("email", "X_D@App.com.ar");
        form.AddField("clave", "private");
        byte[] rawFormData = form.data;

        WWW request = new WWW(login_URL, rawFormData, headers);
        StartCoroutine(StartLogin(request));
    }
    public void GetDevices()
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["x-access-token"] = API_Key;

        WWW request = new WWW(device_URL, null, headers);
        StartCoroutine(StartGetDevices(request));
    }
    //___________________________________________________________

    IEnumerator StartLogin(WWW req)
    {
        yield return req;
        API_Key= API_WLED_DB.GetAPI_KEY(req.text);
        GetDevices();
    }
    IEnumerator StartGetDevices(WWW req)
    {
        yield return req;
        devices = FindObjectsOfType<MarkerController>();
        devices[0].InsertData(API_WLED_DB.GetAPI_FieldDevice(req.text));
        devices[0].GetStateLigth();
    }
}
