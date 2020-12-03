using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] CardBase cardBase;
	
	[SerializeField] Text cardTitle; 
	[SerializeField] Text atk; 
	[SerializeField] Text hp; 
	[SerializeField] Text mana;
	[SerializeField] Image art;

    Canvas canvas;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;


    Color originalColor = Color.white;
    Color highlightenedColor = Color.red;

    public void ChangeValue(int what)
    {
        int localRand = UnityEngine.Random.Range(-2, 9);
        switch (what)
        {
            case 0:
                StartCoroutine(CounterTextChange(atk, localRand));
                break;
            case 1:
                StartCoroutine(CounterTextChange(hp, localRand));
                if(localRand <= 0)
                {
                    DisposeCard();
                }
                break;
            case 2:
                StartCoroutine(CounterTextChange(mana, localRand));
                break;
        }
    }

    void DisposeCard()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        StartCoroutine(FlyAway());
        GameManager.Instance.HandTransform.GetComponent<HandController>().RearrangeCards();
    }

    IEnumerator FlyAway()
    {
        Vector3 targetPoisition = transform.position + new Vector3(0f, 300f, 0f);
        Vector3 startPosition = transform.position;
        float step = 0;
        while(transform.position != targetPoisition)
        {
            step += 0.02f;
            transform.position = Vector3.Lerp(startPosition, targetPoisition, step);
            yield return null;
            if (Vector3.Distance(startPosition, targetPoisition) < 0.1f)
            {
                transform.position = targetPoisition;
                break;
            }
        }
        Destroy(gameObject);
    }

    IEnumerator CounterTextChange(Text text, int value)
    {
        int localStartValue = Int32.Parse(text.text);
        if (localStartValue < value)
        {
            for (int i = localStartValue; i <= value; i++)
            {
                yield return new WaitForSeconds(0.1f);
                text.text = i.ToString();
            }
        }
        else if (localStartValue > value)
        {
            for (int i = localStartValue; i >= value; i--)
            {
                yield return new WaitForSeconds(0.1f);
                text.text = i.ToString();
            }
        }
        else
        {
            value = UnityEngine.Random.Range(-2, 9);
            StartCoroutine(CounterTextChange(text, value));
        }
    }

    public IEnumerator MoveToPos(Vector2 targetPos, Vector3 targetEuler)
    {
        Vector2 startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        Vector3 startEuler = transform.GetComponent<RectTransform>().eulerAngles;
        float step = 0;
        while (startPos != targetPos)
        {
            step += 0.5f;
            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, targetPos, step);
            transform.eulerAngles = Vector3.Lerp(startEuler, targetEuler, step);
            if(Vector2.Distance(transform.GetComponent<RectTransform>().anchoredPosition, targetPos) <= 0.05f || Vector3.Distance(transform.eulerAngles, targetEuler) <= 0.05f)
            {
                break;
            }
        }
        transform.GetComponent<RectTransform>().anchoredPosition = targetPos;
        transform.eulerAngles = targetEuler;
        yield return null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.localEulerAngles = Vector3.zero;
        transform.position = Input.mousePosition;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        if (transform.parent != GameManager.Instance.PanelTransform)
        {
            canvasGroup.blocksRaycasts = true;
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GameManager.PivotOffset);
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GameManager.CardDistanceFromPivot);
            transform.SetParent(GameManager.Instance.HandTransform);
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
        }
        GameManager.Instance.HandTransform.GetComponent<HandController>().RearrangeCards();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).GetChild(1).GetComponent<Image>().color = highlightenedColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).GetChild(1).GetComponent<Image>().color = originalColor;
    }

    void Awake()
	{
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
		DisplayCard();
	}
	void DisplayCard()
	{
        cardTitle.text = cardBase.CardName;
		atk.text = cardBase.Atk.ToString();
		hp.text = cardBase.HP.ToString();
		mana.text = cardBase.Mana.ToString();
        StartCoroutine(GetArt());
	}
	IEnumerator GetArt()
	{
		using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("http://www.picsum.photos/170", true))
		{
			yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
			{
				Debug.Log(uwr.error);
			}
			else
			{
                var texture = DownloadHandlerTexture.GetContent(uwr);
                if (texture != null)
                {
                    Sprite newSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 170);
                    cardBase.Art = newSprite;
                    art.sprite = newSprite;
                    art.useSpriteMesh = true;
                }
			}
		}
	}
}
