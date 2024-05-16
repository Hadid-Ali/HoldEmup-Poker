using System;
using DG.Tweening;
using UnityEngine;

public class SlideAnimation : MonoBehaviour
{
    public enum SlideDirection { Top, Left, Bottom, Right }

    [Header("The Anchor should be middle for this to work correctly")]
    [Space]
    public SlideDirection direction;
    public float animationTime;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Ease ease;
    [Space] 
    [SerializeField] private bool animateOnEnable;

    private RectTransform rect;
    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        
        if(!animateOnEnable)
            Setup();
    }

    private void OnEnable()
    {
        if (animateOnEnable)
        {
            Setup();
            PlaySlide();
        }
    }

    public void Slide()
    {
        PlaySlide();
    }

    public void Setup()
    {
        var position = transform.localPosition;
        
        initialPosition = new Vector3(position.x,position.y,position.z);
        offsetPosition = new Vector3(position.x,position.y,position.z);;
        
        switch (direction)
        {
            case SlideDirection.Top:
                offsetPosition.y = Screen.height/2 + rect.rect.height;
                transform.localPosition = offsetPosition;
                break;
            case SlideDirection.Left:
                offsetPosition.x = -Screen.width * 2 - rect.rect.width;
                transform.localPosition = offsetPosition;

                break;
            case SlideDirection.Bottom:
                offsetPosition.y = -Screen.height/2 - rect.rect.height;
                transform.localPosition = offsetPosition;

                break;
            case SlideDirection.Right:
                offsetPosition.x = Screen.width * 2 + rect.rect.width;
                transform.localPosition = offsetPosition;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void PlaySlide()
    {
        switch (direction)
        {
            case SlideDirection.Top:
            case SlideDirection.Bottom:
                transform.DOLocalMoveY(initialPosition.y, animationTime).SetEase(ease);
                break;
            case SlideDirection.Left:
            case SlideDirection.Right:
                transform.DOLocalMoveX(initialPosition.x, animationTime).SetEase(ease);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}

    


    


