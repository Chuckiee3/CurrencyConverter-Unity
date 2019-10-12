using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class CurrencyConverter : MonoBehaviour
{
    public void CheckInput(string dateStr)
    {
        
    }

    public void SendRequest(int year, int month, int day)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection.");
            return;
        }
        var builder = new StringBuilder();
        var builderUri = new StringBuilder();
 
        builder.Append(year);
        builder.Append('-');
        builder.Append(month);
        builder.Append('-');
        builder.Append(day);


        var date = builder.ToString();
        builderUri.Append("https://api.exchangeratesapi.io/");
        builderUri.Append(date);
        StartCoroutine(routine: GetRequest(builderUri.ToString()));

    }

    private static IEnumerator GetRequest(string apiUri)
    {
        using (var webRequest = UnityWebRequest.Get(apiUri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log("Network error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }
}
