using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Puzzle : MonoBehaviour
{
    // enum PuzzleType
    // {
    //     Move,
    // }

    // [SerializeField] PuzzleType m_puzzleType;


    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            
            
            MoveObject(other.transform);
        }
    }

    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] float m_moveDuration;

    private void MoveObject(Transform targetObj)
    {
        Debug.Log("Aligned");
        targetObj.parent.GetComponent<Rigidbody>().isKinematic = true;
        targetObj.transform.position = transform.position + m_targetPosition;
    }
    
}
