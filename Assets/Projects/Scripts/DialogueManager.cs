using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [Header("Animation")]
    [SerializeField] private RectTransform DialoguePanel;
    [SerializeField] private RectTransform Character;
    [SerializeField] private CanvasGroup CharacterCanvasGroup;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueLine[] lines;

    private int currentIndex = -1;

    private Vector2 characterOriginalPosition;

    private bool isAnimating;

    private void Start()
    {
        NextButton.onClick.AddListener(ShowNextLine);

        PlayStartAnimation();
    }

    private void PlayStartAnimation()
    {
        isAnimating = true;
        NextButton.interactable = false;

        characterOriginalPosition = Character.anchoredPosition;
        DialoguePanel.localScale = Vector3.zero;
        CharacterCanvasGroup.alpha = 0f;

        Character.anchoredPosition = characterOriginalPosition + new Vector2(-300, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            DialoguePanel
            .DOScale(Vector3.one, 0.3f) //0.3초동안 (1,1,1)사이즈로
            .SetEase(Ease.OutBack)); //목표 크기 살짝 넘어갔다가 복귀

        sequence.Join(
            CharacterCanvasGroup
            .DOFade(1f, 0.4f));

        sequence.Join(
            Character
            .DOAnchorPos(characterOriginalPosition, 0.4f)
            .SetEase(Ease.OutCubic));

        sequence.OnComplete(() =>
        {
            isAnimating = false;
            NextButton.interactable = true;

            ShowNextLine();
        });
    }

    private void ShowNextLine()
    {
        if (isAnimating)
        {
            return;
        }

        currentIndex++;

        if (currentIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentIndex];
        SpeakerText.text = line.speaker;
        DialogueText.text = line.text;

        DialoguePanel.DOPunchScale(
            Vector3.one * 0.02f,
            0.15f,
            4,
            0.5f
            );
    }

    private void EndDialogue()
    {
        isAnimating = true;
        NextButton.interactable = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            CharacterCanvasGroup
            .DOFade(0f, 0.3f));

        sequence.Join(
            Character.DOAnchorPos(
                characterOriginalPosition + new Vector2(120f, 0f),
                0.3f));

        sequence.Append(
            DialoguePanel
            .DOScale(Vector3.zero, 0.25f));

        SpeakerText.text = "";
        DialogueText.text = "";

        NextButton.interactable = false;
    }

    private void OnDestroy()
    {
        NextButton.onClick.RemoveListener(ShowNextLine);
    }
}
