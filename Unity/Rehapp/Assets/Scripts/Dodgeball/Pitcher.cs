using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Rigidbody[] Ball;
    public DodgeballMechanics dM;

    [HideInInspector]
    public int maxPoints;
    [HideInInspector]
    public int minPoints;
    [HideInInspector]
    public bool shootOver;

    float fireRate;
    float forceAmount;
    int globalCamWidth;
    int globalCamHeigth;
    Mesh playerMesh;

    void Start()
    {
        globalCamWidth = (int)GlobalCam.CamDimensions().x;
        globalCamHeigth = (int)GlobalCam.CamDimensions().y;
    }

    void OnEnable()
    {
        dM = GameObject.Find("Player").GetComponent<DodgeballMechanics>();
        playerMesh = dM.GetBodyMesh();
        shootOver = false;
    }

    public void StartShooting()
    {
        StartCoroutine(ShootBall());
    }

    IEnumerator ShootBall()
    {
        yield return new WaitForSeconds(2);
        GameController.onGame = true;
        while (true)
        {
            //transform.position = new Vector3(Random.Range(5, 635), Random.Range(5, 475), transform.position.z);
            float fireRateR = Random.Range(fireRate - 0.5f, fireRate + 0.5f);
            float forceAmountR = Random.Range(forceAmount - 20, forceAmount + 20);
            int randomBall = Random.Range(0, Ball.Length);
            Rigidbody ballRigid;
            if (dM.playerDetected)
            {
                if (randomBall == 0)
                {
                    // outside
                    float xPos = Random.Range(5, globalCamWidth - 5);
                    float yPos;
                    if (xPos < playerMesh.bounds.max.x && xPos > playerMesh.bounds.min.x)
                        yPos = Random.Range(playerMesh.bounds.max.y, globalCamHeigth - 5);
                    else
                        yPos = Random.Range(5, globalCamHeigth - 5);
                    transform.position = new Vector3(xPos, yPos, transform.position.z);
                    maxPoints++;
                }
                else
                {
                    //Inside
                    transform.position = new Vector3(Random.Range(playerMesh.bounds.min.x, playerMesh.bounds.max.x),
                        Random.Range(playerMesh.bounds.min.y, playerMesh.bounds.max.y), transform.position.z);
                    minPoints--;
                }
                ballRigid = Instantiate(Ball[randomBall], transform.position, transform.rotation) as Rigidbody;

            }
            else
            {
                transform.position = new Vector3(Random.Range(5, globalCamWidth - 5),
                    Random.Range(5, globalCamHeigth - 5), transform.position.z);
                ballRigid = Instantiate(Ball[0], transform.position, transform.rotation) as Rigidbody;
                maxPoints++;
            }
            yield return new WaitForSeconds(fireRateR);
            ballRigid.AddForce(transform.forward * forceAmountR);
            if (!GameController.onGame)
            {
                while (ballRigid != null)
                    yield return null;
                shootOver = true;
                break;
            }
        }
    }

    public void SetDifficult(int lvl)
    {
        fireRate = -0.09f * lvl + 5;
        forceAmount = 1700 * lvl + 15000;
    }
}
