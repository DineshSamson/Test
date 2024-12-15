using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject MainObject;

    public Slider VolumeSlider;

    public List<Image> LayoutButton =new List<Image>();
    public Color ButtonSelected;
    public Color ButtonNotSelected;

    public Animation playAnimation;

    public TMP_Text TotalScore;
    private void Start()
    {
        VolumeSlider.value = 1;
        VolumeSlider.onValueChanged.AddListener(delegate { SoundManager.instance.AdjustVolume(VolumeSlider.value); });
    }

    public void Init()
    {
        MainObject.SetActive(true);

        playAnimation.Play();

        if (!PlayerPrefs.HasKey("Layout"))
            SelectedLayout(0);
        else
        {
            SelectedLayout(PlayerPrefs.GetInt("Layout"));
        }

        if(!PlayerPrefs.HasKey("Volume"))
        {
            SoundManager.instance.AdjustVolume(1);
        }
        else
        {
            SoundManager.instance.AdjustVolume(PlayerPrefs.GetFloat("Volume"));
        }

        if(!PlayerPrefs.HasKey("TotalScore"))
        {
            PlayerPrefs.SetInt("TotalScore", 0);
            TotalScore.text = ""+ PlayerPrefs.GetInt("TotalScore");
        }
        else
        {
            TotalScore.text = "" + PlayerPrefs.GetInt("TotalScore");
        }
    }

    public void PlayGame()
    {
        MainController.Instance.gameController.Init();
        Close();
    }

    public void SelectedLayout(int Index)
    {
        PlayerPrefs.SetInt("Layout", Index);
        if (Index == 0)
        {
            MainController.Instance.gameController.rowCount = 2;
            MainController.Instance.gameController.columnCount = 2;
        }
        else if (Index == 1)
        {
            MainController.Instance.gameController.rowCount = 2;
            MainController.Instance.gameController.columnCount = 3;
        }
        else if (Index == 2)
        {
            MainController.Instance.gameController.rowCount = 5;
            MainController.Instance.gameController.columnCount = 6;
        }

        for(int i=0; i < LayoutButton.Count; i++)
        {
            LayoutButton[i].color = ButtonNotSelected;
        }
        LayoutButton[Index].color = ButtonSelected;
    }

    public void Close()
    {
        MainObject.SetActive(false);
    }
}