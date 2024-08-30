using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Container : MonoBehaviour
{
    public Puzzle_Room_Module puzzleRoomModule;
    public Base_Item_ScriptableObject itemSO;

    public float pickUpDistance;

    public bool isPickedUp { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerState_Manager.instance.player.transform;
    }

    public void PickUp()
    {
        isPickedUp = true;
        if (puzzleRoomModule)
        {
            puzzleRoomModule.CheckForItemsPickedUp();
        }
        gameObject.SetActive(false);
    }
}
