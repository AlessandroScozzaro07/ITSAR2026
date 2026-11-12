using Unity.Collections;
using UnityEngine;

public class PlayerContrroller_RigidBody : MonoBehaviour
{
    private Rigidbody rb;  //sia serializedfield che senza
    
    private Vector2 input;// variabile locale perche nell altro script impartiamo l input nin un movimento e viene usata per tuto
    
    private GameControls controls;
    
    [SerializeField]private float force = 50f;
    public void Awake()// metodo di game object. 
    {
        rb = GetComponent<Rigidbody>();// assegna un valore alla variabile dello stesso tipo pescando il 1 component dello stesso tipo disponibile nella hierarchy
        controls = new GameControls();
    }
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    private void OnDestroy() => controls.Dispose();

    private void Update() // ogni volta leggo
    {
        input = controls.Player.Move.ReadValue<Vector2>();
        Debug.Log(input);
    }
    private void FixedUpdate()
    {
        Vector3 input3= new Vector3(input.x, 0f, input.y);
        Vector3 velocity = input3 * force;
        //rb.AddForce(input * force);
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // movepoition con rigidbody kinematic
    }
}
