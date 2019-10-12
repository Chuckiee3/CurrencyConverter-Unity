using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
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
    private TMP_InputField inputDateYear;
    [SerializeField]
    private TMP_InputField inputDateMonth;
    [SerializeField]
    private TMP_InputField inputDateDay;
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

    public void OnYearTextChanged()
    {
      
        if (string.IsNullOrEmpty(inputDateYear.text)) return;
        if (inputDateYear.text.Length != 4) return;
        var year = int.Parse(inputDateYear.text);
        if ( year <= minimumYear)
        {
            year = minimumYear;
            inputDateYear.text = year.ToString();
        }

        if (year > System.DateTime.Today.Year) {
            year = System.DateTime.Today.Year;
            inputDateYear.text = year.ToString();
        }

        inputDateMonth.Select();
        inputDateMonth.ActivateInputField();

    }

    public void OnMonthTextChanged()
    {
        if (string.IsNullOrEmpty(inputDateMonth.text)) return;
        var month = int.Parse(inputDateMonth.text);
        if (month <= 0)
        {
            month = 1;
        }else if (month > 12)
        {
            month = 12;
        }

        inputDateMonth.text = month.ToString();
    }
    public void OnDayTextChanged()
    {
        if (string.IsNullOrEmpty(inputDateDay.text)) return;
        var day = int.Parse(inputDateDay.text);
        if (day <= 0)
        {
            day = 1;
        }else if (day > 31)
        {
            day = 31;
        }

        inputDateDay.text = day.ToString();
    }

    public void OnGetCurrenciesTapped()
    {
        var builder = new StringBuilder();
        builder.Append(inputDateYear.text);
        builder.Append("-");
        builder.Append(inputDateMonth.text);
        builder.Append("-");
        builder.Append(inputDateDay.text);

        if (converter.SendCurrencyListRequest(builder.ToString()))
        {
            dateInfo.text = "Using data from " + builder.ToString();
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
