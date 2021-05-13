using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Declarando as variaveis
    public GameObject fishPrefabs;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    //Variaveis para configuracao do cardume
    [Header("Configuracoes do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    void Start()
    {
        //Criando a aquantidade de peixes que foi escolhida na variavel numFish
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++)
        {
            //Criando uma posicao randomica para spawn do peixe
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            //Instanciando na cena a quantidade de peixes que foi escolhida 
            allFish[i] = Instantiate(fishPrefabs, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //posicionamento que se encontra os peixes
        goalPos = this.transform.position;
    }
    private void Update()
    {
        //Ir para uma posicao randomica
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
    }
}
