using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class VoskResultText : MonoBehaviour
{
    public VoskSpeechToText VoskSpeechToText;
    public VoskDialogText VoskDialogText;
    public Text ResultText;
    public InputField InputField;
    public GameObject obj;

    private Dictionary<string, int> numberWords = new Dictionary<string, int>()
    {
        { "����", 0 },
        { "����", 1 },
        { "���", 2 },
        { "���", 3 },
        { "������", 4 },
        { "����", 5 },
        { "�����", 6 },
        { "����", 7 },
        { "������", 8 },
        { "������", 9 },
        { "������", 10 },
        { "�����������", 11 },
        { "����������", 12 },
        { "����������", 13 },
        { "������������", 14 },
        { "����������", 15 },
        { "�����������", 16 },
        { "����������", 17 },
        { "������������", 18 },
        { "������������", 19 },
        { "��������", 20 },
        { "��������", 30 },
        { "�����", 40 },
        { "���������", 50 },
        { "����������", 60 },
        { "���������", 70 },
        { "�����������", 80 },
        { "���������", 90 },
        { "���", 100 },
        { "������", 200 },
        { "������", 300 },
        { "���������", 400 },
        { "�������", 500 },
        { "��������", 600 },
        { "�������", 700 },
        { "���������", 800 },
        { "���������", 900 },
        { "������", 1000 },
        { "������", 1000 },
        { "�����", 1000 },
        { "�������", 1000000 },
        { "��������", 1000000 },
        { "���������", 1000000 }
    };

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string transcription)
    {
        Debug.Log(transcription);
        var result = new RecognitionResult(transcription);

        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                ResultText.text += ", ";
            }

            string phrase = result.Phrases[i].Text;
            phrase = ConvertNumberWordsToNumerals(phrase);
            ResultText.text += phrase;
        }
        ResultText.text += "\n";
    }

    public void CLearText()
    {
        ResultText.text = "�������������:\n";
    }

    public void AddTextFromInputField()
    {
        string newText = InputField.text;
        newText = ConvertNumberWordsToNumerals(newText);
        ResultText.text += newText + "\n";
        InputField.text = "";
        obj.SetActive(false);
        VoskDialogText.PlayAnimationsFromKeyboardInput(newText);
    }


    public void OpenInputField()
    {
        obj.SetActive(true);
        InputField.Select();
        InputField.text = "";
    }

    public void CloseInputField()
    {
        obj.SetActive(false);
    }

    private string ConvertNumberWordsToNumerals(string text)
    {
        string[] words = text.Split(' ');
        List<string> resultWords = new List<string>();
        int currentNumber = 0;
        bool hasNumber = false;

        foreach (string word in words)
        {
            if (numberWords.ContainsKey(word))
            {
                currentNumber += numberWords[word];
                hasNumber = true;
            }
            else
            {
                if (hasNumber)
                {
                    resultWords.Add(currentNumber.ToString());
                    currentNumber = 0;
                    hasNumber = false;
                }
                resultWords.Add(word);
            }
        }

        if (hasNumber)
        {
            resultWords.Add(currentNumber.ToString());
        }

        return string.Join(" ", resultWords);
    }
}
