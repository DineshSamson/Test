using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.SceneTemplate;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class GameController : MonoBehaviour
{
    public GameObject MainObject;

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

    public TMP_Text Turns_Text;
    private int TurnCounter = 0;
    public TMP_Text Score_Text;
    public int Score;

    public static Action CardFlipCallBack;

    public int ComboCounter;
    public int GameEndCounter;

    public TMP_Text TotalScore;

    public GameObject Endgame;

    private void OnEnable()
    {
        CardFlipCallBack += StatusAudioPlay;
    }

    private void OnDisable()
    {
        CardFlipCallBack -= StatusAudioPlay;
    }

    public void Init()
    {
        MainObject.SetActive(true);
        SetupCards();
        AddScore(0);
        AddTuens();

        ComboCounter = -1;
        GameEndCounter= 0;

        TotalScore.text = ""+ PlayerPrefs.GetInt("TotalScore");
        Endgame.SetActive(false);

    }


    public void SetupCards()
    {
        cardsList.Clear();

        List<Sprite> tempOptionList = Options.ToList();

        List<Sprite> FinalOptionList = new List<Sprite>();

        int NumberOfCards = rowCount * columnCount;

        for(int i = 0; i < NumberOfCards/2; i++) 
        {
          
            int RandomOption = UnityEngine.Random.Range(0, tempOptionList.Count);

            FinalOptionList.Add(tempOptionList[RandomOption]);
            FinalOptionList.Add(tempOptionList[RandomOption]);

            tempOptionList.RemoveAt(RandomOption);
        }

        SuffleList(FinalOptionList);


        result = FinalOptionList.GetRange(0, NumberOfCards);

        for (int i= 0; i < result.Count; i++)
        {
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

   
    void SuffleList<T>(List<T> list)
    {
        for(int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0 , i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    IEnumerator InitialFlipWithDelay()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < cardsList.Count; i++)
        {
            cardsList[i].FlipCardToBack();
            cardsList[i].card_Button.interactable = true;
        }
    }

    void VerifyCardMatch(SampleCard card)
    {
        SoundManager.instance.PlayButtonClickSound();
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
              
                SoundManager.instance.PlayMatchSound();
                StartCoroutine( SelectedCard.HideCard());
                StartCoroutine(card.HideCard());            
                ComboCounter++;
                AddScore(1);
                StartCoroutine(CheckGameEnd());
            }
            else
            { 
                StartCoroutine(SelectedCard.KeepCard());
                StartCoroutine(card.KeepCard());
                SoundManager.instance.PlayMissMatchSound();
                ComboCounter = -1;
            }
            isCardSelected = false;
            SelectedCard = null;
            AddTuens();
        }
    }

  

    void AddTuens()
    {       
        Turns_Text.text = ""+TurnCounter++;
    }

    void AddScore(int ScoreToAdd)
    {
        if(ComboCounter == 0)
            Score += ScoreToAdd;
        else
            Score += ComboCounter * 5;

        Score_Text.text = ""+ Score;
    }

    public void StatusAudioPlay()
    {
        Debug.Log("Fliped");
    }

    public void Close()
    {
        MainObject.SetActive(false);
    }

    public IEnumerator CheckGameEnd()
    {
        yield return new WaitForSeconds(1);
        ++GameEndCounter;
        if(GameEndCounter == ((rowCount * columnCount) / 2))
        {
            int TotalScore = PlayerPrefs.GetInt("TotalScore") + Score;
            PlayerPrefs.SetInt("TotalScore", TotalScore);
            Endgame.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            MainController.Instance.mainMenuController.Init();
            Close();
        }
    }
}
