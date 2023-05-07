using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gteem
{
    public class Move : MonoBehaviour
    {
        public float speed;
        public float jump;
        public KeyCode KeyJump;

        bool lockCode;
        bool lockJump;


        void Update()
        {
            if (lockCode == false)
            {
                float x = Input.GetAxisRaw("Horizontal");

                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                if (x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);

                }
                else if (x < 0)
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (x != 0)
                {
                    gameObject.GetComponent<Animator>().SetBool("run", true);
                }
                else
                {
                    gameObject.GetComponent<Animator>().SetBool("run", false);
                }
            }
            if (lockJump == false)
            {
                if (Input.GetKeyDown(KeyJump))
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.up * jump;
                    gameObject.GetComponent<Animator>().SetTrigger("jump");



                }
            }
        }

        /// <summary>
        /// In eveint system
        /// </summary>
        public void LockCode()
        {
            lockCode = true;
        }
        public void UnLockCode()
        {
            lockCode = false;
        }
        public void LockJump()
        {
            lockJump = true;
        }
        public void UnLockJump()
        {
            lockJump = false;
        }
    }
}
