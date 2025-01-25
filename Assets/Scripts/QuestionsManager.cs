using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class QuestionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public List<Button> sectionButtons; // Buttons for sections
    public List<GameObject> sectionIndicators; // Indicators for selected sections
    public TMP_Text questionText; // Display for the current question
    public TMP_Text questionCounterText; // Display for question count
    public Button nextButton; // Next button
    public Toggle trueToggle; // Toggle for "True"
    public Toggle falseToggle;
    public TMP_Text feedbackText;

    [Header("ECG Monitor")] 
    public TMP_Text HeartRateText;

    [Header("Data")]
    public List<Section> sections; // List of sections and questions

    private int currentSectionIndex = -1; // Tracks the selected section
    private int currentQuestionIndex = 0; // Tracks the current question within a section

    void Start()
    {
        // Initialize section button listeners
        for (int i = 0; i < sectionButtons.Count; i++)
        {
            int index = i; // Prevent closure issue
            sectionButtons[i].onClick.AddListener(() => SelectSection(index));
        }

        trueToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, trueToggle, true));
        falseToggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, falseToggle, false));
        
        nextButton.onClick.AddListener(NextQuestion);
        ResetUI();
        SelectSection(0);
        
        InvokeRepeating(nameof(UpdateHeartRate), 0, 0.8f);
    }

    void UpdateHeartRate()
    {
        int simulatedHeartRate = Random.Range(80, 100);
        HeartRateText.text = simulatedHeartRate.ToString();
    }

    void ResetUI()
    {
        // Reset indicators and hide questions initially
        foreach (var indicator in sectionIndicators)
        {
            indicator.SetActive(false);
        }

        questionText.text = "";
        questionCounterText.text = "";
        feedbackText.text = "";
        
        // Reset toggles
        trueToggle.isOn = false;
        falseToggle.isOn = false;
    }

    void SelectSection(int sectionIndex)
    {
        currentSectionIndex = sectionIndex;
        currentQuestionIndex = 0;

        // Update UI for selected section
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

        // Update question text and counter
        Question currentQuestion = currentSection.Questions[currentQuestionIndex];
        questionText.text = currentQuestion.QuestionText;
        questionCounterText.text = $"{currentQuestionIndex + 1}/{currentSection.Questions.Count}";
        
        // Reset toggles for the new question
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
            // End of questions in the current section
            Debug.Log("End of section");
        }
    }
    
    void OnToggleChanged(bool isOn, Toggle currentToggle, bool selectedAnswer)
    {
        if (!isOn) return;

        // Ensure only one toggle is active at a time
        if (currentToggle == trueToggle)
        {
            falseToggle.isOn = false;
            Debug.Log("True selected");
        }
        else if (currentToggle == falseToggle)
        {
            trueToggle.isOn = false;
            Debug.Log("False selected");
        }
        
        //ValidateAnswer(selectedAnswer);
    }
    
    void ValidateAnswer(bool selectedAnswer)
    {
        if (currentSectionIndex < 0 || currentSectionIndex >= sections.Count)
            return;

        Section currentSection = sections[currentSectionIndex];
        Question currentQuestion = currentSection.Questions[currentQuestionIndex];

        if (selectedAnswer == currentQuestion.IsAnsweredCorrectly)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;
        }
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

