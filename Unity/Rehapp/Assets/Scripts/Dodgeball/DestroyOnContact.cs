using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    int score;
    private DodgeballController gameController;
    public Collider[] ignoreCollider;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<DodgeballController>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < ignoreCollider.Length; i++)
        {
            Collider ownCol = GetComponent<Collider>();
            Physics.IgnoreCollision(ownCol, ignoreCollider[i]);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
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
