using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using LitJson;

public class TestWebClient : MonoBehaviour {

    void Start () {
        WebClient wc = new WebClient();
        string json = wc.DownloadString("http://appadmin.com/api/arproduct/checkDatasetVersion");
        if (json != null)
        {
            Debug.Log(json);
            RequestTotalDatasetModel requestDataset = JsonMapper.ToObject<RequestTotalDatasetModel>(json);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
