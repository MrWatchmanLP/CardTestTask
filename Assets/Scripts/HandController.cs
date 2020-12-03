using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public int CardsInHand = 0;
    public float Angle = 90;
    public List<RectTransform> cardsTransforms;
    public GameObject cardPrefab;

    void Awake()
    {
        CardsInHand = transform.childCount;
        GetAllCardsInHand();
        RearrangeCards();
    }

    void GetAllCardsInHand()
    {
        cardsTransforms.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rt = transform.GetChild(i).GetComponent<RectTransform>();
            rt.name = "Card Pivot " + cardsTransforms.Count.ToString();
            cardsTransforms.Add(rt);
        }
    }

    public void ChangeValueOfAllCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int first_random = Random.Range(0, 2);
            transform.GetChild(i).GetComponent<CardDisplay>().ChangeValue(first_random);
        }
    }

    public void DrawCard()
    {
        GameObject go = Instantiate(cardPrefab, transform);
        cardsTransforms.Add(go.GetComponent<RectTransform>());
        go.name = "Card Pivot " + cardsTransforms.Count.ToString();
        RearrangeCards();
    }

    public void RearrangeCards()
    {
        GetAllCardsInHand();
        CardsInHand = transform.childCount;
        if ( CardsInHand > 2)
        {
            float tempHelp = 0;
            if (CardsInHand > 2 && CardsInHand < 6)
                tempHelp = CardsInHand - 1;
            float step = Angle / ((CardsInHand - 1) + tempHelp);
            for (int i = 0; i < CardsInHand; i++)
            {
                float tempAngleHelp = 0;
                if (tempHelp > 0)
                    tempAngleHelp = 2;
                StartCoroutine(transform.GetChild(i).GetComponent<CardDisplay>().MoveToPos(new Vector2(0f, -650f), new Vector3(0, 0, (Angle / (2 + tempAngleHelp)) - step * i)));
            }
        }
        else if (CardsInHand == 2)
        {
            StartCoroutine(transform.GetChild(0).GetComponent<CardDisplay>().MoveToPos(new Vector2(-100f, -725f), new Vector3(0f, 0f, 0f)));
            StartCoroutine(transform.GetChild(1).GetComponent<CardDisplay>().MoveToPos(new Vector2(100f, -725f), new Vector3(0f, 0f, 0f)));
        }
        else if (CardsInHand == 1)
        {
            StartCoroutine(transform.GetChild(0).GetComponent<CardDisplay>().MoveToPos(new Vector2(0f, -725f), new Vector3(0f, 0f, 0f)));
        }
    }
}
