using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gteem
{
    [GMonoBehaviourAttribute("SWIMMING SYSTEM", true)]
    public class SwimSystem : GCustomMonoBehaviour
    {

        #region Variabil
        public float speed = 3f;
        public float timeToDie = 20f;
        [Header("How Many the player will deep in water when start driving")]
        public float ToDeepInWater;
        public float SpeedTogiveOxygen = 2f;
        public GameObject UiOxygenBar;
        Slider OxygenBar;
        public KeyCode InputDivingMood;
        [System.Serializable]
        public class Effict
        {
            [Header("Partical Effict")]
            public GameObject Swimming;
            public GameObject Diving;
            public GameObject BloodScreen;
            [Header("Audio Effict")]
            public GameObject AudioMoveSwimming;
            public GameObject AudioDiving;
            public GameObject AudioStartSwimming;
            public GameObject AudioStartDiving;
            public GameObject AudioEndDiving;
            public GameObject AudioOxeBeEnded;

        }
        public Effict effict;

        float timer;

        private bool lockDieTime = true;
        private bool lockOutWater = true;
        private bool lockInput;
        private bool IsBack;
        Rigidbody2D rb;

        private bool fixinWater;
        private Vector3 inWater;
        private Vector3 PlayerPos;
        Color Bloodscreen;
        public UnityEngine.Events.UnityEvent onStartAction, onExitAction, OnDiving, Onswimming, OnIdelSwimming, onBeOutWater, onMoveSwimming, OnStartDiving, GameOver, OnEndDiving;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            OxygenBar = UiOxygenBar.GetComponent<Slider>();

            rb = gameObject.GetComponent<Rigidbody2D>();
            timer = timeToDie;
            OxygenBar.maxValue = timeToDie;
            OxygenBar.value = timeToDie;

        }

        #region InPut
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.tag == "OutWater" && Swimming.DivingMood == true)
            {

                transform.Rotate(transform.rotation.x, transform.rotation.y, 0);
                Swimming.SwimMood = true;
                Swimming.DivingMood = false;
                fixinWater = false;
                lockOutWater = true;


                Exit();

                OnEndDiving.Invoke();
                effict.AudioOxeBeEnded.SetActive(false);
            }


        }

        private void OnTriggerStay2D(Collider2D collision)
        {


            if (collision.tag == "Water" && Swimming.SwimMood == false && Swimming.DivingMood == false)
            {
                Swimming.SwimMood = true;
                Swimming.DivingMood = false;

                UiOxygenBar.SetActive(true);

                lockInput = true;
                onStartAction.Invoke();
                effict.AudioStartSwimming.SetActive(true);


            }





        }


        float timerExit = 0.5f;
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Water")
            {
                if (Swimming.DivingMood == false && Swimming.SwimMood == true)
                {
                    onBeOutWater.Invoke();
                    Exit();
                }
            }
        }

        #endregion
        // Update is called once per frame
        void Update()
        {

            if (Swimming.SwimMood == true)
            {

                fixinWater = false;

                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                Swimming.swimming(rb, speed);

                RegiveTime();
                Onswimming.Invoke();
                effict.AudioDiving.SetActive(false);
                effict.AudioOxeBeEnded.SetActive(false);
                effict.AudioStartDiving.SetActive(false);
                effict.Diving.SetActive(false);
                effict.Swimming.SetActive(true);
                effict.BloodScreen.SetActive(false);

                // print("Im swimming");
                if (Input.GetKey(InputDivingMood))
                {
                    Swimming.DivingMood = true;
                    Swimming.SwimMood = false;
                    inWater = new Vector3(transform.position.x, transform.position.y - ToDeepInWater, transform.position.z);
                    PlayerPos = transform.position;
                    gameObject.GetComponent<Animator>().SetBool("swimming", false);
                    OnStartDiving.Invoke();
                    effict.AudioStartDiving.SetActive(true);
                    effict.Swimming.SetActive(false);
                    effict.AudioMoveSwimming.SetActive(false);
                    effict.AudioStartSwimming.SetActive(false);
                }

                float x = rb.velocity.x;
                if (x != 0)
                {

                    onMoveSwimming.Invoke();
                    effict.AudioMoveSwimming.SetActive(true);
                }
                else
                {
                    OnIdelSwimming.Invoke();
                    effict.AudioMoveSwimming.SetActive(false);

                }

            }
            if (Swimming.DivingMood == true)
            {

                if (transform.position != inWater && fixinWater == false)
                {
                    transform.position = Vector3.Lerp(transform.position, inWater, 5 * Time.deltaTime);

                }
                if (transform.position.y < PlayerPos.y - ToDeepInWater + 1 && transform.position.y > PlayerPos.y - ToDeepInWater)
                {
                    fixinWater = true;

                }
                if (fixinWater == true)
                {
                    lockOutWater = false;
                    Swimming.Diving(rb, speed);
                    DriveTime();
                    //   print("im diving");
                    OnDiving.Invoke();
                    effict.AudioDiving.SetActive(true);
                    effict.AudioStartSwimming.SetActive(false);
                    effict.Diving.SetActive(true);

                }

                if (OxygenBar.value < timeToDie / 3)
                {
                    effict.AudioOxeBeEnded.SetActive(true);
                    effict.BloodScreen.SetActive(true);


                }
                effict.AudioMoveSwimming.SetActive(false);
            }


        }

        #region Diving Health Bar 
        private void DriveTime()
        {
            timer -= Time.deltaTime;
            OxygenBar.value = timer;

            if (timer == 0 || timer < 0)
            {

                GameOver.Invoke();

            }
        }
        private void RegiveTime()
        {
            timer += Time.deltaTime * SpeedTogiveOxygen;
            OxygenBar.value = timer;
            if (timer == timeToDie || timer > timeToDie)
            {
                timer = timeToDie;
            }
        }

        #endregion
        public void Exit()
        {


            Swimming.SwimMood = false;
            Swimming.DivingMood = false;

            effict.AudioStartSwimming.SetActive(false);
            effict.AudioMoveSwimming.SetActive(false);

            onExitAction.Invoke();
            lockInput = false;
            gameObject.GetComponent<Animator>().SetBool("IsSwim", false);
            gameObject.GetComponent<Animator>().SetBool("swimming", false);

            timerExit = 0.5f;
            if (OxygenBar.value != timeToDie)
            {
                RegiveTime();

            }
            else
            {
                UiOxygenBar.SetActive(false);
            }

        }


    }
}