using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] AudioClip cardFlippedSound;
    [SerializeField] AudioClip wrongSound;
    [SerializeField] AudioClip rightSound;
    AudioSource audioSource;
    private static GameManager _instance;
    private int cardsFlipped = 0;
    private int numberOfPairs;
    private int pairsLeft;
    bool inputBlocked = false;
    [SerializeField] float delay;
    Card cardOne = null;
    Card cardTwo = null;
    List<Sprite> sprites;
    public static GameManager Instance { get { return _instance; } }
    string[] imageFileNames = { "Card01", "Card02", "Card03", "Card04", "Card05", "Card06", "Card07", "Card08" };

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null);
        Debug.Assert(cardFlippedSound);
        Debug.Assert(wrongSound);
        Debug.Assert(rightSound);
        numberOfPairs = 3;
        LoadSprites();
        SetUpCards();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && inputBlocked == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Card card = hit.collider.gameObject.GetComponent<Card>();
                OnCardClicked(card);
            }
        }
    }

    private void OnCardClicked(Card card)
    {
        if (card.IsFlipped())
        {
            return;
        }
        audioSource.Stop();
        audioSource.clip = cardFlippedSound;
        audioSource.Play();
        cardsFlipped++;
        card.FlipCard();
        if (cardsFlipped == 1)
        {
            cardOne = card;
        }
        else if (cardsFlipped == 2)
        {
            cardTwo = card;
            inputBlocked = true;
            //check match
            if (IsMatch())
            {
                StartCoroutine("DiscardCardsOnMatch");
                pairsLeft--;
                if (pairsLeft == 0)
                {
                    OnGameOver();
                }
            }
            else
            {
                StartCoroutine("ResetCards");
            }
        }
    }

    IEnumerator ResetCards()
    {
        yield return new WaitForSeconds(delay);
        cardOne.FlipCard();
        cardTwo.FlipCard();
        yield return new WaitForSeconds(0.1f);
        audioSource.Stop();
        audioSource.clip = wrongSound;
        audioSource.Play();
        inputBlocked = false;
        cardsFlipped = 0;
        yield return null;
    }

    private void SetUpCards()
    {
        pairsLeft = numberOfPairs;
        // Debug.Log(Screen.width + "  " + Screen.height);
        // Debug.Log("Aspect Ratio: " + Camera.main.aspect);
        // Debug.Log("Viewport w: " + Camera.main.aspect * Camera.main.orthographicSize);
        //setup  Camera.main.orthographicSize according to screen ratio etc.
        int[] pool = CreatePool();
        int x = 3;
        int y = 2;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float xPos = -1.5f + 1.5f * (float)i;
                float yPos = 4f - 1.5f * (float)j;
                Vector3 pos = new Vector3(xPos, yPos, 0.0f);
                Card newCard = InstantiateCard(pos, pool[i + j * x]);
            }
        }
    }

    private Card InstantiateCard(Vector3 pos, int id)
    {
        GameObject newObj = Instantiate(cardPrefab, pos, Quaternion.identity) as GameObject;
        newObj.name = "Card[" + pos.x + "][" + pos.y + "]";
        Card newCard = newObj.GetComponent<Card>();
        newCard.Init(sprites[id], id);
        return newCard;
    }

    private bool IsMatch()
    {
        if (cardOne == null || cardTwo == null)
        {
            return false;
        }
        else if (cardOne.Id == cardTwo.Id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LoadSprites()
    {
        sprites = new List<Sprite>();
        for (int i = 0; i < numberOfPairs; i++)
        {
            Sprite item = Resources.Load<Sprite>("images/" + imageFileNames[i]);
            if (item != null)
            {
                sprites.Add(item);
            }
            else
            {
                Debug.Log("Failed to load sprites! Application Closing!");
                Application.Quit();
            }

        }
    }

    private int[] CreatePool()
    {
        int[] pool = new int[numberOfPairs * 2];
        for (int i = 0; i < numberOfPairs; i++)
        {
            pool[2 * i] = i;
            pool[2 * i + 1] = i;
        }

        //shuffle 
        for (int i = 0; i < numberOfPairs * 2 - 1; i++)
        {
            int temp = pool[i];
            int j = Random.Range(i + 1, numberOfPairs * 2);
            pool[i] = pool[j];
            pool[j] = temp;
        }


        return pool;
    }

    IEnumerator DiscardCardsOnMatch()
    {
        yield return new WaitForSeconds(delay);
        cardOne.Discard();
        cardTwo.Discard();
        audioSource.Stop();
        audioSource.clip = rightSound;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        inputBlocked = false;
        cardsFlipped = 0;
        yield return null;
    }

    public void OnGameOver() 
    {
        Debug.Log("GAME WON");
    }
}
