using System.Collections;
using TMPro;
using System.IO;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuestionsManager : MonoBehaviour
{
    public bool ShowEnter = false;
    public bool StopMonitor = false;
    public string MrSceneName;
    
    [Header("UI Elements")] 
    public List<Button> sectionButtons;
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
    public GameObject[] InstructionPanels;
    public GameObject[] QuestionPanels;
    public GameObject EnterUI;

    [Header("ECG Monitor")] 
    public TMP_Text HeartRateText;
    public TMP_Text TempratureText;
    public TMP_Text SopText;
    public Text TotalText;
    public Text CurrentText;
    public Text ResultText;
    
    [Header("Computer Input")]
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public TMP_Text resultText;

    [Header("Data")] 
    public List<Section> sections;

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
    
        dropdown2?.onValueChanged.AddListener(delegate { CalculateDifference(); });
    
        trueToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, trueToggle, "True"));
        falseToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, falseToggle, "False"));

        nextButton.onClick.AddListener(NextQuestion);
        previousButton.onClick.AddListener(PreviousQuestion);
        finishButton.onClick.AddListener(Finish);

        ResetUI();

        LoadProgress(); // 👈 Load saved progress
        InvokeRepeating(nameof(UpdateHeartRate), 0, 0.5f);

        if (ShowEnter)
        {
            EnterUI.SetActive(false);
        }
    }

    void LoadProgress()
    {
        if (PlayerPrefs.HasKey("SavedSectionIndex") && PlayerPrefs.HasKey("SavedQuestionIndex"))
        {
            int savedSection = PlayerPrefs.GetInt("SavedSectionIndex");
            int savedQuestion = PlayerPrefs.GetInt("SavedQuestionIndex");

            if (savedSection >= 0 && savedSection < sections.Count)
            {
                currentSectionIndex = savedSection;
                currentQuestionIndex = Mathf.Clamp(savedQuestion, 0, sections[savedSection].Questions.Count - 1);

                ResetUI();
                sectionIndicators[currentSectionIndex].SetActive(true);
                DisplayQuestion();
            }
            else
            {
                SelectSection(0); // fallback
            }

            foreach (var panel in InstructionPanels)
            {
                panel.SetActive(false);
            } 
            
            foreach (var panel in QuestionPanels)
            {
                panel.SetActive(true);
            }

            ShowEnter = true;
        }
        else
        {
            SelectSection(0); // No saved progress, start fresh
        }
    }

    
    public void StopMonitorBool()
    {
        StopMonitor = true;
    }

    void UpdateHeartRate()
    {
        if (!StopMonitor)
        {
            int simulatedHeartRate = Random.Range(7, 10) * 10;  // 70, 80, 90
            HeartRateText.text = simulatedHeartRate.ToString();

            int temp = Random.Range(9, 10) * 10;  // 90, 100
            TempratureText.text = temp.ToString();

            int sop = Random.Range(6, 10) * 10;  // 60, 70, 80
            SopText.text = sop.ToString();

            int total = Random.Range(8, 12) * 10; // 80, 90, 100, 110
            int current = Random.Range(8, total / 10 + 1) * 10; // Ensures current is always ≤ total
            int result = total - current;
        
            TotalText.text = total + "/";
            CurrentText.text = current.ToString();
            ResultText.text = result.ToString();
        }
    }
    
    public void CalculateDifference()
    {
        int value1 = int.Parse(dropdown1.options[dropdown1.value].text);
        int value2 = int.Parse(dropdown2.options[dropdown2.value].text);

        int result = Mathf.Max(value1 - value2, 0); // Ensures no negative result

        resultText.text = result.ToString();
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

        SaveProgress(); // Save after moving to next
    }


    public void Finish()
    {
        QuestionPanel.SetActive(false);
        SectionPanel.SetActive(false);
        ThirdPartPanel.SetActive(true);

        PlayerPrefs.DeleteKey("SavedSectionIndex");
        PlayerPrefs.DeleteKey("SavedQuestionIndex");

        //ExportToCSV();
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
    
    void SaveProgress()
    {
        PlayerPrefs.SetInt("SavedSectionIndex", currentSectionIndex);
        PlayerPrefs.SetInt("SavedQuestionIndex", currentQuestionIndex);
        PlayerPrefs.Save();
    }

    [ContextMenu("MR")]
    public void LoadMrScene()
    {
        SceneManager.LoadScene(MrSceneName);
        //Application.Quit();
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