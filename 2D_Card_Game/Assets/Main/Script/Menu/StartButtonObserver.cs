using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButtonObserver : IUIObserver
{
    private TextMeshProUGUI text;
    private GameObject layoutPanel;

    public StartButtonObserver(TextMeshProUGUI textComponent, GameObject layoutPanel    )
    {
        text = textComponent;
        this.layoutPanel = layoutPanel;
    }

    public void OnNotify(UIEventType eventType)
    {
        if (eventType == UIEventType.Start)
        {
          
            layoutPanel.SetActive(true);
        }
    }
}
