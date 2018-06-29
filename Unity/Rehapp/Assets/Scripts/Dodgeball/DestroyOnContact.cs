using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    int score;
    private DodgeballController gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<DodgeballController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
        if (other.gameObject.tag == "BallBomb")
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
        if (other.gameObject.tag == "Player")
        {
            if (this.tag == "BallBomb")
            {
                score = -1;
                //instantiate player explosion
            }
            else
            {
                score = 1;
                //instantiate Ball explosion
            }
            StartCoroutine(DestoyThis());
        }
        if (other.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
     IEnumerator DestoyThis()
    {
        yield return new WaitForSeconds(1);
        gameController.AddScore(score);
        Destroy(gameObject);
    }
}
