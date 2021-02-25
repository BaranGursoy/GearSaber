using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GearAndGameControls : MonoBehaviour
{
    private Touch touch;
    private Vector2 dir;
    private float angle;
    private Quaternion rotation;
    private float smaller_y;
    public Transform saberPoint;
    private SpriteRenderer sr;

    Light2D lightOfSaber;
    GameObject saber;

    public Transform[] spriteBottom;
    
    int i;

    public static int deathCounter = 0;

    AudioSource audioSource;

    private void Start()
    {
        Application.targetFrameRate = 60;

        audioSource = GetComponent<AudioSource>(); //For general audio source

        FindObjectOfType<AudioManager>().Play("Humming");

        string color;
        color = PlayerPrefs.GetString("Color", "red");

        // Saber Color Choose
        if(color == "red")
        {
            saber = transform.Find("Sword").gameObject;
            saber.SetActive(true);
            i = 0;
        }

        else if (color == "blue")
        {
            saber = transform.Find("SwordBlue").gameObject;
            saber.SetActive(true);
            i = 1;
        }

        else if (color == "green")
        {
            saber = transform.Find("SwordGreen").gameObject;
            saber.SetActive(true);
            i = 2;
        }

        // Saber Color Choose is over

        lightOfSaber = saber.GetComponentInChildren<Light2D>(); // Get light component
        
        smaller_y = 1.35f; // Constant for scaling down the lightsaber


        if(PlayerPrefs.GetInt("Music",1) == 1) // If music is on play music
        {
            audioSource.volume = 0.6f;
        }

        if(PlayerPrefs.GetInt("Music", 1) == 0) // If music is off don't play music
        {
            audioSource.volume = 0f;
        }


    }


    void Update()
    {

        if(Input.touchCount>0)
        {
            touch = Input.GetTouch(0);

            // Calculations for rotating the gear
            if(touch.phase == TouchPhase.Moved)
            {
                dir = Camera.main.ScreenToWorldPoint(touch.position);
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                Quaternion currentRotation = transform.rotation;
                transform.rotation = Quaternion.RotateTowards(currentRotation, rotation, Time.deltaTime * 600f);        
            }
            // Calculations are over
        }


        if (PauseMenu.isGamePaused) // If game is paused change the audio pitch
        {
            audioSource.pitch = 0.5f;
        }

        else if(!PauseMenu.isGamePaused) // If game is resumed change back the audio pitch to its original
        {
            audioSource.pitch = 1f;
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }


        if (saber.transform.localScale.y <= 0)
        {
            if (Score.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", Score.score);
            }

            deathCounter++;

            if(deathCounter % 2 == 0) // At every 2 deaths show an ad
            {
                AdController.instance.ShowAd();
            }

            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        { 
            //Make the saber smaller if you an enemy collides with you
            saber.GetComponent<Transform>().localScale = new Vector3(saber.transform.localScale.x, saber.transform.localScale.y - smaller_y, saber.transform.localScale.z);
            Vector3 requiredDistance = spriteBottom[i].position - saberPoint.position;
            
            // Calculate the saber's new position
            saber.transform.position -= requiredDistance;
            saber.transform.rotation = transform.rotation;
            // Calculations are over

            lightOfSaber.pointLightOuterRadius -= 1.70f; // Make the light radius smaller

            FindObjectOfType<AudioManager>().Play("Hit"); // Play hit sound effect

            Destroy(collision.gameObject); // Destroy the enemy even if it hits you

        }
    }




}
