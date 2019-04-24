using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    private Rigidbody2D rb2d;
    private int count;
    private int lives;
 

    public bool flipX;


    public float speed;
    public float hurt;
    public float jumpForce;
   


    public Text livesText;
    public Text winText;
    public Text scoreText;
    public Text loseText;
    public Text instructionText;
   

    public AudioSource musicSource;
    public AudioSource jumpSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        lives = 3;
        winText.text = "";
        loseText.text = "";
        instructionText.text = "Collect all four coins and enter the door!";
       
        SetCountText();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

    }



    // Update is called once per frame


    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);


        if (Input.GetKey("escape"))
            {
            Application.Quit();
        }

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 2);
            jumpSource.clip = musicClipThree;
            jumpSource.Play();
            jumpSource.loop = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
   

        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag("Door"))
        {
            transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            lives = 3;
            SetCountText();

        }

        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            rb2d.AddForce(new Vector2(hurt, 0), ForceMode2D.Impulse);
            lives = lives - 1; // this removes 1 from the score
            SetCountText();
        }

        if (lives == 0)
        {
            Destroy(gameObject);

            if (Input.GetKey("escape"));
            Application.Quit();
        }

        if (count == 8)
        {
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);

            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void SetCountText()
    {
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + count;
        if (count == 8)
        {
            winText.text = "You Win!";

        }

        if (lives <= 0)
        {
            loseText.text = "You Lose!";
        }
    }


}
