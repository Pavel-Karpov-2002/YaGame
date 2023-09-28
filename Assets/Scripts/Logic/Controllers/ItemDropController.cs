using System.Collections;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField] private float _maxRangeDrop;
    [SerializeField] private float _minRangeDrop;
    [SerializeField] private float _timeForStopKinematic;

    public void DropItems(Item item, int minCountDrops, int maxCountDrops)
    {
        for (int i = 0; i < Random.Range(minCountDrops, maxCountDrops + 1); i++)
        {
            Item itemObj = Instantiate(item, transform.position, transform.rotation);
            Vector3 randomPosition = new Vector3(
                Random.Range(_minRangeDrop, _maxRangeDrop),
                Random.Range(_minRangeDrop, _maxRangeDrop),
                Random.Range(_minRangeDrop, _maxRangeDrop)
            );
            StartCoroutine(ChangeKinematic(itemObj.Rigidbody, randomPosition));
        }
    }

    private IEnumerator ChangeKinematic(Rigidbody itemRb, Vector3 randomPosition)
    {
        itemRb.AddForce(randomPosition, ForceMode.Impulse);
        yield return new WaitForSeconds(_timeForStopKinematic);

        itemRb.isKinematic = true;
    }
}
