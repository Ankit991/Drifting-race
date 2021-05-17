using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class carcontroller1 : MonoBehaviour
{
    #region public field
    public GameObject Followcam;
    public GameObject castray_object;
    public GameObject[] SpeedBooster_particle;
    public GameObject[] tierSmoke_partilce;
    public GameObject[] Booster_particle;
    public Transform[] wheelMesh;
    public Rigidbody rb;
    public LayerMask Road;

    public CinemachineVirtualCamera Cam1, Cam2, Cam3;
    //int ,float ,bool ,vector etc
    public int boostspeed;
    public int ActualSpeed;
    public int TurnTimeSpeed;
    public int turnpoint;
    public float movespeed = 25f;
    public float turnspeed = .8f;
    public float Turntimer_val = 1.6f;
    public float TurnFixed_val = .3f;
    public bool Turn;
    public bool Turntimer;
    public bool fixingTurn;
    [HideInInspector] public bool isStartGame;
    #endregion


    #region private field
    bool isGameOver = true;
    bool isBoostspeed;
    bool isSpeedSlow;
    bool isSpeedNormal;
    bool IsRaceFinished = false;

    #endregion






    //  bool mouseoveUI = false;
    //Script Reference;
    UImanager uimanager;
    // Start is called before the first frame update
    #region monobehaviour callback
  
    void Start()
    {
        uimanager = GameObject.Find("UIManager").GetComponent<UImanager>();
        Cam2.Priority = 9;
       
      
    }
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGameOver&&isStartGame)
        {
            Turn = !Turn;
            Turntimer = true;
            StartCoroutine(StopTurning());

        }
    }
    void FixedUpdate()
    {
       
       
        if (Turntimer)
        {
            if (turnpoint==1)
            {
                LeftTurn();
                RightTurn();
                //when car is turning then smoke particle will play
                Booster_particle[0].SetActive(false);
                Booster_particle[1].SetActive(false);
                tierSmoke_partilce[0].SetActive(true);
                tierSmoke_partilce[1].SetActive(true);
            }
        }
        else
        {   //when car is moving forward then smoke particle will not play
            tierSmoke_partilce[0].SetActive(false);
            tierSmoke_partilce[1].SetActive(false);
           
            wheelMesh[0].localRotation = Quaternion.Euler(0, 0, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, 0, 0);
            if (isGameOver&&!IsRaceFinished)
            {
                Booster_particle[0].SetActive(true);
                Booster_particle[1].SetActive(true);
                if (!isBoostspeed&&isSpeedNormal)
                {
                    movespeed = ActualSpeed;
                    rb.drag = 1.5f;
                }else if (isSpeedSlow)
                {
                  
                    movespeed = 150f;
                }

            }
            else if(!isGameOver)
            {
              
                Booster_particle[0].SetActive(false);
                Booster_particle[1].SetActive(false);
                tierSmoke_partilce[0].SetActive(false);
                tierSmoke_partilce[1].SetActive(false);
            }
           
        }
        if (isStartGame)
        {
            moveforward();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedSlow")
        {
            isSpeedSlow = true;
            isSpeedNormal = false;


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
        if (other.gameObject.tag == "TurnPoint")
        {
            if (turnpoint <= 0)
            {
                turnpoint++;
                uimanager.Lprogresscount += .1666f;
                uimanager.LevelProgress();
            }
        }
        if (other.gameObject.tag == "RaceComplete")
        {
            // movespeed = boostspeed;
            //  rb.DOMove(new Vector3(1200, transform.position.y, transform.position.z), 1f);
            Cam2.Priority = 11;
        }
        if (other.gameObject.tag == "RaceDone")
        {
            uimanager.RacePosition++;
            uimanager.Win();
        }
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            uimanager.Coincollected++;
            uimanager.Coinsound.Play();
        }
        if (other.gameObject.tag == "Diamond")
        {
            Destroy(other.gameObject);
            uimanager.DiamondCollect++;
            uimanager.Coinsound.Play();
        }
        if (other.gameObject.tag == "RaceFinished")
        {
            StartCoroutine(CameraChange());
            IsRaceFinished = true;
            movespeed = 0;
            turnspeed = 0f;

        }
        if (other.gameObject.tag == "Respawn")
        {
            Destroy(Followcam);
            Cam2.Priority = 11;

            isGameOver = false;
            rb.freezeRotation = false;
            rb.angularDrag = .05f;
            rb.drag = 0f;
            rb.mass = 2000f;
            movespeed = 0f;
            turnspeed = 0f;
            uimanager.GameOver();
            rb.velocity = rb.velocity / 2.5f;
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Respawn")
        //{
        //    movespeed = 0f;
        //    uimanager.GameOver();
        //}
    }
    #endregion

    #region private method
    IEnumerator speedBoost()
    {
        isBoostspeed = true;
        Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1f;
        Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 2f;
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
        Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        Cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
    }
    #region Turning And Movement
    void moveforward()
    {
        rb.AddRelativeForce(Vector3.forward * 1 * movespeed, ForceMode.Acceleration);

    }
    void LeftTurn()
    {
        if (Turn)
        {
            movespeed = TurnTimeSpeed;
            rb.drag = 3f;
            rb.AddRelativeTorque(Vector3.up * 1 * turnspeed, ForceMode.VelocityChange);
            wheelMesh[0].localRotation = Quaternion.Euler(0, 30, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, 30, 0);

        }

    }
    void RightTurn()
    {
        if (!Turn)
        {
            movespeed = TurnTimeSpeed;
            rb.drag = 3f;
            rb.AddRelativeTorque(Vector3.up * -1 * turnspeed, ForceMode.VelocityChange);
            wheelMesh[0].localRotation = Quaternion.Euler(0, -30, 0);
            wheelMesh[1].localRotation = Quaternion.Euler(0, -30, 0);
        }

    }
    IEnumerator FixedCarTurn()
    {

        while (fixingTurn)
        {

            this.rb.DORotate(new Vector3(0, 90, 0), 1f, RotateMode.Fast);
            movespeed = ActualSpeed;
            rb.drag = 1.5f;
            isSpeedSlow = false;
            break;
        }
        while (!fixingTurn)
        {
            this.rb.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.Fast);
            movespeed = ActualSpeed;
            rb.drag = 1.5f;
            isSpeedSlow = false;
            break;
        }
        yield return null;
    }
    IEnumerator StopTurning()
    {
        yield return new WaitForSeconds(Turntimer_val);
        turnpoint = 0;
        Turntimer = false;
        yield return new WaitForSeconds(TurnFixed_val);

        fixingTurn = !fixingTurn;
        StartCoroutine(FixedCarTurn());
    }
    #endregion

    IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(2f);
        Cam3.Priority = 12;
    }
    #endregion


    #region public method

    public void StartGame()
    {
        isStartGame = true;
        uimanager.StartButton.SetActive(false);
        StartCoroutine(speedBoost());
    }
    #endregion
}
