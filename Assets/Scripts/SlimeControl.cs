using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeControl : MonoBehaviour
{

    public float moveSpeed;
    public float distance;
    public float restartDelay;

    public Vector3 startPos1;
    public Vector3 startPos2;

    public Transform currentPoint;
    public GameObject Slime;
    private Animator animator;



    void Start()
    {
        animator = GetComponent<Animator>();
        startPos1 = Slime.transform.position;
    }
   

    void Update()
    {

        Slime.transform.position = Vector3.MoveTowards(Slime.transform.position, startPos1, Time.deltaTime * moveSpeed);

        if (Slime.transform.position == startPos1)
        {
            startPos1 = startPos2;

        }

        if (startPos1 == startPos2)
        {
            startPos2 = Slime.transform.position;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("carpisma1 ");
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Invoke("RestartGame", restartDelay);
            Debug.Log("carpisma2 ");
        }

    }

  

    void RestartGame()
    {
        //StartCoroutine(WaitAndRestart());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator WaitAndRestart()
    {
        yield return new WaitForSeconds(restartDelay);
    }



}
