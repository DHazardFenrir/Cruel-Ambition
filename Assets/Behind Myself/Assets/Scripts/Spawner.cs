    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject enemyObject;
        private BoxCollider2D mycollider;
        [SerializeField] float radius;
        private bool isOcuppied = false;
        [SerializeField] int enemiesPerPoint = 4;
        private bool isSpawning = false;
        private float minX, maxX;
        private float minY, maxY;

   
        void Awake()
        {
            mycollider = GetComponent<BoxCollider2D>();
        }

      

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isSpawning)
            {
                StartCoroutine(CheckToSpawn());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StopAllCoroutines();
                isSpawning = false;
            }
        }

        private IEnumerator CheckToSpawn()
        {
            isSpawning = true;

            minX = mycollider.bounds.min.x;
            minY = mycollider.bounds.min.y;
            maxX = mycollider.bounds.max.x;
            maxY = mycollider.bounds.max.y;
            float randomPosX;
            float randomPosY;
            randomPosX = Random.Range(minX, maxX);
            randomPosY = Random.Range(minY, maxY);
            Vector3 point = new Vector3(randomPosX, randomPosY, 0);
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                       point, radius
                   );
            isOcuppied = false;
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
                {
                    Debug.Log("Pos occupied");
                    isOcuppied = true;
                    break;
                }


            }
            for (int i = 0; i < enemiesPerPoint; i++)
            {

                point += new Vector3(0.5f, i, 0);

                if (!isOcuppied)
                {
                    Instantiate(enemyObject, point, enemyObject.transform.rotation);

                }


            }
            yield return new WaitForSeconds(.5f);

        }
    }

