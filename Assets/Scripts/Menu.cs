using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    private Animator animator;
    private CanvasGroup canvasGroup;
    private MessageTyper[] msgTyper;

    public bool IsOpen
    {
        get { return animator.GetBool("IsOpen"); }
        set { animator.SetBool("IsOpen", value); }
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        msgTyper = GetComponentsInChildren<MessageTyper>();

        //moves menues back to screen when going from design mode to play mode
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }

    public void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
        }

        //todo set condition of transition to start typer
        /*
        foreach(MessageTyper typer in msgTyper)
        {
            typer.StartCoroutine("TypeIn");
        }
        foreach(MessageTyper typer in msgTyper)
        {
            typer.StartCoroutine("TypeOff");
        }
        */

    }
}
