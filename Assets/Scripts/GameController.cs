using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    //������ �� ������ �������� ������
    [SerializeField] public GameObject[] Enemys_Prefabs = new GameObject[5];

    //List(������������ ������) � ������ ������� (����������� � ���� ����� ����� � ������� ����, ��������� ��� ������)
    public List<GameObject> Enemys_Alive = new List<GameObject>();

    //������ �� ������ ������ ��� �������������� � ��� ��������
    [SerializeField] private Player _player;
    

    //������ �� ���������� ������������ ����
    private GameObject lastSpawnedEnemy;

    //���������� ���
    [SerializeField] private float tickTime;
    [SerializeField] private float tickModificator;
    private float tickValue;

    //������� ����� ��������� ���������� ����� ��� �������������� � ���
    RandomPoint randomPoint = new RandomPoint();

    //������ ����� ���������� ��������
    private int spaceClicks = 0;

    private void Awake()
    {
        //������ �������� ���������� �� ��������� ������� ������� �� ���-�� ����� �����
        StartCoroutine("StressTick");
    }
    private void Update()
    {
        //������ ����� ���������� ��������, �������
        if (Input.GetKeyDown("space"))
        {
            spaceClicks++;
            Spawn(Enemys_Prefabs[0], randomPoint.InSpawnZone(), Quaternion.identity);
        }
    }

    //����� ��� ������ �����. ��������� � ����: 1) ��� �����, 2) ����� ������, 3) ������� ������� �����
    public void Spawn(GameObject enemyType, Vector2 position, Quaternion rotation)
    {
        lastSpawnedEnemy = Instantiate(enemyType, position, rotation);

        //�������� ����� ���������� ��������(������ ������ 3�� ���� = ����, ������������� �� ����� �������
        if (spaceClicks == 3)
        {
            lastSpawnedEnemy.GetComponent<Enemy>().isGlitch = true;
            spaceClicks = 0;
        }
    }

    //�������� �������� (��� � tickTime �������) ������������� ������ � ����������� �� ���-�� ����� �����
    private IEnumerator StressTick()
    {
        if (Enemys_Alive.Count > 0)
        {
            if (Enemys_Alive.Count <= 2)
                tickValue = tickModificator;
            else
                tickValue = Mathf.Log(Enemys_Alive.Count) * tickModificator;
            _player.StressChange(tickValue);
        }
        yield return new WaitForSecondsRealtime(tickTime);
        Debug.Log("TICK " + tickValue);
        StartCoroutine("StressTick");
    }
    
}

//������� ���� ����� ��� �������� ��������� ���������� �����
public class RandomPoint
{

    private bool isCorrect = false;


    //� ����� ���� (���, �� ��������, ������)
    public Vector3 InSpawnZone()
    {

        Vector3 point;
        /*
        do
        {
            point = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-4.75f, 4.75f));
            if ((point.x <= 6.5 && point.x >= 5.5 || point.x <= -5.5 && point.x >= -6.5) && (point.y <= 4.75 && point.y >= 3.75 || point.y <= -3.75 && point.y >= -4.75))
                {
                    isCorrect = true;
                }     
        } while (!isCorrect);
        isCorrect = false;
        */
        do
        {
            point = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-5.75f, 5.75f));
            if (Vector2.Distance(point, Vector2.zero) >= 5.5 && Vector2.Distance(point, Vector2.zero) <= 6)
                isCorrect = true;
        } while (!isCorrect);
        isCorrect = false;
        return point;
        
    }

    //� ������� ���� (��� ����� ��������)
    public Vector3 InGameZone()
    {
        Vector3 point = new Vector2(Random.Range(-4.5f, 4.5f), Random.Range(-3f, 3f));
        return point;
    }
}
