using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public int healthCount;
    public int coinCount;
    int Coinleft;

    private Rigidbody2D rb;
    private Animator animator;

    public Text Healthtext;
    public Text Cointext;

    public AudioClip[] AudioClipArr;
    private AudioSource audioSource;

    bool isOnground = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Healthtext.GetComponent<Text>().text = ("Health: " + healthCount);
        Cointext.GetComponent<Text>().text = ("Coin: " + coinCount);

    }

    // Update is called once per frame
    void Update()
    {
        movement();

        Coinleft = GameObject.FindGameObjectsWithTag("Coin").Length;
        if(healthCount == 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
        if(Coinleft == 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            healthCount -= 10;
            Healthtext.GetComponent<Text>().text = ("Health: " + healthCount);
            audioSource.PlayOneShot(AudioClipArr[2]);

        }

        if (collision.gameObject.tag == "Coin")
        {
            coinCount++;
            Destroy(collision.gameObject);
            Cointext.GetComponent<Text>().text = ("Coin: " + coinCount);
            audioSource.PlayOneShot(AudioClipArr[0]);
        }

        if(collision.gameObject.tag == "Ground")
        {
            isOnground = true;
        }
    }

    private void movement()
    {
        float hVelocity = 0;
        float vVelocity = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hVelocity = -moveSpeed;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            hVelocity = moveSpeed;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));

        }
        else
        {
            animator.SetFloat("xVelocity", 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnground == true)
        {
            vVelocity = jumpForce;
            animator.SetTrigger("Jumptrigger");
            audioSource.PlayOneShot(AudioClipArr[3]);
            isOnground = false;
        }

        hVelocity = Mathf.Clamp(rb.velocity.x + hVelocity, -5, 5);

        rb.velocity = new Vector2(hVelocity, rb.velocity.y + vVelocity);
    }
}
