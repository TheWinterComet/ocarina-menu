using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CButtonBehavior : MonoBehaviour
{
    [SerializeField] DisplayItems displayItems = null;
    [SerializeField] GameObject animationPrefab = null;
    [SerializeField] Image CLeftImage = null, CDownImage = null, CRightImage = null;
    [SerializeField] float objectMovementSpeed = 700f;

    // object references
    GameObject newPrefab = null;
    Canvas parentCanvas = null;
    Image prefabImage = null;
    Image targetImage = null;

    // prefab status to coordinate update
    bool prefabExists = false;
    int currentAmmo = -1;

    //caching
    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }


    // instantiates prefab with the item icon at the point of the menu item
    public void SpawnPrefab(Vector3 start, Image icon, string cButton, int ammoCount)
    {
        if(prefabExists == false)
        {
            // sets prefab data
            newPrefab = Instantiate
                (animationPrefab, Camera.main.WorldToScreenPoint(start), Quaternion.identity, parentCanvas.transform);
            prefabImage = newPrefab.GetComponent<Image>();
            prefabImage.sprite = icon.sprite;
            currentAmmo = ammoCount;


            // determines which C Button image to target
            if (cButton == "left")
                targetImage = CLeftImage;
            if (cButton == "down")
                targetImage = CDownImage;
            if (cButton == "right")
                targetImage = CRightImage;


            // coordinates that prefab exists
            prefabExists = true;
        }
    }


    // if the prefab exists, moves it towards the c button
    private void Update()
    {
        TransformPrefab();
    }


    // moves prefab (if it exists) towards a target location
    void TransformPrefab()
    {
        if (prefabExists == true)
        {
            float step = objectMovementSpeed * Time.deltaTime;
            newPrefab.transform.position = Vector3.MoveTowards(newPrefab.transform.position, targetImage.gameObject.transform.position, step);


            // if the item is approximately equal in position to the c button, sets the item to the button itself
            if (Vector3.Distance(newPrefab.transform.position, targetImage.gameObject.transform.position) < 0.005f)
                SetCButton();
        }
    }

    // sets c button graphic and destroys prefab
    void SetCButton()
    {
        // ensures that there are no duplicate images or information in the cButtons
        if (CLeftImage.sprite == prefabImage.sprite)
            CheckDuplicates(CLeftImage);      
        if (CRightImage.sprite == prefabImage.sprite)
            CheckDuplicates(CRightImage);
        if (CDownImage.sprite == prefabImage.sprite)
            CheckDuplicates(CDownImage);


        // sets new item image
        targetImage.sprite = prefabImage.sprite;


        // if item has ammunition, sets the ammunition graphic, else sets it to blank
        Text ammo = targetImage.GetComponentInChildren<Text>();
        if (currentAmmo >= 0)
            ammo.text = currentAmmo.ToString();
        else if (currentAmmo < 0)
            ammo.text = "";
        currentAmmo = -1;


        // resets the equpped displays
        displayItems.SetEquippedGraphics(CLeftImage, CRightImage, CDownImage);


        // destroys prefab
        Destroy(newPrefab);
        prefabImage = null;
        prefabExists = false;
    }

    void CheckDuplicates(Image imageToCheck)
    {
        imageToCheck.sprite = targetImage.sprite;
        Text ammo = CLeftImage.GetComponentInChildren<Text>();
        ammo.text = targetImage.GetComponentInChildren<Text>().text;
    }
}
