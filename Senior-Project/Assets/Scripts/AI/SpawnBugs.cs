using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    public GameObject bug;
    public Vector3 offset;
    private int currentWave = 1;
    public int waveSize;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(SpawnBug), 30, 5);
        InvokeRepeating(nameof(SpawnWave), 100, 100);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnBug()
    {
        if (FlowFieldGenerator.FlowFieldFinished)
        {
            Saver.allUnits.Add(Instantiate(bug, transform.position + offset, Quaternion.identity));
        }   
    }

    public void SpawnWave()
    {
        for (int i = 0; i < currentWave * (DBAccess.getDiff() + 1) * waveSize; i++)
        {
            Vector3 offset2 = new Vector3(0, 0, 0.5f);
            Saver.allUnits.Add(Instantiate(bug, transform.position + offset + (i * offset2), Quaternion.identity));
        }
        currentWave++;
    }
}
