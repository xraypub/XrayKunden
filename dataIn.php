   <?php 
         $username = "";
	     $pword = "";
	    		 
	     error_reporting(0);
	 
	 	 
	 if ($_SERVER["REQUEST_METHOD"] == "POST" && $_SERVER["SERVER_PORT"] == 443) 
	 {
		 require 'PHP-File mit Zugangsdaten';        // !!!! Daten Ändern !!!
		 
		 if (conn->connect_error) 
	    {
			
         die("Connection failed: ");
		   
        }
		 
		 
         $softwareid = check_input($_POST["softwareid"]);
		 $file_handle = fopen('VERZEICHNIS', 'r');		         // Verzeichnis für Datei mit APP-ID ändern !!!
		 $idhash = fread($file_handle, filesize('VERZEICHNIS'));   // Verzeichnis für Datei mit APP-ID ändern !!!
         fclose($file_handle);
		 
		 if( !(password_verify($softwareid, $idhash)))
		 {
			die("Connection faild"); 
		 }
		 
         $vorname = check_input($_POST["vorname"]);
         $nachname = check_input($_POST["nachname"]);
	     $strasse = check_input($_POST["strasse"]);	
         $ort = check_input($_POST["ort"]);	
         $plz = check_input($_POST["plz"]);
         $kundennr = check_input($_POST["kundennr"]);
		
	   
            
		   $statement = $conn->prepare("INSERT INTO Kunden (Vorname, Nachname, Strasse, Ort, Plz, KundenNr) VALUES ( ?, ?, ?, ?, ?, ?)");
		   $statement->bind_param("ssssss", $vorname, $nachname, $strasse, $ort, $plz, $kundennr);
		   $statement->execute();
		   $statement->close(); 
			
   	    
				
	 }
	 
	 $conn->close();
	 
	 
	 function check_input($data) {
		 $data = trim($data);
		 $data = stripslashes($data);
		 $data = htmlspecialchars($data);
		 return $data;	 
	 }
	 
	
	 
	?>
