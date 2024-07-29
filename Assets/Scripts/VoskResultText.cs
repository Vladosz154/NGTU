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
        { "ноль", 0 },
        { "один", 1 },
        { "два", 2 },
        { "три", 3 },
        { "четыре", 4 },
        { "п€ть", 5 },
        { "шесть", 6 },
        { "семь", 7 },
        { "восемь", 8 },
        { "дев€ть", 9 },
        { "дес€ть", 10 },
        { "одиннадцать", 11 },
        { "двенадцать", 12 },
        { "тринадцать", 13 },
        { "четырнадцать", 14 },
        { "п€тнадцать", 15 },
        { "шестнадцать", 16 },
        { "семнадцать", 17 },
        { "восемнадцать", 18 },
        { "дев€тнадцать", 19 },
        { "двадцать", 20 },
        { "тридцать", 30 },
        { "сорок", 40 },
        { "п€тьдес€т", 50 },
        { "шестьдес€т", 60 },
        { "семьдес€т", 70 },
        { "восемьдес€т", 80 },
        { "дев€носто", 90 },
        { "сто", 100 },
        { "двести", 200 },
        { "триста", 300 },
        { "четыреста", 400 },
        { "п€тьсот", 500 },
        { "шестьсот", 600 },
        { "семьсот", 700 },
        { "восемьсот", 800 },
        { "дев€тьсот", 900 },
        { "тыс€ча", 1000 },
        { "тыс€чи", 1000 },
        { "тыс€ч", 1000 },
        { "миллион", 1000000 },
        { "миллиона", 1000000 },
        { "миллионов", 1000000 }
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
        ResultText.text = "–аспознавание:\n";
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
