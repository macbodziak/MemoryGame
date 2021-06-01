using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    bool flipped = false;
    bool isRotating = false;
    [SerializeField] float rotationSpeed;
    Sprite frontImage;
    Sprite backImage;
    private int id;
    Animator animator;

    public int Id
    {
        get {return id;}
    }

    private void Awake()
    {
        backImage = Resources.Load<Sprite>("images/CardBack");
        animator = GetComponent<Animator>();
    }


    public void Init(Sprite sprite, int _id)
    {
        frontImage = sprite;
        id = _id;
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && isRotating == false)
        {
            FlipCard();
        }
    }

    public void FlipCard()
    {
        flipped = !flipped;
        animator.SetTrigger("Flip");
    }

    void ToggleSprite()
    {
        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        if (flipped)
        {
            spriteR.sprite = frontImage;
        }
        else
        {
            spriteR.sprite = backImage;
        }
    }

    public bool IsFlipped()
    {
        return flipped;
    }

    public void SetPosition(Vector2 pos)
    {

    }

    public void Discard()
    {
        animator.SetTrigger("Destroy");
    }

    void DestroySelf()
    {
        Destroy(gameObject); 
    }

    void OnEndFlipAnimation()
    {
        isRotating = false;
    }
}
