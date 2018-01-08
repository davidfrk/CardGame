using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Deck : MonoBehaviour {

    static public Deck instance;
    public Transform Graveyard;
    private List<Card> gameCards;
    public List<Card> Cards;
    public bool infiniteDeck = true;
    Card graveyardCard;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameCards = new List<Card>(Cards);
        GameManager.instance.ClearEvent.AddListener(Clear);
    }

    public void Add(Card card)
    {
        Cards.Add(card);
    }

    public void Remove(Card card)
    {
        Cards.Remove(card);
    }

    public void Save(int deck)
    {

    }

    public void Load(int deck)
    {

    }

    public Card DrawCard()
    {
        int rand = Random.Range(0, gameCards.Count);
        Card card = gameCards[rand];
        if (!infiniteDeck)
        {
            gameCards.RemoveAt(rand);
        }
        GameObject go = Instantiate(card.gameObject, transform.position, transform.rotation);
        return go.GetComponent<Card>();
    }

    public void AddToGraveyard(Card card)
    {
        if (graveyardCard != null)
        {
            Destroy(graveyardCard.gameObject, GameManager.instance.DrawCardDuration);
        }
        graveyardCard = card;
    }

    public void Clear()
    {
        if (graveyardCard != null)
            Destroy(graveyardCard.gameObject);
        gameCards.Clear();
    }
}
