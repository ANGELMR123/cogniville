using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardGridUI : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public string cardName;
        public Sprite cardImage;
    }

    [SerializeField] private List<Card> cardList = new List<Card>();
    [SerializeField] private List<Card> cardListToSort = new List<Card>();
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform cardPrefab;
    private int currentLevel;

    private void Start()
    {
        currentLevel = LevelManager.LoadLevel(); // Cargar el nivel desde el archivo
        cardPrefab.gameObject.SetActive(false);
        FillGrid();
    }

    private void OnEnable()
    {
        // FillGrid(); // Si quieres llenar la cuadrícula cada vez que se habilita
    }

    // Mapeo del nivel al número de cartas
    private Dictionary<int, int> levelToCardsCount = new Dictionary<int, int>()
    {
        { 1, 6 }, { 2, 6 }, { 3, 6 }, { 4, 9 }, { 5, 9 },
        { 6, 9 }, { 7, 12 }, { 8, 12 }, { 9, 12 }
    };

    public void FillGrid()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        cardListToSort.Clear(); // Limpia la lista antes de llenarla de nuevo

        // Obtener el número de cartas para el nivel actual
        if (!levelToCardsCount.TryGetValue(currentLevel, out int cardsToShow))
        {
            Debug.LogError("Nivel actual no válido: " + currentLevel);
            return;
        }

        if (cardList.Count < cardsToShow)
        {
            Debug.LogError("No hay suficientes cartas en la lista de cartas para mostrar.");
            return;
        }

        for (int i = 0; i < cardsToShow / 2; i++)  // cardsToShow / 2 porque cada carta se añade dos veces
        {
            cardListToSort.Add(cardList[i]);
            cardListToSort.Add(cardList[i]);
        }

        System.Random rnd = new System.Random();
        IOrderedEnumerable<Card> randomized = cardListToSort.OrderBy(i => rnd.Next());

        foreach (Card card in randomized)
        {
            Transform cardTransform = Instantiate(cardPrefab, cardContainer);
            cardTransform.gameObject.SetActive(true);
            cardTransform.name = card.cardName;
            CardSingleUI cardSingleUI = cardTransform.GetComponent<CardSingleUI>();
            cardSingleUI.SetCardImage(card.cardImage);

            // Subscribe card to the MemoryGameManagerUI and CardGroup
            MemoryGameManagerUI.Instance.Subscribe(cardSingleUI);
            cardSingleUI.transform.SetParent(cardContainer);
        }
    }

    public void CompleteLevel()
    {
        currentLevel++;
        LevelManager.SaveLevel(currentLevel); // Guardar el nivel después de completarlo

        string nextScene = GetNextSceneName(currentLevel);

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            FillGrid();
        }
    }

    private string GetNextSceneName(int level)
    {
        if (level > 9) return "Completado";
        if (level > 6) return "BloqueDificil";
        if (level > 3) return "BloqueMedio";
        return "LevelMenu";
    }

}
