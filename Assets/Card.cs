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

    private void Awake()
    {
        backImage = Resources.Load<Sprite>("images/CardBack");
    }

    void Start()
    {
    }

    public void Init(Sprite sprite)
    {
        frontImage = sprite;
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
        Debug.Log(gameObject.name + " flipped, current value: " + flipped);
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
                ToggleSprite();
                spriteToggled = true;
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
}
