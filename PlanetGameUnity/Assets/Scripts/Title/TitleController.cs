using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] AudioSource se;
    [SerializeField] Image titleImg;
    [SerializeField] TMP_InputField keywordField;
    [SerializeField] Button matchButton;

    enum TitleState { FadeIn, Display, FadeOut, Done }
    TitleState state = TitleState.FadeIn;
    float timer = 0f;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float displayDuration = 1f;
    void Start()
    {
        titleImg.canvasRenderer.SetAlpha(0f);
        keywordField.gameObject.SetActive(false);
        matchButton.gameObject.SetActive(false);
        se.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case TitleState.FadeIn:
                timer += Time.deltaTime;
                titleImg.canvasRenderer.SetAlpha(Mathf.Clamp01(timer / fadeDuration));
                if (timer >= fadeDuration)
                {
                    timer = 0f;
                    state = TitleState.Display;
                }
                break;

            case TitleState.Display:
                timer += Time.deltaTime;
                if (timer >= displayDuration)
                {
                    timer = 0f;
                    state = TitleState.FadeOut;
                }
                break;

            case TitleState.FadeOut:
                timer += Time.deltaTime;
                titleImg.canvasRenderer.SetAlpha(1f - Mathf.Clamp01(timer / fadeDuration));
                if (timer >= fadeDuration)
                {
                    timer = 0f;
                    state = TitleState.Done;
                    titleImg.gameObject.SetActive(false);
                    keywordField.gameObject.SetActive(true);
                    matchButton.gameObject.SetActive(true);
                }
                break;

            case TitleState.Done:
                break;
        }
    }
}
