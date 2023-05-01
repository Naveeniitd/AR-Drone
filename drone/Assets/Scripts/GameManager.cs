using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class GameManager : MonoBehaviour
{
    public Button _flybutton;
    public Button _Landbutton;
    public droneController _droneController;
    public GameObject _Controls;
    public GameObject _drone;
 
    public ARRaycastManager _Raycastmanager;
    public ARPlaneManager _planemanager;
    List<ARRaycastHit> _HitResult = new List<ARRaycastHit>();
    // Start is called before the first frame update
    struct DroneAnimation
    {
        public bool moving;
        public bool interpolatiingAsc;
        public bool interpolationdesc;
        public float axis;
        public float direction;

    }
    DroneAnimation _movingLeft;
    DroneAnimation _movingback;

    void Start()
    {
        _flybutton.onClick.AddListener(eventonclickflybutton);
        _Landbutton.onClick.AddListener(eventonclicklandbutton);
    }

    // Update is called once per frame
    void Update()
    {
        /* float speedX = Input.GetAxis("Horizontal");
         float speedZ = Input.GetAxis("Vertical");*/
        UpdateControls(ref _movingLeft);
        UpdateControls(ref _movingback);
        _droneController.Move(_movingLeft.axis * _movingLeft.direction, _movingback.axis * _movingback.direction);
        if (_droneController.idIdle())
        {
            UpdateAr();
        }
    }
    void UpdateAr()
    {
        Vector2 positionScreenspace = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        _Raycastmanager.Raycast(positionScreenspace, _HitResult, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);
        if(_HitResult.Count > 0)
        {
            if(_planemanager.GetPlane(_HitResult[0].trackableId).alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                Pose pose = _HitResult[0].pose;
                _drone.transform.position = pose.position;
             
                _drone.SetActive(true);
            }
        }
    }
    void UpdateControls(ref DroneAnimation _controls)
    {
        if (_controls.moving || _controls.interpolatiingAsc || _controls.interpolationdesc)
        {
            if (_controls.interpolatiingAsc)
            {
                _controls.axis += 0.05f;


                if (_controls.axis >= 1.0f)
                {
                    _controls.axis = 1.0f;
                    _controls.interpolatiingAsc = false;
                    _controls.interpolationdesc = true;

                }
            }

            else if (!_controls.moving)
            {
                _controls.axis -= 0.05f;
                if (_controls.axis <= 0.0f)
                {
                    _controls.axis = 0.0f;
                    _controls.interpolationdesc = false;
                }
            }
        }
    }
    public void eventonclickflybutton()
    {
        if (_droneController.idIdle())
        {
            _droneController.Takeoff();
            _flybutton.gameObject.SetActive(false);
            _Controls.SetActive(true);
        }
    }
    public void eventonclicklandbutton()
    {
        if (_droneController.IsFlying())
        {
            _droneController.Land();
            _Landbutton.gameObject.SetActive(false);
            _Controls.SetActive(false);
        }

    }
    public void eventonrightbuttonpressed()
    {
        _movingLeft.moving = true;
        _movingLeft.interpolatiingAsc = true;
        _movingLeft.direction = 1.0f;
    }
    public void eventonrightbuttonreleased()
    {
        _movingLeft.moving = false;
    }
    public void eventonleftbuttonpressed()
    {
        _movingLeft.moving = true;
        _movingLeft.interpolatiingAsc = true;
        _movingLeft.direction = -1.0f;

    }
    public void eventonleftbuttonreleased()
    {
        _movingLeft.moving = false;

    }
    public void eventonupbuttonpressed()
    {
        _movingback.moving = true;
        _movingback.interpolatiingAsc = true;
        _movingback.direction = 1.0f;
    }
    public void eventonupbuttonreleased()
    {
        _movingback.moving = false;
    }
    public void eventondownbuttonpressed()
    {
        _movingback.moving = true;
        _movingback.interpolatiingAsc = true;
        _movingback.direction = -1.0f;
    }
    public void eventondownbuttonreleased()
    {
        _movingback.moving = false;
    }
    

}
