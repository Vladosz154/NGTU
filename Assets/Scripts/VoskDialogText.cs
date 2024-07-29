using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class VoskDialogText : MonoBehaviour
{
    public VoskSpeechToText VoskSpeechToText;
    public Text DialogText;
    public Animator animator;
    public VoiceProcessor VoiceProcessor;
    private bool isWordPauseNeeded = false; // Переменная для отслеживания пробела между словами
    public Button play;
    public Button clear;
    public Button input;


    public Slider slider;

    public Text Slidertext;

    public float maxSliderAmount = 3.0f;

    // Словарь, который связывает буквы с соответствующими анимациями жестов
    public Dictionary<char, string> gestureAnimations = new Dictionary<char, string>()
    {
        { 'а', "GestureA" },
        { 'б', "GestureB" },
        { 'в', "GestureV" },
        { 'г', "GestureG" },
        { 'д', "GestureD" },
        { 'е', "GestureE" },
        { 'ж', "GestureZH" },
        { 'з', "GestureZ" },
        { 'и', "GestureI" },
        { 'й', "GestureUH" },
        { 'к', "GestureK" },
        { 'л', "GestureL" },
        { 'м', "GestureM" },
        { 'н', "GestureN" },
        { 'о', "GestureO" },
        { 'п', "GestureP" },
        { 'р', "GestureR" },
        { 'с', "GestureS" },
        { 'т', "GestureT" },
        { 'у', "GestureU" },
        { 'ф', "GestureF" },
        { 'х', "GestureH" },
        { 'ц', "GestureTZ" },
        { 'ч', "GestureCH" },
        { 'ш', "GestureSH" },
        { 'щ', "GestureSHCH" },
        { 'ъ', "GestureTZZ" },
        { 'ы', "GestureIH" },
        { 'ь', "GestureMZ" },
        { 'э', "GestureEH" },
        { 'ю', "GestureYU" },
        { 'я', "GestureYA" },
        // Добавьте другие буквы и соответствующие анимации жестов по аналогии
    };

    public Dictionary<string, string> wordAnimations = new Dictionary<string, string>()
{
    { "привет", "GestureHello" },
    { "привет!", "GestureHello" },
    { "привет.", "GestureHello" },
    // Добавьте другие варианты произношения слова "привет" по аналогии
};

    // Параметры аниматора для управления переходами между анимациями
    private static readonly string TRIGGER_IDLE = "IdleTrigger";

    private Queue<string> animationQueue = new Queue<string>();

    private void Start()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    public void SliderChange(float value)
    {

        Slidertext.text = value.ToString("0.0");
    }

    private void Update()
    {
        animator.speed = slider.value;
    }


    public void OnTranscriptionResult(string transcription)
    {
        Debug.Log("Received transcription: " + transcription);
        DialogText.text = transcription;
        string lowerCaseTranscription = transcription.ToLower();
        animationQueue.Clear();
        isWordPauseNeeded = false; // Сброс флага перед началом нового текста
        VoiceProcessor.isAnimationPlaying = true;
        False();

        string[] words = Regex.Split(lowerCaseTranscription, @"\b"); // Разбиваем распознанную речь на слова

        foreach (string word in words)
        {
            // Проверяем, есть ли слово целиком в словаре
            if (wordAnimations.ContainsKey(word))
            {
                string animationName = wordAnimations[word];
                animationQueue.Enqueue(animationName);
                Debug.Log("Слово" + word);
            }
            else
            {
                // Проигрываем анимации для каждой буквы в слове
                foreach (char letter in word)
                {
                    if (gestureAnimations.ContainsKey(letter))
                    {
                        string animationName = gestureAnimations[letter];
                        animationQueue.Enqueue(animationName);
                    }
                }
                isWordPauseNeeded = true; // Устанавливаем флаг, чтобы добавить задержку между словами
            }
        }

        StartCoroutine(PlayAnimationsFromQueue());
    }


    IEnumerator PlayAnimationsFromQueue()
    {
        float originalSpeed = animator.speed;
        animator.speed = 3f;

        bool allAnimationsFinished = false;

        while (animationQueue.Count > 0)
        {
            string animationName = animationQueue.Dequeue();

            animator.SetTrigger(animationName);

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                if (VoskSpeechToText._isRecording == true)
                    VoskSpeechToText.StopRecording();
                yield return null;
            }

            // Проверяем, нужна ли задержка перед следующей анимацией
            if (isWordPauseNeeded)
            {
                yield return new WaitForSeconds(0.5f);
                isWordPauseNeeded = false; // Сбрасываем флаг после задержки
            }

            animator.SetTrigger(TRIGGER_IDLE);
            yield return new WaitForSeconds(0.5f);
        }

        // Все анимации завершены
        allAnimationsFinished = true;

        animator.speed = originalSpeed;
        VoiceProcessor.isAnimationPlaying = false;

        // Включаем кнопку только после завершения всех анимаций
        if (allAnimationsFinished)
            True();
    }

    public void PlayAnimationsFromKeyboardInput(string inputText)
    {
        animationQueue.Clear();
        isWordPauseNeeded = false; // Сброс флага перед началом нового текста
        False();

        string lowerCaseInput = inputText.ToLower();
        string[] words = Regex.Split(lowerCaseInput, @"\b"); // Разбиваем ввод на слова

        foreach (string word in words)
        {
            // Проверяем, есть ли слово целиком в словаре
            if (wordAnimations.ContainsKey(word))
            {
                string animationName = wordAnimations[word];
                animationQueue.Enqueue(animationName);
            }
            else
            {
                // Проигрываем анимации для каждой буквы в слове
                foreach (char letter in word)
                {
                    if (gestureAnimations.ContainsKey(letter))
                    {
                        string animationName = gestureAnimations[letter];
                        animationQueue.Enqueue(animationName);
                    }
                }
                isWordPauseNeeded = true; // Устанавливаем флаг, чтобы добавить задержку между словами
            }
        }

        StartCoroutine(PlayAnimationsFromQueue());
    }


    private void True()
    {
        play.interactable = true;
        clear.interactable = true;
        input.interactable = true;
    }

    private void False()
    {
        play.interactable = false;
        clear.interactable = false;
        input.interactable = false;
    }

}
