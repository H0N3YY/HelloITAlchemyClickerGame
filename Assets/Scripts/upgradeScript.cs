using UnityEngine;

public class upgradeScript : MonoBehaviour
{
    
    private int tf = 0;
    public GameObject menuBttn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void UpgradeMenu ()
    {
        if (tf == 0)
        {
            menuBttn.SetActive(true);
            tf++;
        }
        else if (tf == 1)
        {
            menuBttn.SetActive(false);
            tf--;
        }
        
    }
    
}
