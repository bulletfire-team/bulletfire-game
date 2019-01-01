using UnityEngine;
using TMPro;

public class BoxMenu : MonoBehaviour
{

    private Server server;
    
    public GameObject boxStuff;
    public Animator anim;
    public TMP_Text nbBoxTxt;
    

    public void OpenBox ()
    {
        if(server.player.NbCoffres > 0)
        {
            server.player.NbCoffres--;
            anim.SetTrigger("open");
            nbBoxTxt.text = server.player.NbCoffres + " coffres disponibles";
        }
    }

    private void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        nbBoxTxt.text = server.player.NbCoffres + " coffres disponibles";
    }

    private void OnEnable()
    {
        boxStuff.SetActive(true);
    }

    private void OnDisable()
    {
        boxStuff.SetActive(false);
    }
}
