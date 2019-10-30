using UnityEngine;

public class TileMaker : MonoBehaviour
{
    [SerializeField] int id = 0;

    public void ChooseTile()
    {
        FindObjectOfType<Map>().AddPoint(id);
    }
}
