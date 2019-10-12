using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class ActionHandler : MonoBehaviour
{
    [SerializeField]
    private CurrencyConverter converter;
    [Header("Feedback")]
    [SerializeField]
    private TextMeshProUGUI dateInfo;

    [SerializeField]
    private GameObject errorCanvas;
    [SerializeField]
    private TextMeshProUGUI errorText;
    
    
    [Header("Input")]
    [SerializeField]
    private TMP_InputField inputDate;
    [SerializeField]
    private TMP_InputField inputAmount;
    
    [SerializeField]
    private TMP_Dropdown inputDropdown;
    [SerializeField]
    private TMP_Dropdown outputDropdown;
    [SerializeField]
    private TextMeshProUGUI getCurrencyButtonText;
    [SerializeField] private GameObject inputPanel;
    
    
    [Header("Output")]
    [SerializeField]
    private TextMeshProUGUI inputLabel;
    [SerializeField]
    private TextMeshProUGUI outputLabel;
    [SerializeField]
    private TextMeshProUGUI oneToOneLAbel;

    [SerializeField] private GameObject outputPanel;
    private int dateCharacterCount;

    private int previousCharacterCount;

    
    
    
    public int minimumYear;

    public void OnDateTextChanged()
    {
        #region InputAddRemoveMinusSign
        
        //Add remove - to input Correctly
        var length = inputDate.text.Length;
        if(previousCharacterCount < length){
            if (length != 4 && length != 7)
            {
                previousCharacterCount = length;
                return;
            }
            inputDate.text += "-";
            previousCharacterCount = length + 1;
            inputDate.caretPosition = length+1;
        
        }
        else
        {
            if (length != 4 && length != 7) return;
            length--;
            previousCharacterCount = length;
            inputDate.text = inputDate.text.Substring(0, length);
        }
        
        #endregion

        #region YearInputCorrection
        
        var index = inputDate.text.IndexOf("-", StringComparison.Ordinal);
        if (length < 4 || index == -1 || index < 4) return;
        var year = int.Parse(inputDate.text.Substring(0,index));
        if (year <= minimumYear)
        {
            year = minimumYear;
            inputDate.text = year.ToString() + inputDate.text.Substring(4, inputDate.text.Length - 4);
            inputDate.caretPosition = length+1;
            return;
        }

        if (year <= System.DateTime.Today.Year) return;
        year = System.DateTime.Today.Year;
        inputDate.text = year.ToString() + inputDate.text.Substring(4, inputDate.text.Length - 4);
        inputDate.caretPosition = length+1;
        #endregion

        #region MonthInputCorrection

        

        #endregion
        #region DayInputCorrection

        

        #endregion

        #region FinalInputCorrection

        

        #endregion

        
    }

    public void OnGetCurrenciesTapped()
    {
        if (converter.SendCurrencyListRequest(inputDate.text))
        {
            dateInfo.text = "Using data from " + inputDate.text;
            getCurrencyButtonText.text = "Update Date";
            dateInfo.gameObject.SetActive(true);
        }

    }

    public void AmountEditEnd()
    {
        if(string.IsNullOrEmpty(inputAmount.text)) return;
        if (inputAmount.text[inputAmount.text.Length - 1].CompareTo('.') != 0) return;
        inputAmount.text = inputAmount.text.Substring(0, inputAmount.text.Length-1);
    }
   

    public void OnCalculateTapped()
    {
        converter.SendConversionRequest(inputAmount.text, inputDropdown.options[inputDropdown.value].text,outputDropdown.options[outputDropdown.value].text);
    }

    public void DisplayError(string errorMsg)
    {
        errorText.text = errorMsg;
        errorCanvas.SetActive(true);
    }
    public void HideError()
    {
        errorCanvas.SetActive(false);
    }

    public void RefreshDropdowns(List<string> availableCurrencyCodes)
    {
        inputPanel.SetActive(true);
        inputDropdown.ClearOptions();
        outputDropdown.ClearOptions();
        
        var len = availableCurrencyCodes.Count;
        for (var i = 0; i < len; i++)
        {
            var opt = new TMP_Dropdown.OptionData(availableCurrencyCodes[i], null);
            inputDropdown.options.Add(opt);
            outputDropdown.options.Add(opt);
        }
        inputDropdown.RefreshShownValue();
        outputDropdown.RefreshShownValue();
    }

    public void SetOutput(float amount, string inputCurrencyCode, float result, string outputCurrencyCode)
    {
        outputPanel.SetActive(true);
        inputLabel.text = amount.ToString("F3") + " " + inputCurrencyCode + " is";
        outputLabel.text = result.ToString("F3") + " " + outputCurrencyCode;
        oneToOneLAbel.text = "1 " + inputCurrencyCode + " = " + (result / amount).ToString("F5") + " " +
                             outputCurrencyCode;
    }
}
