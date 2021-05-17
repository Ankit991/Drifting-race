using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider[] wheelcollider;
    public Transform[] Meshwheel;
    public float MotorFoce;
    public float RightSteeringForce, LeftSteeringForce;
     
    public bool Getrot;
    public bool rot;
    // Start is called before the first frame update
    void Start()
    {
      // StartCoroutine(SpeedBoost());
      
    }
    IEnumerator SpeedBoost()
    {
        MotorFoce = 1000f;
        yield return new WaitForSeconds(3f);
        MotorFoce = 400f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        WheelUpdate();
        if (Input.GetMouseButtonDown(0))
        {
            Getrot = !Getrot;
            rot = true;
            StartCoroutine(CarRotationStop());
        }
        if (rot)
        {
            if (Getrot)
            {
               CarRotRight();
              
            }
            if (!Getrot)
            {
                CarRotLeft();
              
            }
        }
        else
        {
            CarNotRot();
            wheelcollider[2].motorTorque = MotorFoce;
            wheelcollider[3].motorTorque = MotorFoce;
        }
    }
    #region Rotation of Car
    void CarRotLeft()
    {
        wheelcollider[0].steerAngle = LeftSteeringForce;
        wheelcollider[1].steerAngle = LeftSteeringForce;
        SidewayFriction(.9f);
    }
    void CarRotRight()
    {
        wheelcollider[0].steerAngle = RightSteeringForce;
        wheelcollider[1].steerAngle = RightSteeringForce;
        SidewayFriction(.9f);
    }
    void CarNotRot()
    {
        wheelcollider[0].steerAngle = 0;
        wheelcollider[1].steerAngle = 0;
      
        CarBreak(0);
    }
    IEnumerator CarRotationStop()
    {
       
        yield return new WaitForSeconds(1f);
       
        rot = false;
        yield return new WaitForSeconds(.5f);
        if (Getrot)
        {
            this.transform.localRotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
        }
        else
        {
            this.transform.localRotation = Quaternion.Euler(transform.rotation.x, 0,transform.rotation.z);
        }
       
    }
    void ForwardFriction()
    {
       
    }
    void SidewayFriction(float val)
    {
        WheelFrictionCurve WFC = wheelcollider[2].sidewaysFriction;
        WheelFrictionCurve WFC1= wheelcollider[3].sidewaysFriction;
        WFC.stiffness = val;
         WFC.stiffness = val;
    }
    #endregion
    void CarBreak(float breakforce)
    {
        wheelcollider[2].brakeTorque = breakforce;
        wheelcollider[3].brakeTorque = breakforce;
    }
    #region carWheelRotation
    private void Engineforce(float motor, float Steering)
    {

        wheelcollider[2].motorTorque = motor;
        wheelcollider[3].motorTorque = motor;


        wheelcollider[0].steerAngle = Steering;
        wheelcollider[1].steerAngle = Steering;

    }
    private void WheelUpdate()
    {
        LocalRotationofWheelmesh(wheelcollider[0], Meshwheel[0]);
        LocalRotationofWheelmesh(wheelcollider[1], Meshwheel[1]);
        LocalRotationofWheelmesh(wheelcollider[2], Meshwheel[2]);
        LocalRotationofWheelmesh(wheelcollider[3], Meshwheel[3]);
    }
    private void LocalRotationofWheelmesh(WheelCollider wheelcollider, Transform meshwheel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelcollider.GetWorldPose(out pos, out rot);
        meshwheel.rotation = rot;
        meshwheel.position = pos;
    }
    #endregion 
}
