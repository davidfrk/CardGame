using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class RandomDeck : MonoBehaviour {

    static public RandomDeck instance;
    public Transform Graveyard;
    public List<CardCount> Cards;
    int cardsCount = 0;
    Card graveyard;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (CardCount cardCount in Cards)
        {
            cardsCount += cardCount.count;
        }
        GameManager.instance.ClearEvent.AddListener(Clear);
    }

    private Card NextCard()
    {
        int rand = Random.Range(1, cardsCount + 1);
        int tempCount = 0;
        foreach (CardCount cardCount in Cards)
        {
            tempCount += cardCount.count;
            if (tempCount >= rand)
                return cardCount.card;
        }
        return null;
    }

    public Card DrawCard()
    {
        GameObject go = Instantiate(NextCard().gameObject, transform.position, transform.rotation);
        return go.GetComponent<Card>();
    }

    public void AddToGraveyard(Card card)
    {
        if (graveyard != null)
        {
            Destroy(graveyard.gameObject, GameManager.instance.DrawCardDuration);
        }
        graveyard = card;
    }

    public void Clear()
    {
        if (graveyard != null)
            Destroy(graveyard.gameObject);
    }
}

[System.Serializable]
public class CardCount
{
    public Card card;
    public int count;
}
