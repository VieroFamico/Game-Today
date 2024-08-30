using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Interact_Manager : MonoBehaviour
{
    public TextMeshProUGUI interactPromptText;
    public string promptMessage = "Press E to pick up";
    public float promptOffset = 1f;

    private Transform closestItem;
    private float closestDistance;
    private List<Item_Container> nearbyItems = new List<Item_Container>();
    private Transform player;

    void Start()
    {
        player = PlayerState_Manager.instance.player.transform;
        interactPromptText.enabled = false;
    }

    void Update()
    {
        UpdateNearbyItems();
        ShowInteractPrompt();
    }

    private void UpdateNearbyItems()
    {
        nearbyItems.Clear();
        Item_Container[] items = FindObjectsOfType<Item_Container>();

        closestItem = null;
        closestDistance = Mathf.Infinity;

        foreach (var item in items)
        {
            float distance = Vector2.Distance(item.transform.position, player.position);
            if (distance < item.pickUpDistance && !item.isPickedUp)
            {
                nearbyItems.Add(item);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item.transform;
                }
            }
        }
    }

    private void ShowInteractPrompt()
    {
        if (closestItem != null)
        {
            interactPromptText.enabled = true;
            interactPromptText.text = promptMessage;
            interactPromptText.transform.position = closestItem.position + Vector3.up * promptOffset;

            if (Input.GetKeyDown(KeyCode.E))
            {
                closestItem.GetComponent<Item_Container>().PickUp();
            }
        }
        else
        {
            interactPromptText.enabled = false;
        }
    }
}
