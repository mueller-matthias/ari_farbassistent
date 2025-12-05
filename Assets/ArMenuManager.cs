using UnityEngine;

public class ARMenuManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject arRoot;

    void Start()
    {
        mainMenuUI.SetActive(true);
        arRoot.SetActive(false);
    }

    public void OnStartAR()
    {
        mainMenuUI.SetActive(false);
        arRoot.SetActive(true);
    }
}
