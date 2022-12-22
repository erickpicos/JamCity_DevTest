using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject panelA;
    public GameObject panelB;
    public GameObject buttonShowBlue;
    public GameObject buttonHideAll;
    
    public void HideAll()
    {
        panelA.SetActive(false);
        panelB.SetActive(false);
        buttonHideAll.SetActive(false);
        buttonShowBlue.SetActive(true);
    }
    public void ShowBlue()
    {
        panelA.SetActive(true);
        panelB.SetActive(false);
        buttonHideAll.SetActive(true);
        buttonShowBlue.SetActive(false);
    }
    public void ChangePanels()
    {
        panelA.SetActive(!panelA.activeSelf);
        panelB.SetActive(!panelB.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(buttonShowBlue.activeSelf){ ShowBlue();}
            else if(buttonHideAll.activeSelf){ HideAll();}
        }
    }
}
