<?php

require_once('composer_components/autoload.php');
require_once('php/ConfigurationReader.php');

class Common  {
    
    protected static $logger = null; 
    
    public static function getLogger() {
        if (is_null(self::$logger)) {
            $folder = ConfigurationReader::getLoggingFolder();
            $level = null;
            if (ConfigurationReader::getDebugMode() == true) {
                $level = Psr\Log\LogLevel::DEBUG;
            }
            else {
                $level = Psr\Log\LogLevel::INFO;
            }
            
            // TODO: _Array options not working
            self::$logger = new Katzgrau\KLogger\Logger($folder, $level, array (
                'dateFormat' => 'Y-m-d H:i:s.u',
                'extension' => 'log',
                'prefix' => ''
            ));
        }
        
        return self::$logger;
    }
    
    
    public static function arrayToString($array) {
        return '(' . count($array) . '): ' . print_r($array, true);
    }
 
}

?>
