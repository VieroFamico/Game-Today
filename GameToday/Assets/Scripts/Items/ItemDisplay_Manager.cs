using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay_Manager : MonoBehaviour
{
    public static ItemDisplay_Manager instance;

    public Animator showItemAnimator;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    public Button closeDisplayButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        closeDisplayButton.onClick.AddListener(CloseDisplay);
    }
    public void ShowItem(Base_Item_ScriptableObject itemToShow)
    {
        showItemAnimator.gameObject.SetActive(true);
        showItemAnimator.SetTrigger("Show");
        itemName.text = itemToShow.name;
        itemDescription.text = itemToShow.itemDescription;
        itemImage.sprite = itemToShow.ItemSprite;

        Player_Menus_Manager.instance.pauseMenuButton.gameObject.SetActive(false);
    }

    public void CloseDisplay()
    {
        showItemAnimator.SetTrigger("Hide");
        Player_Menus_Manager.instance.pauseMenuButton.gameObject.SetActive(true);
    }

    private IEnumerator DisableDisplayPanelAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        showItemAnimator.gameObject.SetActive(false);
    }
}
