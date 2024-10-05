using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToFollow;

    private void Update() {
        transform.position = gameObjectToFollow.transform.position;
    }
}
