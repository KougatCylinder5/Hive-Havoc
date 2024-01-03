using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    public GameObject bug;
    public Vector3 offset;
    private int currentWave = 1;
    public int waveSize;

    void Awake()
    {
        //After 60 seconds, spawn a bug every 5 seconds.
        InvokeRepeating(nameof(SpawnBug), 60, 5);
        //Every 100 seconds, spawn a wave.
        InvokeRepeating(nameof(SpawnWave), 100, 100);
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
        //Make a wave of bugs from the nest all at once, based on the difficulty of the save and the current wave number.
        for (int i = 0; i < currentWave * (DBAccess.getDiff() + 1) * waveSize; i++)
        {
            Vector3 offset2 = new Vector3(0, 0, 0.5f);
            Saver.allUnits.Add(Instantiate(bug, transform.position + offset + (i * offset2), Quaternion.identity));
        }
        currentWave++;
    }
}