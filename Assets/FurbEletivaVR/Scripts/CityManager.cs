
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

    private void Start()
    {
        StartCoroutine(StartLightningLoop());
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
