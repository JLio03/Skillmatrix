# Skillmatrix
> Offizielle Skillmatrix der CM Informatik für die Ablösung des Excel-Files.

## Inhaltsverzeichnis
* [Allgemeine Informationen](#allgemeines)
* [Technische Informationen](#technisches)
* [Installation & Konfiguration](#installation)
* [Troubleshooting / Mögliche Fehlerquellen](#troubleshooting)

## Allgemeines
* Status: **In Entwicklung** / Freigegeben / Wird nicht weiterentwickelt
* Fachliche Ansprechperson: (Janosch Lio (LJa))
* Technische Ansprechperson: (Janosch Lio (LJa))

### Strategische_Einordnung
Ersetzt das riesige Skillmatrix Excel-Sheet.

## Technisches
Technische Informationen zu der Komponente.

### Eigenschaften
Die Komponente erfüllt folgende Eigenschaften:

(falls ja, bitte Beschreibung wie/unter welchem Umständen/mit welcher Konfiguration)

* Stateless: nein
* Mehrmandantenfähig: nein


### Abhängigkeiten
Folgende Services verwendet die Komponente:

| Service       | Version   | Verfügbarkeit | Fehlertoleranzklasse   |
| ------------- | --------- | ------------- | ---------------------- |
| MSSQL DB      | -         | muss          | Absturz                |

#### Fehlertoleranz

Wenn Azure / Azure Entra ID nicht verfügbar ist, ist das Anmelden nicht möglich und die WebApp kann somit nicht verwendet werden.

### Technologiestack
Folgende Technologien werden eingesetzt:
* .NET 8
* Blazor Server
* MSSQL

## Installation
Informationen zur Installation der Komponente.

### Systemvoraussetzungen
Folgende Komponenten müssen auf dem System installiert/vorhanden sein:
* Visual Studio
* .NET 8
* MSSQL

### Installationsanleitung
[Vollständige Anleitung zur Installation der Komponente inklusive benötigter Services.](./Installation.md)

### Entwicklerhinweise
*--*

### Konfigurationsmöglichkeiten
Folgende Konfigurationen müssen in der Datei appsettings.json gesetzt werden.

| Name       | Beschreibung                         | obligatorisch  | Standardwert | Erlaubte Werte
| ---------- | ------------------------------------ | -------------- | ------------ | -------------
| Instance |  Eintrittspunkt der Azure Entra ID Authentifizierung | Ja | https://login.microsoftonline.com/ | -
| Domain | Port auf den sich der Service bindet | Ja | cmiag.ch | -
| Tenant-ID | Mandanten-ID der Organisation CM Informatik auf Azure Entra ID | Ja | ![image](https://github.com/CMInformatik/cmi-skillmatix/assets/132274831/832d3a34-e45f-45bb-8f08-fad1488b623c) | -
| Client-ID | Client-ID der Anwendung (für das Erstellen des Projektes wurde die ID der Kapplanung verwendet) | Ja | ![image](https://github.com/CMInformatik/cmi-skillmatix/assets/132274831/9fba7c2c-8112-4c77-a4cb-8e241ba0c082) | -      
| CallbackPath | Token des Authentifizierungsergebnis wird nach Anmeldevorgang hier hin gesendet | Ja | /signin-oidc | - 

Die App kann nun gestartet werden.

