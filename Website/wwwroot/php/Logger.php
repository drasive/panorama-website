<?php

require_once('vendor/autoload.php');
require_once('php/ConfigurationReader.php');

class Logger  {
    
    private static $logger = null; 
    
    private static function getLogger() {
        if (is_null(self::$logger)) {
            $folder = ConfigurationReader::getLoggingFolder();
            $level = null;
            if (ConfigurationReader::getDebugMode() == true) {
                $level = Psr\Log\LogLevel::DEBUG;
            }
            else {
                $level = Psr\Log\LogLevel::INFO;
            }
            
            self::$logger = new Katzgrau\KLogger\Logger($folder, $level, array (
                'dateFormat' => 'Y-m-d H:i:s.u',
                'extension' => 'log',
                'prefix' => ''
            ));
        }
        
        return self::$logger;
    }
    
    
    private static function logToUI($messageType, $message, $object = null) {        
        echo '[' . strtoupper($messageType) . '] ' . $message . '<br>'; 
        
        if (!is_null($object)) {
            echo '    ' . print_r($object, true) . '<br>';
        }
    }
    
    public static function logError($message, $object = null) {
        if (is_array($object)) {
            self::getLogger()->error($message, $object);
        }
        else {
            self::getLogger()->error($message);
        }
        
        if (ConfigurationReader::getDebugMode() == true) {
            self::logToUI('Error', $message, $object);
        }
    }
    
    public static function logWarning($message, $object = null) {
        if (is_array($object)) {
            self::getLogger()->warning($message, $object);
        }
        else {
            self::getLogger()->warning($message);
        }
        
        if (ConfigurationReader::getDebugMode() == true) {
            self::logToUI('Warning', $message, $object);
        }
    }
    
    public static function logInfo($message, $object = null) {
        if (is_array($object)) {
            self::getLogger()->info($message, $object);
        }
        else {
            self::getLogger()->info($message);
        }
        
        
        if (ConfigurationReader::getDebugMode() == true) {
            self::logToUI('Info', $message, $object);
        }
    }
    
    public static function logDebug($message, $object = null) {
        if (is_array($object)) {
            self::getLogger()->debug($message, $object);
        }
        else {
            self::getLogger()->debug($message);
        }
        
        if (ConfigurationReader::getDebugMode() == true) {
            self::logToUI('Debug', $message, $object);
        }
    }
    
}

?>
