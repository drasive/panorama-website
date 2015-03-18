<a href="ImageReader.php">ImageReader.php</a><?php

require_once('php/ConfigurationReader.php');

class ImageReader  {
    
    private static function ParseCreationDate($imagePath) {
        $filename = pathinfo($imagePath)['filename'];
        $filenameFormat = ConfigurationReader::getImageFileFormat();
        
        return DateTime::createFromFormat($filenameFormat, $filename)->getTimestamp();
    }
    
    private static function GenerateThumbnailPath($imageFolder, $imagePath) {
        return $imageFolder . pathinfo($imagePath)['filename'] . '_thumb.' . pathinfo($imagePath)['extension'];
    }
    
    
    public static function GetLastImage() {
        if (ConfigurationReader::getTestImages() == true) {
            // Return test image
            return array(
                'imagePath'     => 'http://placehold.it/3840x2160',
                'thumbnailPath' => 'http://placehold.it/1280x720',
                'creationDate'  => time()
            ); 
        }
        
        // Get all current panoramic images (sorted so new images are first, excluding thumbnails)
        $imageFolder = ConfigurationReader::getImageFolder() . DIRECTORY_SEPARATOR;
        $imageFiles = array_filter(glob($imageFolder . '*.png'), function($filePath) {
            $fileName = pathinfo($filePath)['filename'];
            
            // Return true if the filename doesn't contain "thumb"
            return !strpos(strtolower($fileName), 'thumb') !== false;
        });
        
        // Remove gaps in array keys (created by filtering)
        $imageFiles = array_values(array_filter($imageFiles));
        
        // TODO: Refactor into logging
        if (ConfigurationReader::getDebugMode() == true) {
            echo '[DEBUG] Images (' . count($imageFiles) . '): ' . print_r($imageFiles, true) . PHP_EOL;
        }
        
        if (count($imageFiles) === 0) {
            // No images
            return null;
        }
        
        // Use the newest one (list sorted so new images are first)
        $imagePath = $imageFiles[count($imageFiles) - 1];
        
        if (!file_exists($imagePath)) {
            return null;
        }
        
        // Parse info
        $thumbnailPath = self::GenerateThumbnailPath($imageFolder, $imagePath);
        $creationDate = self::ParseCreationDate($imagePath);
        
        return array(
                'imagePath'     => $imagePath,
                'thumbnailPath' => $thumbnailPath,
                'creationDate'  => $creationDate
            );
    }
    
    public static function GetArchiveImages($date) {
        if (ConfigurationReader::getTestImages() == true) {
            // Return test images
            return array(
                array(
                    'imagePath'     => 'http://placehold.it/1280x720',
                    'thumbnailPath' => 'http://placehold.it/640x360',
                    'title'         => 'Test 1280x720'
                ),
                array(
                    'imagePath'     => 'http://placehold.it/1500x300',
                    'thumbnailPath' => 'http://placehold.it/150x30',
                    'title'         => 'Test 1500x300 '
                ),
                array(
                    'imagePath'     => 'http://placehold.it/2560x1440',
                    'thumbnailPath' => 'http://placehold.it/1920x1080',
                    'title'         => 'Test 2560x1440'
                )
            );
        }
        
        // Check if images were archived that day
        $imageFolder = ConfigurationReader::getImageFolder() . DIRECTORY_SEPARATOR;
        $imageSubfolderFormat = ConfigurationReader::getImageSubfolderFormat();
        $imageSubfolder = $imageFolder . date($imageSubfolderFormat, $date) . DIRECTORY_SEPARATOR;        
        
        if (!file_exists($imageSubfolder) || !is_dir($imageSubfolder)) {
            // No archive folder for that day
            return array();
        }
        
        // Get all archived images for that day (sorted so new images are first, excluding thumbnails)
        $imageFiles = array_filter(glob($imageSubfolder . '*.png'), function($filePath) {
            $fileName = pathinfo($filePath)['filename'];
            
            // Return true if the filename doesn't contain "thumb"
            return !strpos(strtolower($fileName), 'thumb') !== false;
        });
        
        // Remove gaps in array keys (created by filtering)
        $imageFiles = array_values(array_filter($imageFiles));
        
        if (count($imageFiles) === 0) {
            // No images archived for that day
            return array();
        }
        
        // TODO: Refactor into logging
        if (ConfigurationReader::getDebugMode() == true) {
            echo '[DEBUG] Images (' . count($imageFiles) . '): ' . print_r($imageFiles, true) . PHP_EOL;
        }
        
        // Filter images based on time between creation        
        $archiveFrequency = ConfigurationReader::getArchiveFrequency() * 60;
        $imageFilesFiltered = array();
        $lastCreationDate = null;
        for ($imageIndex = 0; $imageIndex < count($imageFiles) - 1; $imageIndex++) {
            $currentImage = $imageFiles[$imageIndex];
            $currentCreationDate = self::ParseCreationDate($currentImage);
            
            if ($lastCreationDate == null ||
                abs($lastCreationDate - $currentCreationDate) >= $archiveFrequency) {
                
                array_push($imageFilesFiltered, $currentImage);
                $lastCreationDate = $currentCreationDate;
            }
        }
        
        // Parse info
        $images = array();
        foreach ($imageFiles as $imageFile) {
            $thumbnailPath = self::GenerateThumbnailPath($imageSubfolder, $imageFile);
            $title = date('l, H:i:s d.m.Y', self::ParseCreationDate($imageFile));
            
            $image = array(
                'imagePath'     => $imageFile,
                'thumbnailPath' => $thumbnailPath,
                'title'         => $title
            );
            
            array_push($images, $image);
        }
        
        return $images;
    }
    
}

?>
