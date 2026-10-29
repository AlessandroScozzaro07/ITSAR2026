using Unity.VisualScripting;
using UnityEngine;

public class LifeCycleLogger : MonoBehaviour
{
    // awake è eseugito solo alla prima attivazione dell'oggetto
     void Awake()
    {
        Debug.Log($" Eseguo Awake {gameObject.name}");
    }
    //onenable è eseguito a ogni riattivazione dell'oggetto
     void OnEnable()
    {
        Debug.Log($" Eseguo OnEnalbe {gameObject.name}");
    }
    // start è eseguito solo al primo avvio della scena
    void Start()
    {
        Debug.Log($" Eseguo Start {gameObject.name}");

    }
    //Update è chiamato a ogni frase
    void Update()
    {
        Debug.Log($" Eseguo Update {gameObject.name}");
    }
    //fixupdate è chiamato a intervalli fissi mentre l'oggetto è attivo(calcoli fisici)
    private void FixedUpdate()
    {
        Debug.Log($" Eseguo FixUpdate {gameObject.name}");
    }
    // Lateupdate è chiamato al termine del ciclo updtae/fixupdate come ultimo step del ciclo
    private void LateUpdate()
    {
        Debug.Log($" Eseguo LateUpdate {gameObject.name}");
    }
    //Ondisable è chiamamto alla disattivazione dell'oggetto
    private void OnDisable()
    {
        Debug.Log($" Eseguo OnDisable {gameObject.name}");
    }
    //OnDestroy è chiamato alla distruzione dell'oggetto
    private void OnDestroy()
    {
        Debug.Log($" Eseguo OnDestroy {gameObject.name}");
    }
}
