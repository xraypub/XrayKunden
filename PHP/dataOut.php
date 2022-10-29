   <?php 
         $username = "";
	     $pword = "";
	    		 
	    error_reporting(0);
	 
	 	 
	if ($_SERVER["REQUEST_METHOD"] == "POST" && $_SERVER["SERVER_PORT"] == 443)
	{                                                  
		 require 'PHP-File mit Zugangsdaten';              // !!!! Daten Ändern !!!
		 
		 if ($conn->connect_error) 
	    {
			
         die("Error 5");
		   
        }
		 
		 
         $softwareid = check_input($_POST["softwareid"]);
		 $file_handle = fopen('VERZEICHNIS', 'r');		               // Verzeichnis für Datei mit APP-ID ändern !!!
		 $idhash = fread($file_handle, filesize('VERZEICHNIS'));       // Verzeichnis für Datei mit APP-ID ändern !!!
         fclose($file_handle);
		 
		 if( !(password_verify($softwareid, $idhash)))
		 {
			die("Error 1"); 
		 }
		 
                    
		   $statement = $conn->prepare("SELECT * FROM Kunden"); 
		   $statement->execute();
		   $result = $statement->get_result();
		   $listData = $result->fetch_all(MYSQLI_ASSOC);
		   
		   
	
	       echo json_encode($listData);
	
		   $result->free_result();
	       unset($listData);
	 
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
