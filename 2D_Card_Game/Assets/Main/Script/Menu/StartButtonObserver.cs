using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButtonObserver : IUIObserver
{
    private TextMeshProUGUI text;

    public StartButtonObserver(TextMeshProUGUI textComponent)
    {
        text = textComponent;
    }

    public void OnNotify(UIEventType eventType)
    {
        if (eventType == UIEventType.Start)
        {
            text.text = "Resume";
        }
    }
}
