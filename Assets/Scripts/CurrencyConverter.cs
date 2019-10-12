using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using UnityEngine;
using UnityEngine.Networking;

public class CurrencyConverter : MonoBehaviour
{
    public ActionHandler actionHandler;
    public bool currencyListRequestActive;
    private AllRatesData downloadedRates;
    private void Start()
    {
        currencyListRequestActive = false;
    }

    public bool SendCurrencyListRequest(string inputStr)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            actionHandler.DisplayError("You need internet connection to use the app!");
            return false;
        }

        if (inputStr.Length == 0)
        {
            actionHandler.DisplayError("Please enter a valid date!");
            return false;
        }
        if(currencyListRequestActive) return false;
        
        var builderUri = new StringBuilder();
        builderUri.Append("https://api.exchangeratesapi.io/");
        builderUri.Append(inputStr);
        StartCoroutine(routine: GetCurrencyList(builderUri.ToString()));
        return true;
    }

    private  IEnumerator GetCurrencyList(string apiUri)
    {
        using (var webRequest = UnityWebRequest.Get(apiUri))
        {
            currencyListRequestActive = true;
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                actionHandler.DisplayError("Network Error: "+webRequest.error);
            }
            else
            {
                var output = webRequest.downloadHandler.text.ToUpper();
                if (output.Contains("ERROR"))
                {
                    actionHandler.DisplayError(output);
                }else{
                    downloadedRates = JsonUtility.FromJson<AllRatesData>(output);
                    output = output.Replace("\"", "");
                    var rateStartIndex = output.IndexOf("RATES", StringComparison.Ordinal);
                    var ratesContentStr = output.Substring(rateStartIndex);
                    var rateContentStartIndex = ratesContentStr.IndexOf('{');
                    var rateContentEndIndex = ratesContentStr.IndexOf('}');
                    var rateContentString = ratesContentStr.Substring(rateContentStartIndex, rateContentEndIndex - rateContentStartIndex);
                    var rateContentArray = rateContentString.Replace("}", "").Replace("{","").Split(',');
                    
                    var length = rateContentArray.Length;
                    if(downloadedRates.RATES == null){
                        downloadedRates.RATES = new Dictionary<string, float>();
                    }
                    else
                    {
                        downloadedRates.RATES.Clear();
                    }

                    for (var i = 0; i < length; i++)
                    {
                        var itemCodeAndRate = rateContentArray[i].Split(':');
                        downloadedRates.RATES.Add(itemCodeAndRate[0], float.Parse(itemCodeAndRate[1]));
                    }

                    var availableCurrencyCodes = downloadedRates.RATES.Keys.ToList();
                    actionHandler.RefreshDropdowns(availableCurrencyCodes);


                }
                    
            }
            currencyListRequestActive = false;
        }
    }

    public void SendConversionRequest(string inputAmountText, string inputCurrencyCode, string outputCurrencyCode)
    {
        var amount = float.Parse(inputAmountText);
        if(downloadedRates == null) return;
        if (Math.Abs(downloadedRates.RATES[inputCurrencyCode]) < 0.00000001)
        {
            actionHandler.DisplayError(outputCurrencyCode + " rate is downloaded incorrectly. Please enter a new date.");
        }

        var result = downloadedRates.RATES[outputCurrencyCode] / downloadedRates.RATES[inputCurrencyCode] * amount;
        actionHandler.SetOutput(amount, inputCurrencyCode, result, outputCurrencyCode);
        
    }
}
