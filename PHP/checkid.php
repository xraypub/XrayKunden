<?php

         $file_handle = fopen('VERZEICHNIS', 'r');		            // Verzeichnis für Datei mit APP-ID ändern !!!
		 $contents = fread($file_handle, filesize('VERZEICHNIS'));    // Verzeichnis für Datei mit APP-ID ändern !!!
		 var_dump($file_handle);
		 var_dump($contents);
         fclose($file_handle);
		 var_dump(password_verify("yyyxxxccc", $contents));

?>
