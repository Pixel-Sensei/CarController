using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController2 : MonoBehaviour
{
    // Referenz zum GUIController, um Timer und Bestzeiten zu verwalten
    public GUIController guiController;

    // Arrays für Rad-Colliders und deren zugehörige Meshes (Grafiken)
    public WheelCollider[] wheelsCol;
    public Transform[] wheelsMesh;

    // Parameter für Beschleunigung, Bremskraft und maximale Lenkwinkel
    public float acceleration;
    public float breakingForce;
    public float maxTurningAngle;

    // Aktuelle Werte für Beschleunigung, Bremskraft und Lenkwinkel
    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurningAngle = 0f;

    // Start wird einmalig beim Start des Spiels aufgerufen
    void Start()
    {
        // Initialisierung der Fahrzeugparameter
        acceleration = 1000f;
        breakingForce = 1000f;
        maxTurningAngle = 40f;

        // Synchronisiere die Position der Collider mit den Meshes
        for (int i = 0; i < wheelsCol.Length; i++)
        {
            wheelsCol[i].transform.position = wheelsMesh[i].position;
        }
    }

    // FixedUpdate wird in gleichmäßigen Zeitintervallen aufgerufen (ideal für Physik-Berechnungen)
    void FixedUpdate()
    {
        // Überprüfe die Eingaben für Beschleunigung (vertikale Achse)
        if (Input.GetAxis("Vertical") != 0)
        {
            currentAcceleration = acceleration * Input.GetAxis("Vertical");
        }
        else
        {
            currentAcceleration = 0f;
        }

        // Überprüfe die Eingaben für die Lenkung (horizontale Achse)
        if (Input.GetAxis("Horizontal") != 0)
        {
            currentTurningAngle = maxTurningAngle * Input.GetAxis("Horizontal");
        }
        else
        {
            currentTurningAngle = 0;
        }

        // Lenkwinkel auf die vorderen Räder anwenden
        wheelsCol[0].steerAngle = currentTurningAngle;
        wheelsCol[1].steerAngle = currentTurningAngle;

        // Geringere Gegenlenkung auf die hinteren Räder anwenden
        wheelsCol[2].steerAngle = -currentTurningAngle / 10f;
        wheelsCol[3].steerAngle = -currentTurningAngle / 10f;

        // Überprüfe, ob die Bremse (Leertaste) gedrückt wird
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = breakingForce;
        }
        else
        {
            currentBreakingForce = 0f;
        }

        // Beschleunigungsmoment auf die vorderen Räder anwenden
        wheelsCol[0].motorTorque = currentAcceleration;
        wheelsCol[1].motorTorque = currentAcceleration;

        // Bremskraft auf alle Räder anwenden
        for (int i = 0; i < wheelsCol.Length; i++)
        {
            wheelsCol[i].brakeTorque = currentBreakingForce;
        }

        // Aktualisiere die Position und Rotation der Rad-Meshes basierend auf den Collidern
        for (int i = 0; i < wheelsMesh.Length; i++)
        {
            Vector3 wheelPosition;
            Quaternion wheelRotation;

            // Hole die Weltposition und Rotation des aktuellen Rads
            wheelsCol[i].GetWorldPose(out wheelPosition, out wheelRotation);

            // Setze die Position und Rotation des zugehörigen Meshes
            wheelsMesh[i].position = wheelPosition;
            wheelsMesh[i].rotation = wheelRotation;
        }
    }

    // Methode wird ausgelöst, wenn das Fahrzeug mit einem Collider in Berührung kommt
    private void OnTriggerEnter(Collider other)
    {
        // Überprüfe, ob das Fahrzeug die Ziellinie erreicht hat
        if (other.tag == "Finish")
        {
            // Aktualisiere die Bestzeit, falls die aktuelle Zeit besser ist
            if (guiController.timer <= guiController.bestTimer)
            {
                guiController.bestTimer = guiController.timer;
                guiController.bestTimerText.text = "BestTime: " + Math.Round(guiController.bestTimer, 2);
            }

            // Starte den Timer neu
            guiController.startTime = Time.time;
        }
    }
}
