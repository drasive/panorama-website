<?php

set_error_handler('ErrorHandler::handleError');
set_exception_handler('ErrorHandler::handleException');
register_shutdown_function('ErrorHandler::handleShutdown');

require_once('php/Logger.php');

class ErrorHandler  {
    
    private static function Redirect($target) {
        echo '<meta http-equiv="Location" content="' . $target . '">';
    }    
    
    public static function handleError($code, $message, $errFile, $errLine) {
        Logger::logError($message);
        
        self::Redirect('500.php');
        exit(1);
    }
    
    public static function handleException($exception) {
        Logger::logError($exception);
        
        self::Redirect('500.php');
        exit(1);
    }
    
    public static function handleShutdown() {
        if (isset($error) && $error['type'] === E_ERROR) {
          Logger::logError($error);
          
          self::Redirect('500.php');
          exit(1);
        }
    }
    
}

?>
