using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCard : MonoBehaviour
{
    public int index;
    public Button card_Button;
    public Image front_Image;
    public GameObject back_Image;
    public Animation _animation;

   
    //Flips the card to back
    public void FlipCardToBack()
    {
        if (_animation.enabled)
        { 
        _animation.Play("FlipBack");
            StartCoroutine(HandleImageOnFlipBack());
        }
    }

    //Flips the card to front
    public void FlipCardToFront()
    {
        if (_animation.enabled)
        {
            _animation.Play("FlipFront");
            StartCoroutine(HandleImageOnFlipFront());
        }
    }

    IEnumerator HandleImageOnFlipBack()
    {
        //Handles Front image and back image 
        yield return new WaitForSeconds(0.5f);
        front_Image.gameObject.SetActive(false);
        back_Image.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        card_Button.transform.localEulerAngles = Vector2.zero;
        card_Button.interactable = true;
    }

    IEnumerator HandleImageOnFlipFront()
    {
        //Handles Front image and back image 
        yield return new WaitForSeconds(0.5f);
        front_Image.gameObject.SetActive(true);
        back_Image.SetActive(false);
        card_Button.interactable = false;

        GameController.CardFlipCallBack?.Invoke();
    }

    public IEnumerator KeepCard()
    {
        if (_animation.enabled)
        {
            yield return new WaitForSeconds(2);
            FlipCardToBack();
        }
    }

    public IEnumerator HideCard()
    {
        if (_animation.enabled)
        {
            card_Button.GetComponent<Image>().enabled = false;
            yield return new WaitForSeconds(1.5f);
            front_Image.gameObject.SetActive(false);
            back_Image.SetActive(false);
            card_Button.interactable = false;
        }

    }
}
