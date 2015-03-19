<?php

class ConfigurationReader  {
    
    private static $configurationFile = 'app.ini';
    
    private static $releaseSection = 'release';
    private static $debugMode = 'debugMode';
    private static $testImages = 'testImages';
    
    private static $applicationSection = 'application';
    private static $imageFolder = 'imageFolder';
    private static $imageSubfolderFormat = 'imageSubfolderFormat';
    private static $imageFileFormat = 'imageFileFormat';
    private static $archiveDuration = 'archiveDuration';
    private static $archiveFrequency = 'archiveFrequency';
    private static $loggingFolder = 'loggingFolder';
    
    
    private static function getConfiguration() {
        return parse_ini_file(self::$configurationFile, true);
    }
    
    // Release section
    public static function getDebugMode() {
        return self::getConfiguration()[self::$releaseSection][self::$debugMode];
    }
    
    public static function getTestImages() {
        return self::getConfiguration()[self::$releaseSection][self::$testImages];
    }
    
    // Application section
    public static function getImageFolder() {
        return self::getConfiguration()[self::$applicationSection][self::$imageFolder];
    }
    
    public static function getImageSubfolderFormat() {
        return self::getConfiguration()[self::$applicationSection][self::$imageSubfolderFormat];
    }
    
    public static function getImageFileFormat() {
        return self::getConfiguration()[self::$applicationSection][self::$imageFileFormat];
    }
    
    
    public static function getArchiveDuration() {
        return self::getConfiguration()[self::$applicationSection][self::$archiveDuration];
    }
    
    public static function getArchiveFrequency() {
        return self::getConfiguration()[self::$applicationSection][self::$archiveFrequency];
    }
    
    
    public static function getLoggingFolder() {
        return self::getConfiguration()[self::$applicationSection][self::$loggingFolder];
    }
    
}

?>
