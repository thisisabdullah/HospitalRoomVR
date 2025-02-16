using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class QuestionsManager : MonoBehaviour
{
    [Header("UI Elements")] public List<Button> sectionButtons;
    public List<GameObject> sectionIndicators;
    public TMP_Text questionText;
    public TMP_Text questionCounterText;
    public Button nextButton;
    public Button previousButton;
    public Button finishButton;
    public Toggle trueToggle;
    public Toggle falseToggle;
    public TMP_Text feedbackText;
    public GameObject ThirdPartPanel;
    public GameObject QuestionPanel;
    public GameObject SectionPanel;

    [Header("ECG Monitor")] public TMP_Text HeartRateText;
    public TMP_Text TempratureText;
    public TMP_Text SopText;

    [Header("Data")] public List<Section> sections;

    private int currentSectionIndex = -1;
    private int currentQuestionIndex = 0;

    private Dictionary<string, string> userAnswers = new Dictionary<string, string>(); // Store answers

    void Start()
    {
        for (int i = 0; i < sectionButtons.Count; i++)
        {
            int index = i;
            //sectionButtons[i].onClick.AddListener(() => SelectSection(index));
        }

        trueToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, trueToggle, "True"));
        falseToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, falseToggle, "False"));

        nextButton.onClick.AddListener(NextQuestion);
        previousButton.onClick.AddListener(PreviousQuestion);
        finishButton.onClick.AddListener(Finish);
        ResetUI();
        SelectSection(0);

        InvokeRepeating(nameof(UpdateHeartRate), 0, 0.8f);
    }

    void UpdateHeartRate()
    {
        int simulatedHeartRate = Random.Range(70, 100);
        HeartRateText.text = simulatedHeartRate.ToString();

        int temp = Random.Range(90, 100);
        TempratureText.text = simulatedHeartRate.ToString();

        int sop = Random.Range(60, 100);
        SopText.text = simulatedHeartRate.ToString();
    }

    void ResetUI()
    {
        foreach (var indicator in sectionIndicators)
        {
            indicator.SetActive(false);
        }

        questionText.text = "";
        questionCounterText.text = "";
        feedbackText.text = "";

        trueToggle.isOn = false;
        falseToggle.isOn = false;
    }

    void SelectSection(int sectionIndex)
    {
        currentSectionIndex = sectionIndex;
        currentQuestionIndex = 0;

        ResetUI();
        sectionIndicators[sectionIndex].SetActive(true);
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentSectionIndex < 0 || currentSectionIndex >= sections.Count)
            return;

        Section currentSection = sections[currentSectionIndex];
        if (currentQuestionIndex >= currentSection.Questions.Count)
            return;

        Question currentQuestion = currentSection.Questions[currentQuestionIndex];
        questionText.text = currentQuestion.QuestionText;
        questionCounterText.text = $"{currentQuestionIndex + 1}/{currentSection.Questions.Count}";

        trueToggle.isOn = false;
        falseToggle.isOn = false;
        feedbackText.text = "";
    }

    void NextQuestion()
    {
        if (currentSectionIndex < 0 || currentSectionIndex >= sections.Count)
            return;

        Section currentSection = sections[currentSectionIndex];

        if (currentQuestionIndex < currentSection.Questions.Count - 1)
        {
            currentQuestionIndex++;
            DisplayQuestion();
        }
        else
        {
            if (currentSectionIndex < sections.Count - 1)
            {
                currentSectionIndex++;
                currentQuestionIndex = 0;
                SelectSection(currentSectionIndex);
            }
            else
            {
                nextButton.gameObject.SetActive(false);
                finishButton.gameObject.SetActive(true);
                Debug.Log("Quiz Finished");
            }
        }
    }

    public void Finish()
    {
        QuestionPanel.SetActive(false);
        SectionPanel.SetActive(false);
        ThirdPartPanel.SetActive(true);
        ExportToCSV();
    }

    public void PreviousQuestion()
    {
        if (currentSectionIndex < 0 || currentSectionIndex >= sections.Count)
            return;

        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            DisplayQuestion();
            nextButton.gameObject.SetActive(true);
            finishButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Already at the first question");
        }
    }

    void OnToggleChanged(bool isOn, Toggle currentToggle, string answer)
    {
        if (!isOn) return;

        if (currentToggle == trueToggle)
        {
            falseToggle.isOn = false;
        }
        else if (currentToggle == falseToggle)
        {
            trueToggle.isOn = false;
        }

        SaveAnswer(answer);
    }

    void SaveAnswer(string answer)
    {
        if (currentSectionIndex < 0 || currentSectionIndex >= sections.Count)
            return;

        Section currentSection = sections[currentSectionIndex];
        Question currentQuestion = currentSection.Questions[currentQuestionIndex];

        string key =
            $"Section {currentSectionIndex + 1} - Q{currentQuestionIndex + 1} - {currentQuestion.QuestionText}";
        userAnswers[key] = answer;
    }

    void ExportToCSV()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Question,Answer");

        foreach (var entry in userAnswers)
        {
            string question = entry.Key.Replace("\"", "\"\""); // Escape existing quotes
            string answer = entry.Value.Replace("\"", "\"\""); // Escape existing quotes

            sb.AppendLine($"\"{question}\",\"{answer}\""); // Wrap in quotes
        }
        
#if UNITY_EDITOR
        string filePath = Path.Combine(Application.persistentDataPath, "UserAnswers.csv");
#else
        string filePath = "/sdcard/Documents/UserAnswers.csv";
#endif

        File.WriteAllText(filePath, sb.ToString());
        Debug.Log($"CSV Exported: {filePath}");
    }
}

[System.Serializable]
public class Question
{
    public string QuestionText;
    public bool IsAnsweredCorrectly;
}

[System.Serializable]
public class Section
{
    public string SectionName;
    public List<Question> Questions;
}