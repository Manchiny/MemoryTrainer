using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellContent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Color _defaulColor;

    private Canvas _mainCanvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    public int Number { get; private set; }
    public Cell Cell { get; set; }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        Cell = GetComponentInParent<Cell>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotTransform = _rectTransform.parent;
        slotTransform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
        ResetColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        Block();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
    }

    public void Block()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Unblock()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void SetColor(Color color)
    {
        _background.color = color;
    }

    public void ResetColor()
    {
        _background.color = _defaulColor;
    }

    public void SetNumber(int number)
    {
        Number = number;
    }
}
