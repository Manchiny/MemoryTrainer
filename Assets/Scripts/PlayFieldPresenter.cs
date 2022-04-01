using UnityEngine;
using UnityEngine.UI;

public class PlayFieldPresenter : MonoBehaviour
{
    [SerializeField] private UserUIView _userUIView;

    [SerializeField] private GridLayoutGroup _mainField;
    [SerializeField] private GridLayoutGroup _poolField;
    [SerializeField] private Cell _cellPrefab;

    private PlayField _playField;
    private void Start()
    {
        _playField = new PlayField(_mainField, _poolField, _cellPrefab);

        _userUIView.OnPlayFieldSizeChanged += _playField.OnPlayFieldSliderChanged;
        _userUIView.OnTimerChanged += _playField.OnTimerValueSet;
        _userUIView.OnStartButtonClick += _playField.StartNewGame;
        _userUIView.OnCheckButtonClick += _playField.Check;

        _userUIView.Init(_playField.FieldSize, _playField.ShowTime); 
        _playField.StartNewGame();  
    }
}
