using UnityEngine;

public class MaskSelection : MonoBehaviour
{
    public GameObject maskSelectionParent; 
    public GameObject dungeon;             
    public GameObject player;
    public GameObject mansion;
    public GameObject home;
    public GameObject spawnPoint;    

    void Start()
    {
        dungeon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Gold Mask")
                {
                    SelectGold();
                }
                if (hit.collider.gameObject.name == "Silver Mask")
                {
                    SelectSilver();
                }
                if (hit.collider.gameObject.name == "Bronze Mask")
                {
                    SelectBronze();
                }
            }
        }
    }

    void SelectGold()
    {
        maskSelectionParent.SetActive(false);
        dungeon.SetActive(true);
        player.transform.position = spawnPoint.transform.position;
    }

    void SelectSilver()
    {
        maskSelectionParent.SetActive(false);
        mansion.SetActive(true);
        player.transform.position = spawnPoint.transform.position;
    }

    void SelectBronze()
    {
        maskSelectionParent.SetActive(false);
        home.SetActive(true);
        player.transform.position = spawnPoint.transform.position;
    }
}