using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class EditBuckets : MonoBehaviour
    {

        [SerializeField] private List<GameObject> childs;
        [SerializeField] private Vector3 bucketAddPos;
        [SerializeField] private Vector3 collectedStickScale;

        private void Update()
        {
            StartCoroutine(EditBucketRot());
            StartCoroutine(EditBucketPos());
        }
        void AddChildToList()
        {
            // CLEAR LIST //
            childs.Clear();
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    childs.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        IEnumerator EditBucketPos()
        {
            if (transform.childCount > 1)
            {
                transform.GetChild(0).DOMove(transform.position, .25f);
                
                int count = transform.childCount;
                for (int i = 1; i < count; i++)
                {
                    transform.GetChild(i).transform.DOMove(transform.GetChild(0).position + (bucketAddPos * i), .25f);
                }
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator EditBucketRot()
        {
            AddChildToList();
            foreach (GameObject go in childs )
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(go.transform.DORotate(
                        new Vector3(transform.parent.rotation.x, transform.parent.rotation.y,
                            transform.parent.rotation.z),
                        .25f))
                    .Append(go.transform.DOScale(collectedStickScale, .25f));

            }
            yield return new WaitForSeconds(1);
        }
    }
}