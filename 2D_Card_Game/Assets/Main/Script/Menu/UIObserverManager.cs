using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIObserverManager : MonoBehaviour
{
    public TextMeshProUGUI startButtonText;
    public TextMeshProUGUI soundButtonText;
    public Button startButton;
    public Button soundButton;
    public Button exitButton;
    public AudioSource audioSource;

    public List<TextMeshProUGUI> menuItems;
  


    private List<IUIObserver> observers = new List<IUIObserver>();
   

    void Start()
    {
        RegisterObservers();

        startButton.onClick.AddListener(() => NotifyObservers(UIEventType.Start));
        soundButton.onClick.AddListener(() => NotifyObservers(UIEventType.Sound));
        exitButton.onClick.AddListener(() => NotifyObservers(UIEventType.Exit));
    }

    void RegisterObservers()
    {
        observers.Add(new StartButtonObserver(startButtonText));
        observers.Add(new SoundButtonObserver(soundButtonText, audioSource));
        observers.Add(new ExitButtonObserver());
    }

    void NotifyObservers(UIEventType eventType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(eventType);
        }
    }


    
    public void SetActiveMenuItem(TextMeshProUGUI selectedItem)
    {
        foreach (var item in menuItems)
        {
            item.color = (item == selectedItem) ? Color.yellow : Color.white;
        }
    }

}

public enum UIEventType
{
    Start,
    Sound,
    Exit
}

public interface IUIObserver
{
    void OnNotify(UIEventType eventType);
}
