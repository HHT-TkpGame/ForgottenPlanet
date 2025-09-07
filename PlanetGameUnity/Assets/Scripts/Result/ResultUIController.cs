using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] RectTransform contentRect;
    [SerializeField] GameObject endButton;
    [SerializeField] GameObject boostButton;
    readonly Vector2 StartPos = new Vector2 (0, -350);
    const float END_POS_Y = -20;
    public event Action OnEnded;
    bool canScroll;
    public event Action onScrollEnded;
    const float SCROLL_SPEED = 60f;
    const float BOOST_SPEED = SCROLL_SPEED * 3;
    float boostSpeed;
    [SerializeField] TMP_Text txtClues;
    [SerializeField] TMP_Text txtSelectTruth;
    [SerializeField] TMP_Text txtTruth;
    void Start()
    {
        contentRect.anchoredPosition = StartPos;
        endButton.SetActive(false);
        onScrollEnded += DisplayButton;
    }

    void Update()
    {
        if(!canScroll) { return; }
        if(contentRect.anchoredPosition.y < END_POS_Y )
        {
            // Translateだと画面のサイズや解像度の影響でスクロール速度が不安定なのでanchoredPositionを使う
            contentRect.anchoredPosition += Vector2.up * ((SCROLL_SPEED + boostSpeed) * Time.deltaTime);
        }
        else
        {
            canScroll = false;
            onScrollEnded.Invoke();
        }
    }

    void DisplayButton()
    {
        endButton.SetActive(true);
    }
    public void OnBoostButtonClicked()
    {
        boostSpeed = BOOST_SPEED;
    }
    public void OnEndButtonClicked()
    {
        OnEnded?.Invoke();
    }
    public void SetDisplayData(int answer, int foundClues)
    {
        txtClues.text = foundClues.ToString();
        txtSelectTruth.text = TruthNameConstants.TruthNames[answer-1];
        if (AnalyzeSaver.SelectedTruthId != default)
        {
            txtTruth.text = TruthNameConstants.TruthNames[AnalyzeSaver.SelectedTruthId];
        }
        else
        {
            txtTruth.text = TruthNameConstants.TruthNames[1];
        }
        canScroll = true;
    }
}
