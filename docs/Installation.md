# Installationsanleitung

1. "ConnectionStrings" in Appsettings.json anpassen. Folgend das Format:
> Data Source={NBPLJA01};Initial Catalog={Datenbankname};{User ID=;Password=};Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False
Zu beachten:
- Initial Catalog legt den Namen der Datenbank fest
- User muss genügend Rechte auf dem SQL-Server haben
2. "Konfigurationsmöglichkeiten" in README.md anpassen
- Die Datenbank wird beim Starten der App mit dem Namen, der im ConnectionString festgelegt wurde, erstellt
- Es müssen keine Migrations initial ausgeführt werden, diese werden beim Starten der App erstellt. 
