using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Array der WheelColliders für die Räder des Autos
    public WheelCollider[] wheelsCol;

    // Array der Transform-Komponenten für die visuelle Darstellung der Räder
    public Transform[] wheelsMesh;

    // Parameter für Beschleunigung, Bremskraft und maximalen Lenkwinkel
    public float acceleration;
    public float breakingForce;
    public float maxTurningAngle;

    // Aktueller Wert der Beschleunigung
    private float currentAcceleration = 0f;

    // Aktueller Wert der Bremskraft
    private float currentBreakingForce = 0f;

    // Aktueller Lenkwinkel
    private float currentTurningAngle = 0f;

    // Start wird einmalig beim Start des Spiels aufgerufen
    void Start()
    {
        // Initialisieren der Parameter
        acceleration = 1000f; // Standardbeschleunigung
        breakingForce = 1000f; // Standardbremskraft
        maxTurningAngle = 40f; // Maximaler Lenkwinkel in Grad

        // Räderposition der Meshes an die der Colliders anpassen
        for (int i = 0; i < wheelsCol.Length; i++)
        {
            wheelsCol[i].transform.position = wheelsMesh[i].position;
        }
    }

    // FixedUpdate wird in einem festen Intervall aufgerufen und eignet sich für Physik-Berechnungen
    void FixedUpdate()
    {
        // Abfrage des vertikalen Inputs (z. B. W/S oder Pfeiltasten) für die Beschleunigung
        if (Input.GetAxis("Vertical") != 0)
        {
            currentAcceleration = acceleration * Input.GetAxis("Vertical");
        }
        else
        {
            currentAcceleration = 0f; // Keine Beschleunigung, wenn kein Input
        }

        // Abfrage des horizontalen Inputs (z. B. A/D oder Pfeiltasten) für den Lenkwinkel
        if (Input.GetAxis("Horizontal") != 0)
        {
            currentTurningAngle = maxTurningAngle * Input.GetAxis("Horizontal");
        }
        else
        {
            currentTurningAngle = 0; // Kein Lenkwinkel, wenn kein Input
        }

        // Lenkwinkel auf die vorderen Räder anwenden
        wheelsCol[0].steerAngle = currentTurningAngle;
        wheelsCol[1].steerAngle = currentTurningAngle;

        // Geringerer Lenkwinkel auf die hinteren Räder anwenden
        wheelsCol[2].steerAngle = -currentTurningAngle / 10f;
        wheelsCol[3].steerAngle = -currentTurningAngle / 10f;

        // Prüfen, ob die Bremstaste (Space) gedrückt ist
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = breakingForce; // Bremskraft anwenden
        }
        else
        {
            currentBreakingForce = 0f; // Keine Bremskraft
        }

        // Antriebskraft auf die vorderen Räder anwenden
        wheelsCol[0].motorTorque = currentAcceleration;
        wheelsCol[1].motorTorque = currentAcceleration;

        // Bremskraft auf alle Räder anwenden
        for (int i = 0; i < wheelsCol.Length; i++)
        {
            wheelsCol[i].brakeTorque = currentBreakingForce;
        }

        // Position und Rotation der Rad-Meshes aktualisieren
        for (int i = 0; i < wheelsMesh.Length; i++)
        {
            Vector3 wheelPosition; // Position des Rads
            Quaternion wheelRotation; // Rotation des Rads

            // Position und Rotation des Colliders abrufen
            wheelsCol[i].GetWorldPose(out wheelPosition, out wheelRotation);

            // Mesh-Position und -Rotation an die des Colliders anpassen
            wheelsMesh[i].position = wheelPosition;
            wheelsMesh[i].rotation = wheelRotation;
        }
    }
}
