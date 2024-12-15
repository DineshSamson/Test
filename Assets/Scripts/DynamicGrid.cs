using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DynamicGrid : MonoBehaviour
{
    public GridLayoutGroup _GridLayoutGroup;
    public GameObject _cardPrefab;
    public Transform _gridParent;

    public void SetGridPattern(int rows, int columns, List<SampleCard> CardsList)
    {
        _GridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _GridLayoutGroup.constraintCount = columns;

        RectTransform grideRect = GetComponent<RectTransform>();

        //float parentWidth = grideRect.rect.width;
        //float parentHeight = grideRect.rect.height;

        float parentWidth = grideRect.rect.width - _GridLayoutGroup.padding.left - _GridLayoutGroup.padding.right;
        float parentHeight = grideRect.rect.height - _GridLayoutGroup.padding.top - _GridLayoutGroup.padding.bottom ;

        //float cellWidth = parentWidth / columns - _GridLayoutGroup.spacing.x - _GridLayoutGroup.padding.left / columns;
        //float cellHeight = parentHeight / rows - _GridLayoutGroup.spacing.y - _GridLayoutGroup.padding.top / rows;

        float cellWidth = parentWidth / columns - _GridLayoutGroup.spacing.x;
        float cellHeight = parentHeight / rows - _GridLayoutGroup.spacing.y;

        float SquareCellSize = Mathf.Min(cellWidth, cellHeight);

        //_GridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        _GridLayoutGroup.cellSize = new Vector2(SquareCellSize, SquareCellSize);




        foreach (Transform child in _gridParent)
        {
            Debug.Log("");
            Destroy(child.gameObject);
        }

        int totalCards = rows * columns;

        for (int i = 0; i < CardsList.Count; i++)
        {
            //Instantiate(_cardPrefab, _gridParent);
            CardsList[i].transform.SetParent(_gridParent, false);
        }

        Vector2 CurrentSize = grideRect.sizeDelta;

        float NewHeight = (SquareCellSize + _GridLayoutGroup.spacing.y) * rows;
        float newWidth = (SquareCellSize + _GridLayoutGroup.spacing.x) * columns;

        //grideRect.sizeDelta = new Vector2(CurrentSize.x, NewHeight);

        grideRect.sizeDelta = new Vector2(newWidth, NewHeight);

        grideRect.anchoredPosition = new Vector2(0, 0);
        grideRect.pivot = new Vector2(0.5f, 0.5f);

    }

   


    // Start is called before the first frame update
    void Start()
    {
        //SetGridPattern(4 ,4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
