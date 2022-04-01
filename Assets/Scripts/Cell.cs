using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private Color _colorMatch;
    [SerializeField] private Color _colorMismatch;

    private int _currentNumber;
    private CellContent _baseCellContent;
    private CellContent _newCellContent;
    public int Number => _currentNumber;
    private bool _isMainField;
    private void Awake()
    {
        _baseCellContent = GetComponentInChildren<CellContent>();
    }
    public void Init(bool isMainField)
    {
        _isMainField = isMainField;
        if(isMainField)
        {
            BlockContent();
        }
        else
        {
            _newCellContent = _baseCellContent;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        var newContent = eventData.pointerDrag.GetComponent<CellContent>();      
        var oldCell = newContent.Cell;

        if (_newCellContent != null)
        {
            oldCell.SetContent(oldCell, _newCellContent);
            _newCellContent.ResetColor();
        }
        else
        {
            oldCell.ClearContent();
        }

        SetContent(this, newContent);
    }

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
        _currentNumber = number;
        _baseCellContent.SetNumber(number);
    }

    public void ResetCell()
    {
        _baseCellContent.transform.SetParent(transform);
        _baseCellContent.transform.localPosition = Vector3.zero;
        _baseCellContent.Cell = this;
        _baseCellContent.ResetColor();

        if (!_isMainField)
            _newCellContent = _baseCellContent;
        else
            _newCellContent = null;
    }

    public void SetContent(Cell newCell, CellContent newContent)
    {
        newContent.transform.SetParent(transform);
        newContent.transform.localPosition = Vector3.zero;
        newCell.SetContent(newContent);
        newContent.Cell = newCell;
    }
    private void SetContent(CellContent newContent)
    {
        _newCellContent = newContent;
    }
    public void HideContent()
    {
        _baseCellContent.Hide();
    }

    public void ShowContent()
    {
        _baseCellContent.Show();
    }

    private void BlockContent()
    {
        _baseCellContent.Block();
    }

    public void UnblockContent()
    {
        _baseCellContent.Unblock();
    }

    public void Chek()
    {
        if(_newCellContent != null)
        {
            if(Number == _newCellContent.Number)
            {
                _newCellContent.SetColor(_colorMatch);
            }
            else
            {
                _newCellContent.SetColor(_colorMismatch);
            }
        }
    }

    private void ClearContent()
    {
        _newCellContent = null;
    }

    public void DestroyCell()
    {
        Destroy(gameObject);
    }
}