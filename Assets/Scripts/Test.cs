using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Test : MonoBehaviour
{
    public Animator avatarAnimator;
    public string[] word;
    private string currentWord = "";
    private KeywordRecognizer recognizer;

    void Start()
    {
        // Инициализация распознавания ключевых слов
        recognizer = new KeywordRecognizer(word);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    // Обработчик события распознавания фразы
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        currentWord = args.text;
        AnimateAvatar(currentWord);
    }

    // Метод для анимации аватара на основе распознанного слова
    private void AnimateAvatar(string word)
    {
        // Очистка анимаций перед выполнением новых
        avatarAnimator.SetBool("IsA", false);
        avatarAnimator.SetBool("IsB", false);


        // Анимации букв соответствующих словам
        foreach (char letter in word)
        {
            switch (letter)
            {
                case 'а':
                    avatarAnimator.SetBool("IsA", true);
                    break;
                case 'б':
                    avatarAnimator.SetBool("IsA", true);
                    break;
            }
        }
    }

    void OnDestroy()
    {
        if (recognizer != null)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
