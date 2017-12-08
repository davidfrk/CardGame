using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeck : MonoBehaviour {

    static public RandomDeck instance;
    public List<CardCount> Cards;
    int cardsCount = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (CardCount cardCount in Cards)
        {
            cardCount.count += cardCount.count;
        }
    }

    public Card NextCard()
    {
        int rand = Random.Range(1, cardsCount);
        int tempCount = 0;
        foreach (CardCount cardCount in Cards)
        {
            tempCount += cardCount.count;
            if (tempCount >= rand)
                return cardCount.card;
        }
        return null;
    }
}

[System.Serializable]
public class CardCount
{
    public Card card;
    public int count;
}
