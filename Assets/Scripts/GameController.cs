using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.SceneTemplate;
using UnityEngine.SocialPlatforms.Impl;
using System;
using System.Data;

public class GameController : MonoBehaviour
{
    public GameObject MainObject;

    public int rowCount;
    public int columnCount;

    public SampleCard sampleCard;
    public Transform grideParent;

    public List<Sprite> Options = new List<Sprite>();

    public DynamicGrid dynamicGrid;

    private List<SampleCard> cardsList = new List<SampleCard>();

    private List<Sprite> result = new List<Sprite>();

    private bool isCardSelected = false;
    private SampleCard SelectedCard;

    public TMP_Text Turns_Text;
    public int TurnCounter;
    public TMP_Text Score_Text;
    public int Score;

    public static Action CardFlipCallBack;

    public int ComboCounter;
    public int GameEndCounter;

    public TMP_Text TotalScore;

    public GameObject Endgame;
    public List<string> ImageNameList = new List<string>();

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

        TurnCounter = 0;
        Score = 0;
        ComboCounter = -1;
        GameEndCounter= 0;

        TotalScore.text = ""+ PlayerPrefs.GetInt("TotalScore");
        Endgame.SetActive(false);

    }


    //This method sets the card based number of rows and colums 
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

            cardz.index = i;

            cardz.card_Button.interactable = false;

            cardz.front_Image.sprite = result[i];
            ImageNameList.Add(result[i].name);

            cardz.card_Button.onClick.RemoveAllListeners();
            cardz.card_Button.onClick.AddListener(()=>
            {
                VerifyCardMatch(cardz); 
            });

            cardsList.Add(cardz);
        }

        //Setting the grid parameter based on rows and columns
        dynamicGrid.SetGridPattern(rowCount, columnCount, cardsList);

        StartCoroutine(InitialFlipWithDelay());
    }

    public void OnResumePreviouseGame(int rows, int colums, int Turns, int score, int endCounter, List<string> ImageNames)
    {
        cardsList.Clear();
        result.Clear();
        MainObject.SetActive(true);

        ComboCounter = -1;
        GameEndCounter = endCounter;

        TotalScore.text = "" + PlayerPrefs.GetInt("TotalScore");
        Endgame.SetActive(false);

        Score_Text.text = "" + score;
        Turns_Text.text = "" + Turns;

        for(int i=0; i < ImageNames.Count; i++)
        {
            if (ImageNames[i] != "null")
            {
                for (int j = 0; j < Options.Count; j++)
                {
                    if (ImageNames[i] == Options[j].name)
                    {
                        result.Add(Options[j]);
                    }
                }
            }
            else
            {
                result.Add(null);
            }
        }

        for (int i = 0; i < result.Count; i++)
        {
            SampleCard cardz = Instantiate(sampleCard);
            if (result[i] != null)
            {
               
                cardz.card_Button.interactable = false;

                cardz.front_Image.sprite = result[i];
                Debug.Log("cardz.front_Image.sprite     "+ cardz.front_Image.sprite.name); 
                cardz.card_Button.onClick.RemoveAllListeners();
                cardz.card_Button.onClick.AddListener(() =>
                {
                    VerifyCardMatch(cardz);
                  
                });
                cardsList.Add(cardz);
               
            }
            else
            {
                cardz.front_Image.sprite = null;
                cardsList.Add(cardz) ;
            }
           
        }

        dynamicGrid.SetGridPattern(rows, colums, cardsList);
        StartCoroutine(InitialFlipWithDelay());
    }


    //Shuffle the the final result after randomization
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

    //Verify the the match status
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
                ImageNameList[SelectedCard.index] = "null";
                ImageNameList[card.index] = "null";
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
        ++TurnCounter;
        Turns_Text.text = ""+TurnCounter;
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
            ImageNameList.Clear();
            int TotalScore = PlayerPrefs.GetInt("TotalScore") + Score;
            PlayerPrefs.SetInt("TotalScore", TotalScore);
            Endgame.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            MainController.Instance.mainMenuController.Init();
            Close();
        }
    }



    void SaveGame()
    {
        if (ImageNameList.Count > 0)
        {
            DetailsToSave data = new DetailsToSave
            {
                rowCount = rowCount,
                columnCount = columnCount,
                TurnCounter = TurnCounter,
                Score = Score,
                GameEndCounter = GameEndCounter,
                ImageNameList = new List<string>(ImageNameList)
            };

            string jsonData = JsonUtility.ToJson(data);

            PlayerPrefs.SetString("SavedData", jsonData);
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
