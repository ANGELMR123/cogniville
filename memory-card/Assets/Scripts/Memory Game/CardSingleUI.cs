using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSingleUI : MonoBehaviour
{
    private CardGroup cardGroup;

    [SerializeField] private Button cardBackButton;

    [SerializeField] private Image cardBackBackground;
    [SerializeField] private Image cardFrontBackground;
    [SerializeField] private Image cardFrontImage;

    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;

    private bool objectMatch;

    [Header("DoTween Animation")]
    [SerializeField] private Vector3 selectRotation = new Vector3();
    [SerializeField] private Vector3 deselectRotation = new Vector3();
    [SerializeField] private float duration = 0.25f;
    private Tweener[] tweener = new Tweener[3];

    private void Awake()
    {
        if (cardGroup == null)
        {
            cardGroup = transform.parent.GetComponent<CardGroup>();
        }

        if (cardGroup != null)
        {
            cardGroup.Subscribe(this);
        }
    }

    private void Start()
    {
        cardBackButton.onClick.AddListener(OnClick);

        // Initialize the card to face down
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        // Set the initial color of the card fronts to white
        cardFrontBackground.color = Color.white;
        cardFrontImage.color = Color.white;

        StartCoroutine(WaitingToHide());

        MemoryGameManagerUI.Instance.Subscribe(this);
    }

    private void OnClick()
    {
        cardGroup.OnCardSelected(this);
    }

    public void Select()
    {
        tweener[0] = transform.DORotate(selectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckSelectHalfDuration);
    }

    public void Deselect()
    {
        tweener[1] = transform.DORotate(deselectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckDeselectHalfDuration);
    }

    private IEnumerator WaitingToHide()
    {
        yield return new WaitForSeconds(3f);

        tweener[2] = transform.DORotate(deselectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckWaitingToHide);
    }

    private void CheckWaitingToHide()
    {
        float elapsed = tweener[2].Elapsed();
        float halfDuration = tweener[2].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            cardFront.SetActive(false);
            cardBack.SetActive(true);
        }
    }

    private void CheckSelectHalfDuration()
    {
        float elapsed = tweener[0].Elapsed();
        float halfDuration = tweener[0].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            Debug.Log("Flipping card to show front.");

            cardBack.SetActive(false);
            cardFront.SetActive(true);
            cardFrontBackground.gameObject.SetActive(true);
            cardFrontImage.gameObject.SetActive(true);

            // Reset color to white to avoid tinting
            cardFrontBackground.color = Color.white;
            cardFrontImage.color = Color.white;

            Debug.Log("cardFrontBackground active state: " + cardFrontBackground.gameObject.activeSelf);
            Debug.Log("cardFrontBackground position: " + cardFrontBackground.rectTransform.position);
            Debug.Log("cardFrontBackground size: " + cardFrontBackground.rectTransform.sizeDelta);
            Debug.Log("cardFrontImage sprite: " + (cardFrontImage.sprite != null ? cardFrontImage.sprite.name : "null"));
        }
    }

    private void CheckDeselectHalfDuration()
    {
        float elapsed = tweener[1].Elapsed();
        float halfDuration = tweener[1].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            cardFront.SetActive(false);
            cardBack.SetActive(true);
        }
    }

    public Image GetCardBackBackground() => cardBackBackground;
    public Image GetCardFrontBackground() => cardFrontBackground;

    public void SetObjectMatch()
    {
        objectMatch = true;
    }

    public void SetCardImage(Sprite sprite)
    {
        cardFrontImage.sprite = sprite;
        Debug.Log("SetCardImage called. Sprite: " + (sprite != null ? sprite.name : "null"));
    }

    public bool GetObjectMatch() => objectMatch;

    public void DisableCardBackButton() => cardBackButton.interactable = false;
}
