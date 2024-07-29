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
        // ������������� ������������� �������� ����
        recognizer = new KeywordRecognizer(word);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    // ���������� ������� ������������� �����
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        currentWord = args.text;
        AnimateAvatar(currentWord);
    }

    // ����� ��� �������� ������� �� ������ ������������� �����
    private void AnimateAvatar(string word)
    {
        // ������� �������� ����� ����������� �����
        avatarAnimator.SetBool("IsA", false);
        avatarAnimator.SetBool("IsB", false);


        // �������� ���� ��������������� ������
        foreach (char letter in word)
        {
            switch (letter)
            {
                case '�':
                    avatarAnimator.SetBool("IsA", true);
                    break;
                case '�':
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
