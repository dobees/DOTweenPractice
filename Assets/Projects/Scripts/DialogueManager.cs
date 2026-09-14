using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string speaker;

    [TextArea]
    public string text;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text SpeakerText;
    [SerializeField] private TMP_Text DialogueText;
    [SerializeField] private Button NextButton;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueLine[] lines;

    private int currentIndex = -1;

    private void Start()
    {
        NextButton.onClick.AddListener(ShowNextLine);

        ShowNextLine();
    }

    private void ShowNextLine()
    {
        currentIndex++;

        if (currentIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentIndex];
        SpeakerText.text = line.speaker;
        DialogueText.text = line.text;
    }

    private void EndDialogue()
    {
        SpeakerText.text = "";
        DialogueText.text = "";

        NextButton.interactable = false;
    }

    private void OnDestroy()
    {
        NextButton.onClick.RemoveListener(ShowNextLine);
    }
}
