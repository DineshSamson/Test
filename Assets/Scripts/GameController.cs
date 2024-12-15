using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.SceneTemplate;

public class GameController : MonoBehaviour
{
    public int rowCount = 4;
    public int columnCount = 4;

    public SampleCard sampleCard;
    public Transform grideParent;

    public List<Sprite> Options = new List<Sprite>();

    public DynamicGrid dynamicGrid;

    private List<SampleCard> cardsList = new List<SampleCard>();

    public List<Sprite> result = new List<Sprite>();

    private bool isCardSelected = false;
    private SampleCard SelectedCard;

    // Start is called before the first frame update
    void Start()
    {
        SetupCards();
    }


    public void SetupCards()
    {
        cardsList.Clear();

        List<Sprite> tempOptionList = Options.ToList();

        List<Sprite> FinalOptionList = new List<Sprite>();

        int NumberOfCards = rowCount * columnCount;

        for(int i = 0; i < NumberOfCards/2; i++) 
        {
          
            int RandomOption = Random.Range(0, tempOptionList.Count);

            FinalOptionList.Add(tempOptionList[RandomOption]);
            FinalOptionList.Add(tempOptionList[RandomOption]);

            tempOptionList.RemoveAt(RandomOption);
        }

        SuffleList(FinalOptionList);


        result = FinalOptionList.GetRange(0, NumberOfCards);

        for (int i= 0; i < result.Count; i++)
        {
            Debug.Log("i  " + i);

            SampleCard cardz = Instantiate(sampleCard);

            cardz.card_Button.interactable = false;

            cardz.front_Image.sprite = result[i];

            cardz.card_Button.onClick.RemoveAllListeners();
            cardz.card_Button.onClick.AddListener(()=>
            {
                VerifyCardMatch(cardz); 
            });

            cardsList.Add(cardz);
        }

        dynamicGrid.SetGridPattern(rowCount, columnCount, cardsList);

        StartCoroutine(InitialFlipWithDelay());
    }

    IEnumerator InitialFlipWithDelay()
    {
        yield return new WaitForSeconds(3);
        for(int i = 0; i < cardsList.Count; i++)
        {
            cardsList[i].FlipCardToBack();
            cardsList[i].card_Button.interactable = true;
        }
    }

    void SuffleList<T>(List<T> list)
    {
        for(int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0 , i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    void VerifyCardMatch(SampleCard card)
    {
        card.FlipCardToFront();
        if (!isCardSelected)
        {
            isCardSelected = true;
            SelectedCard = card;
        }
        else
        {
            if(SelectedCard.front_Image.sprite == card.front_Image.sprite)
            {
                SelectedCard.card_Button.interactable = false;
                card.card_Button.interactable = false;
            }
            else
            {
                StartCoroutine(FlipBackHandling(card, SelectedCard));
            }
            isCardSelected = false;
            SelectedCard = null;
        }
    }

    IEnumerator FlipBackHandling(SampleCard Card_1, SampleCard Card_2)
    {
        yield return new WaitForSeconds(1);
        Card_1.FlipCardToBack();
        Card_2.FlipCardToBack();
    }

}
