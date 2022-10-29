# XrayKunden - Dateneingabe und Datenbank-Speicherung

XrayKunden ist ein 'Play-Around', um C# WPF/MVVM Funktionalitäten zu testen. Es gibt 3 TabViews für Kundendaten-Eingaben, Speicherung und Listendarstellung.

1. **Kunden-Daten-Eingabe:**  Verschiedene Kundendaten wie Name, Adresse etc. können in einer Eingabemaske eingegeben werden. Die Daten können dann optional gespeichert werden.
<br><br> a) als JSON File lokal auf dem Rechner <br> b) in einer lokal installierten Postgres Datenbank <br> c) in einer MySQL Datenbank im Internet - C# kommuniziert mittels PHP<br><br>
2. **Postgres-Kundenliste:** Daten aus der lokalen Postgres-Datenbank werden als Liste dargestellt und können geändert sowie zurück gespeichert werden.<br><br>
3. **MySQL-Kundenliste:** Daten werden mittels PHP/JSON aus der MySQL Datenbank ausgelesen, in C# deserialisiert und in einer Liste dargestellt. Die Daten können geändert und zurück gespeichert werden.<br><br>



**Es findet keine umfangreiche Daten-Eingabe Validierung statt, ebenso fehlt eine ausreichende Exceptionbehandlung!**
