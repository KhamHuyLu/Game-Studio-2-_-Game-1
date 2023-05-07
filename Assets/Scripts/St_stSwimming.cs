using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gteem
{
    public class Swimming
    {
        static Quaternion n;
        public static bool SwimMood { get; set; }
        public static bool DivingMood { get; set; }
        public static float OxeBar { get; set; }
        public static void swimming(Rigidbody2D rigidbody, float speed)
        {

            Animator animator = rigidbody.GetComponent<Animator>();
            Transform Player = rigidbody.gameObject.GetComponent<Transform>();

            animator.SetBool("IsSwim", true);
            // rigidbody.constraints = RigidbodyConstraints2D.None;
            float x = Input.GetAxisRaw("Horizontal");

            rigidbody.velocity = new Vector3(x * speed, rigidbody.velocity.y);

            if (x > 0)
            {
                rigidbody.transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("swimming", true);
                animator.SetBool("diving", false);
            }
            else if (x < 0)
            {
                rigidbody.transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool("swimming", true);
                animator.SetBool("diving", false);
            }
            if (x == 0)
            {
                animator.SetBool("swimming", false);
                animator.SetBool("diving", false);
            }



        }

        public static void Diving(Rigidbody2D rigidbody, float speed)
        {
            Animator animator = rigidbody.GetComponent<Animator>();
            Transform Player = rigidbody.gameObject.GetComponent<Transform>();
            rigidbody.constraints = RigidbodyConstraints2D.None;
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            rigidbody.velocity = new Vector3(x * speed, y * speed);

            float OffSet;
            if (y == 0 && x == 0)
            {
                OffSet = 0;
                animator.SetBool("diving", false);
            }
            else
            {
                OffSet = -90;
                animator.SetBool("diving", true);
            }
            if (x > 0)
            {
                rigidbody.transform.localScale = new Vector3(1, 1, 1);

            }
            else if (x < 0)
            {
                rigidbody.transform.localScale = new Vector3(-1, 1, 1);

            }


            float Zrotation = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            n = Quaternion.Euler(n.x, n.y, Zrotation + OffSet);
            Player.rotation = Quaternion.Lerp(Player.rotation, n, 3 * Time.deltaTime);
        }

        public static void RegiveTimeDiving(float timer,float timeToEndOxe ,float SpeedToRegiveOxe)
        {
            timer += Time.deltaTime * SpeedToRegiveOxe;
            OxeBar = timer;
            if (timer == timeToEndOxe || timer > timeToEndOxe)
            {
                timer = timeToEndOxe;
            }
        }
    }

}