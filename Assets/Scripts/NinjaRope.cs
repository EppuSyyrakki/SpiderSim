using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaRope : MonoBehaviour
{
    private List<GameObject> _webParts = new List<GameObject>();

    [SerializeField] 
    private GameObject webPart;

    public Vector3 direction;
    private Vector3 _previousPosition;

    [SerializeField] 
    private float waitingTime = 0.05f;
    private float _timer = 0;

    [SerializeField] 
    private int maxParts = 10;

    private float _webHeight;

    void Start()
    {
        direction = transform.TransformDirection(transform.up);
        SpawnWeb(transform.position);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > waitingTime && _webParts.Count <= maxParts)
        {
            SpawnWeb(_previousPosition + direction);
        }
    }

    void SpawnWeb(Vector3 position)
    {
        GameObject piece = Instantiate(webPart, position, Quaternion.identity, transform);
        piece.transform.up = direction;
        _webHeight = piece.GetComponentInChildren<CapsuleCollider>().height;
        _previousPosition = piece.transform.position;
        _webParts.Add(piece);
    }
}
