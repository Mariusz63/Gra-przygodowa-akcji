using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Singleton 
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject selectedObject;   //We want this particular object
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotIcon;
    public Image handIcon;

    public bool handIsVisible; //if hand is visible we wont to swing the tools

    //public GameObject selectedTree;
    public GameObject chopHolder;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject ourInteractable = selectionTransform.GetComponent<InteractableObject>();

            NPC npc = selectionTransform.GetComponent<NPC>();

            if(npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButton(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if(DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            //ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            //if (choppableTree && choppableTree.playerInRange)
            //{
            //    choppableTree.canBeChopped = true;
            //    selectedTree = choppableTree.gameObject;
            //    chopHolder.gameObject.SetActive(true);
            //}
            //else
            //{
            //    if(selectedTree != null)
            //    {
            //        selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
            //        selectedTree = null;
            //        chopHolder.gameObject.SetActive(false);
            //    }
            //}

            if (ourInteractable && ourInteractable.graczWZasiegu)
            {
                onTarget = true;
                selectedObject = ourInteractable.gameObject;

                interaction_text.text = ourInteractable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (ourInteractable.CompareTag("Pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;

                }
            }
            else // if there is a hit, but without an Interactable Script
            {
                onTarget = false;
               // interaction_Info_UI.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                handIsVisible = false;
            }

        }
        else // if there is no hit at all
        {
            interaction_Info_UI.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            handIsVisible = false;
        }
    }


    public void DisableSelection() 
    { 
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);
        interaction_text.text = null;

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}