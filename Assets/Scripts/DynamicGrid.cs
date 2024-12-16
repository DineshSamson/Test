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

        float parentWidth = grideRect.rect.width - _GridLayoutGroup.padding.left - _GridLayoutGroup.padding.right;
        float parentHeight = grideRect.rect.height - _GridLayoutGroup.padding.top - _GridLayoutGroup.padding.bottom ;


        float cellWidth = parentWidth / columns - _GridLayoutGroup.spacing.x;
        float cellHeight = parentHeight / rows - _GridLayoutGroup.spacing.y;

        float SquareCellSize = Mathf.Min(cellWidth, cellHeight);

        _GridLayoutGroup.cellSize = new Vector2(SquareCellSize, SquareCellSize);

        foreach (Transform child in _gridParent)
        {
            Debug.Log("");
            Destroy(child.gameObject);
        }

        int totalCards = rows * columns;

        for (int i = 0; i < CardsList.Count; i++)
        {

            CardsList[i].transform.SetParent(_gridParent, false);

            if (CardsList[i].front_Image.sprite == null)

            {
                CardsList[i].card_Button.GetComponent<Image>().enabled = false;
                CardsList[i].front_Image.gameObject.SetActive(false);
                CardsList[i].back_Image.SetActive(false);
                CardsList[i].card_Button.interactable = false;
                CardsList[i]._animation.enabled = false;
            }
        }

        Vector2 CurrentSize = grideRect.sizeDelta;

        float NewHeight = (SquareCellSize + _GridLayoutGroup.spacing.y) * rows;
        float newWidth = (SquareCellSize + _GridLayoutGroup.spacing.x) * columns;

        grideRect.sizeDelta = new Vector2(newWidth, NewHeight);

        grideRect.anchoredPosition = new Vector2(0, 0);
        grideRect.pivot = new Vector2(0.5f, 0.5f);
    }
}
