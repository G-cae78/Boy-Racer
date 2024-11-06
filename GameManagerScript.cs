using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript gameManager;
    public Rigidbody playerCar;

    public GameObject gameMenu;
    public Button pause;
    public bool enterMenu;

    public Vector3 carDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterMenu=false;
        gameMenu.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    public void pauseControl(){
        if(enterMenu==true){
            enterMenu=false;
            gameMenu.SetActive(false);
        }
        else{ 
        enterMenu=true;
        menu();
        }
    }
    public void menu(){
        if(enterMenu)
        gameMenu.SetActive(true);
    } 
}
