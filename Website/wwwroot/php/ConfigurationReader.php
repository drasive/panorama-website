<?php

class ConfigurationReader  {
    
    protected static $configurationFile = 'config.ini';
    
    protected static $releaseSection = 'release';
    protected static $debugMode = 'debugMode';
    
    protected static $applicationSection = 'application';
    protected static $panoramicImagesFolder = 'panoramicImagesFolder';
    protected static $archiveDuration = 'archiveDuration';
    protected static $archiveFrequency = 'archiveFrequency';
    
    
    protected static function getConfiguration() {
        return parse_ini_file(self::$configurationFile, true);
    }
    
    // Release section
    public static function getDebugMode() { 
        return self::getConfiguration()[self::$releaseSection][self::$debugMode];
    }
    
    // Application section
    public static function getPanoramicImagesFolder() { 
        return self::getConfiguration()[self::$applicationSection][self::$panoramicImagesFolder];
    }
    
    public static function getArchiveDuration() { 
        return self::getConfiguration()[self::$applicationSection][self::$archiveDuration];
    }
    
    public static function getArchiveFrequency() { 
        return self::getConfiguration()[self::$applicationSection][self::$archiveFrequency];
    }
    
}

?>