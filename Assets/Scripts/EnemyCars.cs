using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyCars : MonoBehaviour
{
    #region public field
    public GameObject[] Nodes;
    public GameObject[] SpeedBooster_particle;
    public GameObject[] tierSmoke_partilce;
    public GameObject[] Booster_particle;
    public Transform[] wheelMesh;
    public Transform PlayerCar;
    //int ,float,bool,vector etc  
    public int boostspeed;
    public int ActualSpeed;
    public int TurnTimeSpeed;
    public int nodeval;
    public float turnspeed = .8f;
    public float movespeed = 25f;
    public float Turntimer_val, TurnFixed_val;
    public bool Turn, fixingTurn;
    private bool Turntimer, Activatecar;
    // public float Dis_Btw_PlayerCar;

    bool isBoostspeed;
    bool isSpeedSlow;
    bool isSpeedNormal;
    bool IsRaceFinished = false;
    bool isGameOver = false;
    #endregion

    #region private field

    private Rigidbody carRb;
    #endregion


    //script reference
    UImanager uimanager;
    carcontroller1 carcontroller;

    #region monobehaviour callback
    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("UIManager").GetComponent<UImanager>();
        carcontroller = GameObject.Find("Cars").GetComponent<carcontroller1>();
        carRb = GetComponent<Rigidbody>();
        StartCoroutine(speedBoost());
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(PlayerCar.position, this.transform.position) < 400f)
        {

            Activatecar = true;

        }
        if (Turntimer)
        {
            //if (turnpoint == 1)
            //{
            LeftTurn();
            RightTurn();
            // when car is turning then smoke particle will play
            Booster_particle[0].SetActive(false);
            Booster_particle[1].SetActive(false);
            tierSmoke_partilce[0].SetActive(true);
            tierSmoke_partilce[1].SetActive(true);
            // }
        }
        else
        {   //when car is moving forward then smoke particle will not play
            tierSmoke_partilce[0].SetActive(false);
            tierSmoke_partilce[1].SetActive(false);
            Booster_particle[0].SetActive(true);
            Booster_particle[1].SetActive(true);
            if (!IsRaceFinished && !isGameOver)
            {
                if (!isBoostspeed && isSpeedNormal)
                {
                    movespeed = ActualSpeed;
                    carRb.drag = 1.5f;
                }
                else if (isSpeedSlow)
                {

                    movespeed = 150f;
                }
            }

            //if (isGameOver)
            //{
            //    Booster_particle[0].SetActive(true);
            //    Booster_particle[1].SetActive(true);
            //}
            //else if (!isGameOver)
            //{
            //    Booster_particle[0].SetActive(false);
            //    Booster_particle[1].SetActive(false);
            //}

        }
        if (Activatecar && carcontroller.isStartGame)
        {
            moveforward();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Node")
        {
            //  nodeval++;
            Turn = !Turn;
            Turntimer = true;
            StartCoroutine(StopTurning());
        }
        //if (other.gameObject.tag == "Coin")
        //{
        //    Destroy(other.gameObject);
        //}
        if (other.gameObject.tag == "SpeedSlow")
        {
            isSpeedSlow = true;
            isSpeedNormal = false;
        }
        if (other.gameObject.tag == "RaceDone")
        {
            if (!uimanager.isRaceDone)
            {
                uimanager.RacePosition++;

            }
        }
        //if (other.gameObject.tag == "SpeedNormal")
        //{
        //    isSpeedNormal = true;
        //    isSpeedSlow = false;

        //}
        if (other.gameObject.tag == "Boost")
        {
            StartCoroutine(speedBoost());
        }
        if (other.gameObject.tag == "RaceFinished")
        {
            IsRaceFinished = true;
            movespeed = 0;
            turnspeed = 0f;

        }
        if (other.gameObject.tag == "Respawn")
        {

            isGameOver = true;
            carRb.freezeRotation = false;
            carRb.angularDrag = .05f;
            carRb.drag = 0f;
            carRb.mass = 2000f;
            movespeed = 0f;
            carRb.velocity = carRb.velocity / 2.5f;

        }
    }
    #endregion

    #region private field
    IEnumerator speedBoost()
    {
        //Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1f;
        //Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 2f;
        isBoostspeed = true;
        Booster_particle[0].SetActive(false);
        Booster_particle[1].SetActive(false);
        SpeedBooster_particle[0].SetActive(true);
        SpeedBooster_particle[1].SetActive(true);
        movespeed = boostspeed;
        yield return new WaitForSeconds(2);
        isBoostspeed = false;
        movespeed = ActualSpeed;
        SpeedBooster_particle[0].SetActive(false);
        SpeedBooster_particle[1].SetActive(false);
        yield return new WaitForSeconds(.5f);
        Booster_particle[0].SetActive(true);
        Booster_particle[1].SetActive(true);
        //Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        //Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
    }
    // Update is called once per frame
   
    void moveforward()
    {
        carRb.AddRelativeForce(Vector3.forward * 1 * movespeed, ForceMode.Acceleration);

    }
    void LeftTurn()
    {
        if (Turn)
        {
            movespeed = TurnTimeSpeed;
            carRb.drag = 3f;
            carRb.AddRelativeTorque(Vector3.up * 1 * turnspeed, ForceMode.VelocityChange);
            wheelMesh[0].localRotation = Quaternion.Euler(0, 30, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0,30, 0);

        }

    }
    void RightTurn()
    {
        if (!Turn)
        {
            movespeed = TurnTimeSpeed;
            carRb.drag = 3f;
            carRb.AddRelativeTorque(Vector3.up * -1 * turnspeed, ForceMode.VelocityChange);
            wheelMesh[0].localRotation = Quaternion.Euler(0, -30, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, -30, 0);
        }

    }
    IEnumerator FixedCarTurn()
    {

        while (fixingTurn)
        {
            this.carRb.DORotate(new Vector3(0, 90, 0), 1f, RotateMode.Fast);
            wheelMesh[0].localRotation = Quaternion.Euler(0, 0, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, 0, 0);
            movespeed = ActualSpeed;
            carRb.drag = 1.5f;
            break;
        }
        while (!fixingTurn)
        {
            this.carRb.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.Fast);
            wheelMesh[0].localRotation = Quaternion.Euler(0, 0, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, 0, 0);
            movespeed = ActualSpeed;
            carRb.drag = 1.5f;
            break;
        }
        yield return null;
    }
    IEnumerator StopTurning()
    {
        yield return new WaitForSeconds(Turntimer_val);
        Turntimer = false;
        yield return new WaitForSeconds(TurnFixed_val);

        fixingTurn = !fixingTurn;
        StartCoroutine(FixedCarTurn());
    }
    #endregion

}
