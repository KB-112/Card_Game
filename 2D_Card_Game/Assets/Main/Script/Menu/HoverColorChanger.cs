using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverColorChanger : MonoBehaviour, IPointerClickHandler
{
    // Static event declaration
    public static event System.Action<int, int> OnGridSelected;

    private TextMeshProUGUI text;
    private UIObserverManager manager;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        manager = FindObjectOfType<UIObserverManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager?.SetActiveMenuItem(text);

        if (TryParseGrid(text.text, out int col, out int row))
        {
            Debug.Log($"Parsed Grid: ({col}, {row})");
            OnGridSelected?.Invoke(col, row); 
        }
        else
        {
            Debug.LogWarning($"Could not parse grid from: {text.text}");
        }
    }

    private bool TryParseGrid(string input, out int col, out int row)
    {
        col = row = 0;

        string[] parts = input.ToLower().Split('x');

        if (parts.Length >= 2 &&
            int.TryParse(parts[0].Trim(), out col) &&
            int.TryParse(parts[1].Trim(), out row))
        {
            return true;
        }

        return false;
    }
}
