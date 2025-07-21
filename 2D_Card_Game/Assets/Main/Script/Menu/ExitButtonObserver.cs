using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonObserver : IUIObserver
{
    public void OnNotify(UIEventType eventType)
    {
        if (eventType == UIEventType.Exit)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
