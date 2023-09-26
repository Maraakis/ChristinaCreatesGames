using System;
using System.Collections.Generic;
using System.Text; 
using TMPro;
using UnityEngine;

[Serializable]
public class Step
{
    public bool isCompleted;
    public string description;
}

public class Quest : MonoBehaviour
{
    [SerializeField] private List<Step> steps;
    [SerializeField] private TMP_Text textField;
    
    private void CheckProgress()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("It has been decreed that thou shalt do the following:");
        sb.AppendLine();

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].isCompleted)
            {
                sb.Append("<style=\"checked\">"); // Your style needs to be called "checked" to work with this.
                sb.Append(steps[i].description);
                sb.Append("</style>\n");
            }
            else
            {
                sb.Append("<style=\"unchecked\">"); // Your style needs to be called "unchecked" to work with this.
                sb.Append(steps[i].description);
                sb.Append("</style>\n");
            }
        }
        
        textField.SetText(sb.ToString());
    }

    private void OnValidate()
    {
        CheckProgress();
    }
}
