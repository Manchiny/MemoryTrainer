using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayField
{
    private const int MIN_NUMBER = 1;
    private const int MAX_NUMBER = 99;

    private GridLayoutGroup _mainField;
    private GridLayoutGroup _heapField;
    private Cell _cellPrefab;

    private HashSet<int> _randomNumbers;
    private List<Cell> _itemsOnMain;
    private List<Cell> _itemsInHeap;

    public int FieldSize { get; private set; } = 5;
    public int ShowTime { get; private set; } = 2;
    public PlayField(GridLayoutGroup mainField, GridLayoutGroup heapField, Cell cellPrefab)
    {
        _mainField = mainField;
        _heapField = heapField;
        _cellPrefab = cellPrefab;
    }
    public void StartNewGame()
    {
        ResetCells();
        ActivateGrids();
        SetupPlayfield();
        ShowMainNumbers();
        HideHeapsNumbers();
        GenerateRandomNumbers();
        RandomizeNumbersInPull();

        _mainField.StopAllCoroutines();
        _mainField.StartCoroutine(EndGeneratePlayField());
    }

    private void GenerateRandomNumbers()
    {
        _randomNumbers = new HashSet<int>();

        while (_randomNumbers.Count < FieldSize * FieldSize)
        {
            int random = Random.Range(MIN_NUMBER, MAX_NUMBER);
            _randomNumbers.Add(random);
        }

        int iterator = 0;
        foreach (var item in _randomNumbers)
        {
            var cell = _itemsOnMain[iterator];
            cell.SetNumber(item);
            iterator++;
        }
    }

    private void RandomizeNumbersInPull()
    {
        List<int> numbers = new List<int>();

        foreach (var num in _randomNumbers)
        {
            numbers.Add(num);
        }

        List<int> randomized = new List<int>();

        for (int i = numbers.Count - 1; i >= 0; i--)
        {
            int random = Random.Range(0, i);
            int num = numbers[random];
            randomized.Add(num);
            numbers.Remove(num);
        }

        for (int i = 0; i < randomized.Count; i++)
        {
            var item = _itemsInHeap[i];
            item.SetNumber(randomized[i]);
        }
    }
    private void SetupPlayfield()
    {
        _mainField.constraintCount = FieldSize;
        _heapField.constraintCount = FieldSize;
        _itemsOnMain ??= new List<Cell>();
        _itemsInHeap ??= new List<Cell>();

        int needCells = FieldSize * FieldSize - _itemsOnMain.Count;

        if (needCells > 0)
        {
            AddCells();
        }
        else if (needCells < 0)
        {
            RemoveCells();
        }

        void AddCells()
        {
            for (int i = 0; i < needCells; i++)
            {
                var itemForMain = Object.Instantiate(_cellPrefab, _mainField.transform);
                itemForMain.Init(true);
                _itemsOnMain.Add(itemForMain);

                var itemForHeap = Object.Instantiate(_cellPrefab, _heapField.transform);
                itemForHeap.Init(false);
                _itemsInHeap.Add(itemForHeap);
            }
        }

        void RemoveCells()
        {
            int index = _itemsOnMain.Count - 1;
            for (int i = 0; i < Mathf.Abs(needCells); i++)
            {
                var cellMain = _itemsOnMain[0];
                cellMain.DestroyCell();
                _itemsOnMain.Remove(cellMain);

                var cellHeap = _itemsInHeap[0];
                cellHeap.DestroyCell();
                _itemsInHeap.Remove(cellHeap);

                index--;
            }
        }

    }

    private void ResetCells()
    {
        if (_itemsOnMain != null && _itemsOnMain.Count > 0)
        {
            for (int i = _itemsOnMain.Count - 1; i >= 0; i--)
            {
                var item = _itemsOnMain[i];
                item.ResetCell();
            }
        }

        if (_itemsInHeap != null && _itemsInHeap.Count > 0)
        {
            for (int i = _itemsInHeap.Count - 1; i >= 0; i--)
            {
                var item = _itemsInHeap[i];
                item.ResetCell();
            }
        }
    }

    private IEnumerator EndGeneratePlayField()
    {
        yield return new WaitForEndOfFrame();
        DeactivateGrids();
        _mainField.StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(ShowTime);
        HideMainNumbers();
        ShowHeapsNumbers();
    }
    public void OnPlayFieldSliderChanged(int value)
    {
        FieldSize = value;
    }

    public void OnTimerValueSet(int value)
    {
        ShowTime = value;
    }

    private void HideMainNumbers()
    {
        foreach (var cell in _itemsOnMain)
        {
            cell.HideContent();
        }
    }

    private void ShowMainNumbers()
    {
        foreach (var cell in _itemsOnMain)
        {
            cell.ShowContent();
        }
    }

    private void HideHeapsNumbers()
    {
        foreach (var cell in _itemsInHeap)
        {
            cell.HideContent();
        }
    }

    private void ShowHeapsNumbers()
    {
        foreach (var cell in _itemsInHeap)
        {
            cell.ShowContent();
            cell.UnblockContent();
        }
    }
    public void Check()
    {
        foreach (var item in _itemsOnMain)
        {
            item.Chek();
        }
    }
    private void DeactivateGrids()
    {
        _mainField.enabled = false;
        _heapField.enabled = false;
    }

    private void ActivateGrids()
    {
        _mainField.enabled = true;
        _heapField.enabled = true;
    }
}
