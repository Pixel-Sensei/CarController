using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GUIController : MonoBehaviour
{
    // Startzeit, zu der das Spiel oder der Timer beginnt
    public float startTime;

    // Aktuelle Zeit des Timers
    public float timer;

    // Bester bisher gemessener Timerwert
    public float bestTimer;

    // Textfeld für die Anzeige der aktuellen Zeit (Timer)
    public TMP_Text timerText;

    // Textfeld für die Anzeige des besten Timerwerts
    public TMP_Text bestTimerText;

    // Start wird einmalig beim Start des Spiels aufgerufen
    void Start()
    {
        // Speichere den Zeitpunkt, an dem das Spiel gestartet wurde
        startTime = Time.time;

        // Initialisiere den besten Timerwert mit Unendlichkeit (noch kein Wert gesetzt)
        bestTimer = Mathf.Infinity;

        // Setze den Text für den besten Timer auf einen Platzhalter
        bestTimerText.text = "BestTime: -";
    }

    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        // Berechne die aktuelle Zeit des Timers basierend auf der verstrichenen Zeit
        timer = Time.time - startTime;

        // Aktualisiere die Anzeige des Timers im Textfeld (auf zwei Dezimalstellen gerundet)
        timerText.text = "Timer: " + Math.Round(timer, 2);
    }
}
