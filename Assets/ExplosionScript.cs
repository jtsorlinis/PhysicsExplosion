using UnityEngine;
using UnityEngine.UI;

public class ExplosionScript : MonoBehaviour {

    public GameObject InfoCanvas;
    public GameObject InfoText;
	public Vector3 Velocity;
    public float Mass;
	public int NoOfPieces;
    public float ExplosionForce;
    public float ExplosionDelay;
    public float gravity;
    float scale;

    GameObject[] Pieces;
    Vector3 rot;
    float ExplosionLength = 1;
    bool IsChild;
    Vector3 parentVelocity;
    GameObject info;

    // Use this for initialization
    void Start () {

        // Set the scale of each cube based on its mass
        scale = Mathf.Pow(Mass, 1f / 3f);
        transform.localScale = new Vector3(scale, scale, scale);

        // Give a random rotation/direction to each of the cubes
        transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		Vector3 RadRot = rot * Mathf.Deg2Rad;

        // If it is a child cube, simulate it, otherwise explode the original
        if(IsChild)
        {

            // Calculate the vectors for each direction
            Velocity.x = Mathf.Cos(RadRot.z) * Mathf.Cos(RadRot.y) * ExplosionForce / Mass / NoOfPieces;
            Velocity.y = Mathf.Sin(RadRot.z) * Mathf.Cos(RadRot.y) * ExplosionForce / Mass / NoOfPieces;
            Velocity.z = Mathf.Sin(RadRot.y) * ExplosionForce / Mass / NoOfPieces;
		}
        else
        {
            // Call the explosion function with a delay
            Invoke("Explode", ExplosionDelay);
        }

    }
		
	
	// Update is called once per frame
	void Update () {
        if(IsChild)
        {
            // Add gravity to the y vector of this cube
            Velocity.y -= gravity * Time.deltaTime;
        }

        // Move this cube by the velocity vector, and also inherit the parent's velocity
        transform.position += (Velocity + parentVelocity) * Time.deltaTime;
	}


    // Explode function
    void Explode()
    {
        Pieces = new GameObject[NoOfPieces];

        // For each piece
        for (int i = 0; i < Pieces.Length; i++)
        {
            // Create the fragment and get a reference to it
            Pieces[i] = Instantiate(this.gameObject);
            ExplosionScript PieceScript = Pieces[i].GetComponent<ExplosionScript>();

            // Set its mass
            PieceScript.Mass = Mass / NoOfPieces;

            // Set it to a fragment
            PieceScript.IsChild = true;

            // Set its rotation to a random vector
            PieceScript.rot = new Vector3(0, Random.Range(0, 359), Random.Range(0, 359));

            // Save the parent's velocity in this child's variable variable
            PieceScript.parentVelocity = Velocity;
		}

        // Destroy the original cube
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
		float VelCalc = Mathf.Sqrt(Mathf.Pow(Velocity.x, 2) + Mathf.Pow(Velocity.y, 2) + Mathf.Pow(Velocity.z, 2));
		float KineticEnergy = .5f * Mass * Mathf.Pow(VelCalc, 2);
        info = Instantiate(InfoText, Camera.main.WorldToScreenPoint(transform.position)+new Vector3(0,50,0), Quaternion.identity, InfoCanvas.transform);
        info.GetComponent<Text>().text = "Mass:" + Mass + " KE:" + KineticEnergy + "\nVel x:" + Velocity.x + " y:" + Velocity.y + " z:" + Velocity.z;
    }

    private void OnMouseExit()
    {
        Destroy(info);
    }
}
