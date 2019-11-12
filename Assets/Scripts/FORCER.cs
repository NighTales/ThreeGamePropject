using System.Collections;
using System.Collections.Generic;
using Lessons.SilaArtema;
using UnityEngine;
using UnityEngine.UI;

public class FORCER : MonoBehaviour
{
    public GameObject ForceZone;
    public RaycastHit hit;
    public Transform offset;
    public float grabPower = 10.0f;
    public float throwPower = 10.0f;
    public float RayDistance = 13.0f;
    public int ForceType;

    private Animator _anim;
    private NewRelictusController NRC;
    private float _forceTime;
    private bool Grab = false; // взять
    private bool Throw = false; // кинуть
    public bool hitRb = false;
    private ForceReaction _forceReaction;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        ForceZone.SetActive(false);
        NRC = GetComponent<NewRelictusController>();
        _forceTime = 1;
        
    }

    void Update()
    {
        if (NRC.Energy.value >= 15)
        {
            q = Input.GetKeyDown(KeyCode.Q);
            e = Input.GetKeyDown(KeyCode.E);
            r = Input.GetKeyDown(KeyCode.R);
            f = Input.GetKeyDown(KeyCode.F);
        }
    }
    bool q, e, r, f;
    void FixedUpdate()
    {

        Timer();
        if(NRC.Energy.value > 15)
        {
            if (r)
            {
                if (hitRb == false)
                {//
                    if (Physics.Raycast(transform.position, transform.forward, out hit, RayDistance, (1 << 10)))
                    {
                        hitRb = true;
                        _forceReaction = hit.rigidbody.GetComponent<ForceReaction>();
                    }
                }

                if (hit.rigidbody)
                {
                    Grab = true;
                    _anim.SetBool("UseForce", true);
                }
            }
            if (f && hitRb)
            {
                Grab = false;
                Throw = true;
                _anim.SetBool("UseForce", false);
                _anim.SetInteger("Force", 1);
            }

            if (e)
            {
                ForceType = 1;
                ForceZone.SetActive(true);
                _forceTime = 0;
                _anim.SetInteger("Force", 1);
                _anim.SetTrigger("ForceStart");
            }
            if (q)
            {
                ForceType = -1;
                ForceZone.SetActive(true);
                _forceTime = 0;
                _anim.SetInteger("Force", -1);
                _anim.SetTrigger("ForceStart");
            }
        }
        

        if (Grab)
        {
            if (hit.rigidbody)
            {
                NRC.Energy.value -= 0.1f;
                NRC.EnergyTime = 0;
                hit.rigidbody.velocity =
                    (offset.position - (hit.transform.position + hit.rigidbody.centerOfMass)) * grabPower;

                _forceReaction.BoostAvailable.enabled = true;
            }
        }

        if(NRC.Energy.value == 0)
        {
            Grab = false;
            Throw = false;
            Invoke("Poff", 0.5f);
            _anim.SetBool("UseForce", false);
            _anim.SetInteger("Force", 0);
            hitRb = false;
        }

        if (Throw)
        {
            if (hit.rigidbody)
            {
                NRC.Energy.value -= 3;
                hit.rigidbody.velocity = transform.forward * throwPower;
                Throw = false;
                hitRb = false;
                Invoke("Poff", 0.5f);
            }
        }
    }

    void Poff()
    {
        _forceReaction.BoostAvailable.enabled = false;
    }

    private void Timer()
    {
        if(_forceTime < 0.5)
        {
            NRC.Energy.value -= 0.5f;
            NRC.EnergyTime = 0;
            _forceTime += Time.fixedDeltaTime;
        }
        else
        {
            ForceZone.SetActive(false);
        }
    }
}