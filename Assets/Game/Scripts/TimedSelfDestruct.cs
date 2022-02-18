using UnityEngine;

namespace Game.Scripts
{
    public class TimedSelfDestruct : MonoBehaviour
    {
        public float lifeTime = 1f;

        float _mSpawnTime;

        void Awake()
        {
            _mSpawnTime = Time.time;
        }

        void Update()
        {
            if (Time.time > _mSpawnTime + lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}