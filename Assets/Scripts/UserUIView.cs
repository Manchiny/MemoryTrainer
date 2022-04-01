using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserUIView : MonoBehaviour
{
    [SerializeField] private Slider _playFieldSizeSlider;
    [SerializeField] private TextMeshProUGUI _playfieldSizeText;
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _checkButton;

    public Action<int> OnTimerChanged;
    public Action<int> OnPlayFieldSizeChanged;

    public Action OnStartButtonClick;
    public Action OnCheckButtonClick;

    public void Init(float playFieldSize, float showTime)
    {
        _playFieldSizeSlider.onValueChanged.AddListener(SetPlayFieldSize);
        _timeSlider.onValueChanged.AddListener(SetTime);

        _startButton.onClick.AddListener(() => OnStartButtonClick?.Invoke());
        _checkButton.onClick.AddListener(() => OnCheckButtonClick?.Invoke());

        SetSlidersValues(playFieldSize, showTime);
        UpdatePlayfieldSizeText();
        UpdateTimerText();
    }

    public void SetSlidersValues(float playFieldSize, float showTime)
    {
        SetPlayFieldSize(playFieldSize);
        SetTime(showTime);
    }

    private void SetPlayFieldSize(float value)
    {
        float steppedValue = Mathf.Round(value);
        _playFieldSizeSlider.value = steppedValue;
        UpdatePlayfieldSizeText();
        OnPlayFieldSizeChanged?.Invoke((int)steppedValue);
    }

    private void SetTime(float value)
    {
        float steppedValue = Mathf.Round(value);
        _timeSlider.value = steppedValue;
        UpdateTimerText();
        OnTimerChanged?.Invoke((int)steppedValue);
    }
    private void UpdateTimerText()
    {
        var value = (int)_timeSlider.value;
        _timerText.text = $"Время ожидания: {value} сек.";
    }

    private void UpdatePlayfieldSizeText()
    {
        var size = (int)_playFieldSizeSlider.value;
        _playfieldSizeText.text = $"Размер поля: {size}x{size}";
    }
}
