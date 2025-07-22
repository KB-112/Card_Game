using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]

public class TypewriterDialogue : MonoBehaviour
{
    [Header("Text & Settings")]
    public TextMeshProUGUI textMeshPro;
    public List<DialogueLines> dialogueLines;

    public float typingSpeed = 0.05f;
    public float nextLineDelay;
    public bool playOnStart = false;
    public string startStatus ; 

    private Coroutine typingCoroutine;

    private DialogueLines currentDialogue;
    private int currentLineIndex = 0;
    void OnEnable()
    {
        HoverColorChanger.OnGridSelected += HandleGridSelected;
    }

    void OnDisable()
    {
        HoverColorChanger.OnGridSelected -= HandleGridSelected;
    }

    private void HandleGridSelected(int col, int row)
    {
        textMeshPro.text = "";

    }
        void Start()
    {
        if (playOnStart && !string.IsNullOrEmpty(startStatus))
        {
            StartDialogue(startStatus);
        }
    }

    public void StartDialogue(string status)
    {
        currentDialogue = dialogueLines.Find(d => d.status == status);

        if (currentDialogue == null)
        {
            Debug.LogError("Dialogue with status '" + status + "' not found!");
            return;
        }

       
        PlayCurrentLine();
    }

    private void PlayCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogue.Count)
        {
            string line = currentDialogue.dialogue[currentLineIndex];
            typingCoroutine = StartCoroutine(TypeLine(line));
           
        }
    }

    private IEnumerator TypeLine(string line)
    {
        textMeshPro.text = "";

        foreach (char c in line)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(nextLineDelay);
        NextLine();
    }

    public void NextLine()
    {
        if (currentDialogue == null) return;

        if (currentLineIndex < currentDialogue.dialogue.Count - 1)
        {
            currentLineIndex++;
            PlayCurrentLine();
        }
        else
        {
            Debug.Log("Dialogue block finished.");
        }

        if (currentLineIndex == currentDialogue.dialogue.Count)
        {
            SkipTyping();


        }
    }
    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            textMeshPro.text = currentDialogue.dialogue[currentLineIndex];
        }
    }

    [System.Serializable]
    public class DialogueLines
    {
        public string status;
        [TextArea]
        public List<string> dialogue = new List<string>();
    }

}
