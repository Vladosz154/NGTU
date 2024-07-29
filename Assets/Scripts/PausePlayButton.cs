using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlayButton : MonoBehaviour
{
    public Sprite pauseButton;
    public Sprite playButton;

    private bool isPaused = false;

    private Image ButtonImage;

    private void Start()
    {
        ButtonImage = GetComponent<Image>();
        ButtonImage.sprite = pauseButton;
    }

    public void TogglePausePlay()
    {
        isPaused = !isPaused;

        ButtonImage.sprite = isPaused ? playButton : pauseButton;

        if (isPaused )
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;                                     
    }
}
