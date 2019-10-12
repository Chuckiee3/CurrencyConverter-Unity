using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    [SerializeField]
    private CurrencyConverter converter;


    [SerializeField]
    private TMP_InputField inputDate;
    
    private int dateCharacterCount;

    private int previousCharacterCount;

    public int minimumYear = 1999;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        
        
    }

    public void OnGetCurrenciesTapped()
    {
    }

    public void OnCalculateTapped()
    {
    }
}
