using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CButtonBehavior : MonoBehaviour
{
    [SerializeField] GameObject animationPrefab = null;
    [SerializeField] float objectMovementSpeed = 0f;
    [SerializeField] Image childImage = null;

    // prefab reference
    GameObject newPrefab = null;
    Canvas parentCanvas = null;

    // image to manipulate
    Image prefabImage = null;
    
    // prefab status to coordinate update
    bool prefabExists = false;


    //caching
    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }


    // instantiates prefab with the item icon at the point of the menu item
    public void SpawnPrefab(Vector3 start, Image icon)
    {
        newPrefab = Instantiate
            (animationPrefab, Camera.main.WorldToScreenPoint(start), Quaternion.identity, parentCanvas.transform);

        prefabImage = newPrefab.GetComponent<Image>();
        prefabImage.sprite = icon.sprite;

        // coordinates that prefab exists
        prefabExists = true;
    }


    // if the prefab exists, moves it towards the c button
    private void Update()
    {
        if(prefabExists == true)
        {
            float step = objectMovementSpeed * Time.deltaTime;
            newPrefab.transform.position = Vector3.MoveTowards(newPrefab.transform.position, transform.position, step);


            // if the item is approximately equal in position to the c button, sets the item to the button itself
            if (Vector3.Distance(newPrefab.transform.position, transform.position) < 0.005f)
                SetCButton();
        }
    }


    // sets c button graphic and destroys prefab
    void SetCButton()
    {
        childImage.sprite = prefabImage.sprite;

        Destroy(newPrefab);

        prefabImage = null;
        prefabExists = false;
    }
}
