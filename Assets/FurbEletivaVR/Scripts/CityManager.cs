
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityManager : MonoBehaviour
{
    public Transform PlayerTransform;

    [Header("Lightning")]
    public GameObject LightningPrefab;
    public float MinLightningSecondsInterval = 1.5f;
    public float MaxLightningSecondsInterval = 2.5f;

    public float MinLightningDistance = 5f;
    public float MaxLightningDistance = 20f;

    [Header("Water")]
    public Transform WaterTransform;
    public float SecondsToMaxWater = 120.0f;
    public float MinWaterHeight = -0.1f;
    public float MaxWaterHeight = 1.5f;

    private float elapsedSeconds = 0f;

    private void Start()
    {
        StartCoroutine(StartLightningLoop());
    }

    private void Update()
    {
        elapsedSeconds += Time.deltaTime;

        if (elapsedSeconds > SecondsToMaxWater)
            elapsedSeconds = SecondsToMaxWater;

        var tempPos = WaterTransform.position;
        tempPos.y = Mathf.Lerp(MinWaterHeight, MaxWaterHeight, elapsedSeconds / SecondsToMaxWater);
        WaterTransform.position = tempPos;
    }

    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    private IEnumerator StartLightningLoop()
    {
        while (true)
        {
            float lightningSecondsInterval = Random.Range(MinLightningSecondsInterval, MaxLightningSecondsInterval);

            yield return new WaitForSeconds(lightningSecondsInterval);

            Vector3 pos = RandomInCircle(PlayerTransform.position, Random.Range(MinLightningDistance, MaxLightningDistance));
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, PlayerTransform.position - pos);
            Instantiate(LightningPrefab, pos, rot);
        }
    }

    private Vector3 RandomInCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
