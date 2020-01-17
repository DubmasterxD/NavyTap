using System.Collections;
using UnityEngine;

namespace NavyTap.Maps
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] Transform perfectScoreMax = null;
        [SerializeField] Transform perfectScoreMin = null;

        public int tileIndex = 0;

        int settingMaxPerfectScoreAnimatorLayer = 1;
        int settingMinPerfectScoreAnimatorLayer = 2;
        int _maxPerfectScoreSettingSpeedAnimatorFloat = Animator.StringToHash("MaxPerfectScoreSettingSpeed");
        int _minPerfectScoreSettingSpeedAnimatorFloat = Animator.StringToHash("MinPerfectScoreSettingSpeed");
        int _setPerfectScoreAnimatorState = Animator.StringToHash("SetPerfectScore");
        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            float maxPerfectScore = perfectScore + perfectScoreDifference;
            float minPerfectScore = perfectScore - perfectScoreDifference;
            anim.SetFloat(_maxPerfectScoreSettingSpeedAnimatorFloat, 1);
            anim.SetFloat(_minPerfectScoreSettingSpeedAnimatorFloat, 1);
            StartCoroutine(StopSettingScore(_maxPerfectScoreSettingSpeedAnimatorFloat, settingMaxPerfectScoreAnimatorLayer, GetTimeFromMaxScore(maxPerfectScore)));
            StartCoroutine(StopSettingScore(_minPerfectScoreSettingSpeedAnimatorFloat, settingMinPerfectScoreAnimatorLayer, GetTimeFromMinScore(minPerfectScore)));
        }

        private float GetTimeFromMaxScore(float maxScore)
        {
            return CalculateTimeFromScore(1 - maxScore);
        }

        private float GetTimeFromMinScore(float minScore)
        {
            return CalculateTimeFromScore(minScore);
        }

        private float CalculateTimeFromScore(float score)
        {
            return 0.5f + score / 2;
        }

        private IEnumerator StopSettingScore(int parameterAnimatorHash, int animatorLayer, float timeToStop)
        {
            yield return new WaitForSeconds(timeToStop - Time.fixedDeltaTime);
            anim.SetFloat(parameterAnimatorHash, 0);
            anim.Play(_setPerfectScoreAnimatorState, animatorLayer, timeToStop);
        }

        public void SpawnIndicator(Indicator toSpawn, float lifespan, float time)
        {
            if (spawnPoint != null)
            {
                Indicator indicator = Instantiate(toSpawn, spawnPoint);
                StartCoroutine(indicator.StartIndicatorIn(time, lifespan));
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Transform), GetType(), name));
            }
        }
    }
}