using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Container : MonoBehaviour
{
    public Puzzle_Room_Module puzzleRoomModule;
    public Base_Item_ScriptableObject itemSO;

    public float pickUpDistance;

    public bool isPickedUp;

    private SpriteRenderer spriteRenderer;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerState_Manager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInteractInput();
    }
    private void GetPlayerInteractInput()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < pickUpDistance)
        {
            if (Input.GetKey(KeyCode.E))
            {
                PickUp();
            }
        }
    }

    private void PickUp()
    {
        isPickedUp = true;
        puzzleRoomModule.CheckForItemsPickedUp();
        Destroy(gameObject);
    }
}
