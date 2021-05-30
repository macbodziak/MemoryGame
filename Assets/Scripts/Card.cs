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
        StartCoroutine("RotateCard");
    }

    IEnumerator RotateCard()
    {
        Transform tran = gameObject.transform;
        float yRotation = 0.0f;
        float deltaRot;
        bool spriteToggled = false;
        isRotating = true;
        do
        {
            deltaRot = 1.0f * rotationSpeed * Time.deltaTime;
            yRotation += deltaRot;
            if (yRotation >= 90.0f && spriteToggled == false)
            {
                spriteToggled = true;
                ToggleSprite();
            }
            if (yRotation > 180.0f)
            {
                deltaRot = 180.0f - tran.rotation.eulerAngles.y % 180.0f;
            }
            tran.Rotate(0.0f, deltaRot, 0.0f, Space.Self);

            yield return null;
        }
        while (yRotation < 180.0f);
        isRotating = false;
        yield return null;
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
        spriteR.flipX = !spriteR.flipX;
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
}
