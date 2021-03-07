using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    private static GameManager _instance;
    [SerializeField] private List<Card> cards;
    public static GameManager Instance { get { return _instance; } }

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
        SetUpGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject + " was hit");
            }
        }
    }
    private void SetUpGame()
    {
        Debug.Log(Screen.width + "  " + Screen.height);
        Debug.Log("Aspect Ratio: " + Camera.main.aspect);
        Debug.Log("Viewport w: " + Camera.main.aspect * Camera.main.orthographicSize);
        //setup  Camera.main.orthographicSize according to screen ratio etc.
        int x = 3;
        int y = 2;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float xPos = -1.5f + 1.5f * (float)i;
                float yPos = 4f - 1.5f * (float)j;
                Vector3 pos = new Vector3(xPos, yPos, 0.0f);
                Card newCard = InstantiateCard(pos);
                cards.Add(newCard);
            }
        }
    }

    private Card InstantiateCard(Vector3 pos)
    {
        GameObject newObj = Instantiate(cardPrefab, pos, Quaternion.identity) as GameObject;
        newObj.name = "Card[" + pos.x + "][" + pos.y + "]";
        Card newCard = newObj.GetComponent<Card>();
        newCard.Init(Resources.Load<Sprite>("images/IMG_2247"));
        return newCard;
    }
}
