using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverColorChanger : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI text;
    private UIObserverManager manager;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        manager = FindObjectOfType<UIObserverManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.SetActiveMenuItem(text);
    }
}
