using System.Collections;
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

            StartCoroutine(LaunchPattern());
        }

        private IEnumerator LaunchPattern()
        {
            while (true)
            {
                yield return PatternCircle();
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator PatternCircle()
        {
            for (float a = 0f; a < 2 * Mathf.PI; a += .5f)
            {
                var inst = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * 10f;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
