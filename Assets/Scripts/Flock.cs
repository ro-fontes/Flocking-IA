using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Declarando as variaveis
    public FlockManager myManager;
    float speed;
    bool turnig = false;

    void Start()
    {
        //Criando um valor randomico para a velocidade do peixe
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    void Update()
    {
        //limitando a distancia entre os peixes com o bounds
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        //criando um raycast
        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        //verificando se o peixe colidir 
        if (!b.Contains(transform.position))
        {
            turnig = true;
            direction = myManager.transform.position - transform.position;
        }
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turnig = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turnig = false;
        }

        if (turnig)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //calculando a velocidade para voltar ao eixo do grupo
            if(Random.Range(0,100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            if(Random.Range(0,100) < 20)
            {
                ApplyRules();
            }
        }
        //Direcao do peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //declarando as variaveis
        GameObject[] gos;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        //pegando todos os peixes que estao na cena
        gos = myManager.allFish;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                //Calculando a distancia de um peixe a outro
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //verificando se a distancia eh a mesma que esta salva no FlockManager
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    //adicionando um peixe ao grupo
                    groupSize++;

                    //Evitando a colisao entre os peixes se a distancia for menor que 1
                    if (nDistance < 1.0)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //verifica se o grupo de peixes for maior que 0 
        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                //fazendo uma rotacao do peixe de forma natural
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
