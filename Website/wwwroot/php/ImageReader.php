<?php

require_once('php/ConfigurationReader.php');

class ImageReader  {
    
    private static function ParseCreationDate($imagePath) {
        $filename = pathinfo($imagePath)['filename'];
        $filenameFormat = ConfigurationReader::getImageFilenameFormat();
        
        return DateTime::createFromFormat($filenameFormat, $filename)->getTimestamp();
    }
    
    public static function GetLastPanoramicImage() {
        // Search for all current, non-archived panoramic images (excluding thumbnails)
        $imageFolder = ConfigurationReader::getImageFolder() . DIRECTORY_SEPARATOR;
        $images = array_filter(glob($imageFolder . '*.png'), function($filePath) {
            $fileName = pathinfo($filePath)['filename'];
            
            // Return true if the filename doesn't contain 'thumb''
            return !strpos(strtolower($fileName), 'thumb') !== false;
        });
        
        // Use the newest one (filename sorted alphabetically)
        $imagePath = $images[count($images) - 1];
        
        if (!file_exists($imagePath)) {
            return null;
        }
        
        if (ConfigurationReader::getDebugMode() == true) {
            echo 'Debug: Using image "' . $imagePath . '"';
        }
        
        // Parse info
        $thumbnailPath = $imageFolder . pathinfo($imagePath)['filename'] . '_thumb.' . pathinfo($imagePath)['extension'];
        $creationDate = self::ParseCreationDate($imagePath);
        
        return array(
                'imagePath'     => $imagePath,
                'thumbnailPath' => $thumbnailPath,
                'creationDate'  => $creationDate
            );
    }
    
}

?>