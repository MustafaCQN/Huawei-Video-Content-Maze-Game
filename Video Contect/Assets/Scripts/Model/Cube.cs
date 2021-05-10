using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform cube;
    [SerializeField]
    private Vector3 position;

    private float animTime = 2;

    private void Awake()
    {
        position = cube.localPosition;
        cube.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        cube.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        cube.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void DoPosition()
    {
        cube.gameObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(_DoPosition());
    }

    private IEnumerator _DoPosition()
    {
        Vector3 startPos = new Vector3(position.x * 50, position.y * 50, 0);

        cube.localPosition = startPos;

        cube.gameObject.SetActive(true);
        Vector3 finalPos = position;

        float timer = 0;
        while (timer <= animTime)
        {
            timer += Time.deltaTime;
            var posTemp = Vector3.Lerp(startPos, finalPos, timer / animTime);
            cube.localPosition = posTemp;
            yield return null;
        }
    }
}
