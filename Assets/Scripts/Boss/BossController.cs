using System.Collections;
using TouhouJam.Manager;
using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _bulletPrefab;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            StartCoroutine(LaunchPattern());
        }

        private IEnumerator LaunchPattern()
        {
            var patterns = new System.Func<IEnumerator>[]
            {
                PatternCircle,
                PatternWave,
                PatternArrow
            };

            while (true)
            {
                while (!GameManager.Instance.CanMove)
                {
                    yield return new WaitForSeconds(.1f);
                }
                yield return patterns[Random.Range(0, patterns.Length)]();
                yield return new WaitForSeconds(1f);
            }
        }

        private void CreateBullet(float a)
        {
            var inst = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            inst.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * 10f;
        }

        private IEnumerator PatternCircle()
        {
            for (float a = 0f; a < 2 * Mathf.PI; a += .3f)
            {
                CreateBullet(a);
                yield return new WaitForSeconds(.05f);
            }
        }

        private IEnumerator PatternWave()
        {
            for (float i = 0f; i < 10; i += .5f)
            {
                var dir = PlayerController.Instance.transform.position - transform.position;
                var a = Mathf.Atan2(dir.y, dir.x) + Mathf.Cos(i);
                CreateBullet(a);
                yield return new WaitForSeconds(.1f);
            }
        }

        private IEnumerator PatternArrow()
        {
            for (float i = .01f; i < .5f; i += .1f)
            {
                var dir = PlayerController.Instance.transform.position - transform.position;
                {
                    var a = Mathf.Atan2(dir.y, dir.x);
                    CreateBullet(a + i);
                    CreateBullet(a - i);
                }
                yield return new WaitForSeconds(.25f);
            }
        }
    }
}
