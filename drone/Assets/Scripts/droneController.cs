using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneController : MonoBehaviour
{
    
    enum DroneState
    {
        DRONESTATE_IDLE,
        DRONESTATE_START_TAKINGOFF,
        DRONESTATE_TAKINGOFF,
        DRONESTATE_MOVINGUP,
        DRONESTATE_FLYING,
        DRONESTATE_START_LANDING,
        DRONESTATE_WAIT_ENGLINE_STOP,
        DRONESTATE_LANDED,
        DRONESTATE_LANDING


    }
    Animator _Anim;
    Vector3 _speed = new Vector3(0.0f, 0.0f, 0.0f);
    DroneState _state;

    public float speedMultiplyer;
    // Start is called before the first frame update
    public bool idIdle()
    {
        return (_state == DroneState.DRONESTATE_IDLE);
    }
    public void Takeoff()
    {
        _state = DroneState.DRONESTATE_START_TAKINGOFF;
    }
    public bool IsFlying()
    {
        return (_state == DroneState.DRONESTATE_FLYING);
    }
    public void Land()
    {
        _state = DroneState.DRONESTATE_START_LANDING;
    }
    void Start()
    {
        _Anim = GetComponent<Animator>();
    
        _state = DroneState.DRONESTATE_IDLE;


    }

    public void Move(float _speedX, float _speedZ)
    {
        _speed.x = _speedX;
        _speed.z = _speedZ;
        UpdateDrone();
        
    }
    // Update is called once per frame
    void UpdateDrone()
    {   
        switch (_state){
            case DroneState.DRONESTATE_IDLE:
                break;
            case DroneState.DRONESTATE_START_TAKINGOFF:
                _Anim.SetBool("TakeOff", true);
                _state = DroneState.DRONESTATE_TAKINGOFF;
                break;
            case DroneState.DRONESTATE_TAKINGOFF:
                if(_Anim.GetBool("TakeOff") == false)
                {
                    _state = DroneState.DRONESTATE_MOVINGUP;

                }
                break;
            case DroneState.DRONESTATE_MOVINGUP:
                if (_Anim.GetBool("MoveUp") == false)
                {
                    _state = DroneState.DRONESTATE_FLYING;
                }
                break;
            case DroneState.DRONESTATE_FLYING:
                float anglez = -30.0f * _speed.x * 60.0f * Time.deltaTime;
                float anglex = 30.0f * _speed.z * 60.0f * Time.deltaTime;
                Vector3 rotation = transform.localRotation.eulerAngles;

                transform.localPosition += _speed * speedMultiplyer * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(anglex, rotation.y, anglez);
                
                break;
            case DroneState.DRONESTATE_START_LANDING:
                _Anim.SetBool("MoveDown", true);
                _state = DroneState.DRONESTATE_LANDED;
                break;
            case DroneState.DRONESTATE_LANDING:
                if(_Anim.GetBool("MoveDown")== false)
                {
                    _state = DroneState.DRONESTATE_LANDED;

                }
                break;
            case DroneState.DRONESTATE_LANDED:
                _Anim.SetBool("Land", true);
                _state = DroneState.DRONESTATE_WAIT_ENGLINE_STOP;
                break;
            case DroneState.DRONESTATE_WAIT_ENGLINE_STOP:
                if (_Anim.GetBool("Land") == false)
                {
                    _state = DroneState.DRONESTATE_IDLE;
                }
                break;
        }
        
    }
}
