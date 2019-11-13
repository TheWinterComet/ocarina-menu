using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateEquip : MonoBehaviour
{
    [SerializeField] float objectMovementSpeed = 1f;

    GameObject cButtonReference = null;
    Image heldItemImage = null;
    Item heldItem = null;

    private void Awake()
    {
        heldItemImage = GetComponent<Image>();
    }

    public void Init(string cbutton, Image icon)
    {
        if (cbutton == "left")
            cButtonReference = GameObject.Find("cleft_img");
        if (cbutton == "down")
            cButtonReference = GameObject.Find("cdown_img");
        if (cbutton == "right")
            cButtonReference = GameObject.Find("cright_img");

        Debug.Log(icon.name);

        heldItemImage.sprite = icon.sprite;
        Debug.Log(cButtonReference.transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        float step = objectMovementSpeed * Time.deltaTime;
    }


    void SetCButton()
    {
        Image image = cButtonReference.GetComponentInChildren<Image>();
        image.sprite = heldItemImage.sprite;

        Destroy(gameObject);
    }
}
