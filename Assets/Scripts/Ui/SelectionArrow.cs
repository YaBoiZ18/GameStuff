using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; // Move arrow up and down
    [SerializeField] private AudioClip interactSound; // When Oprion is selected
    private RectTransform rect;
    private int currentPositon;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Change position
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        //Interact with options
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

    }

    private void ChangePosition(int _change)
    {
        currentPositon += _change;

        if(_change != 0)
        {
            SoundManager.instance.PlaySound(changeSound);
        }

        if (currentPositon < 0)
        {
            currentPositon = options.Length - 1;
        }
        else if (currentPositon > options.Length - 1) 
        {
            currentPositon = 0;
        }

        //Assing the Y position of the current option to the arrow (moving it up and down)
        rect.position = new Vector3(rect.position.x, options[currentPositon].position.y, 0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        //Access the buttons
        options[currentPositon].GetComponent<Button>().onClick.Invoke();
    }
}
