<!doctype html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang=""> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8" lang=""> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9" lang=""> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="no-js" lang="">
<!--<![endif]-->
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>Archive - Panorama Website</title>
    <meta name="description" content="Have a look at the panoramic images taken over the last two weeks.">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Styles -->
    <link rel="stylesheet" href="css/main.css">

    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap-theme.min.css">

    <link rel="stylesheet" href="plugins/unitegallery/css/unite-gallery.css" />

    <!-- Scripts -->
    <script src="bower_components/modernizr/modernizr.js"></script>
    <script src="bower_components/respond/dest/respond.min.js"></script>

    <script src="js/no-fouc-start.js"></script>
</head>
<body>
    <?php
    require_once("php/ConfigurationReader.php");
    require_once("php/Logger.php");
    
    // Get date parameter
    $dateParameter = null;    
    if(isset($_GET['date'])) {
        $dateParameter = $_GET['date'];
    }
    
    // Parse date
    $date = null;    
    if (isset($dateParameter) && !empty(trim($dateParameter))) {
        $date = strtotime($dateParameter);
    }
    
    // Validate date
    $archiveDuration = ConfigurationReader::getArchiveDuration();
    
    $maximumDate = strtotime(date('Y-m-d'));    
    $minimumDate = $maximumDate - ((60 * 60 * 24 * $archiveDuration) - 1);
    if ($date == null || $date == false || $date == '' ||
        $date < $minimumDate || $date > $maximumDate) {
        // Date couldn't be obtained or is invalid
        $defaultDate = strtotime(date('Y-m-d'));
        $date = $defaultDate;
        
        Logger::logDebug('[ARCHIVE] "date" HTTP parameter not existing or invalid, ' . 
                         'using the default value "' . date('Y-m-d', $defaultDate));
    }
    
    Logger::logDebug('[ARCHIVE] "date" HTTP parameter = "' . date('Y-m-d', $date) . '"');
    ?>

    <?php
    require("_browserupgrade.php");
    
    require_once("php/UiHelper.php");
    UiHelper::SetActivePage('archive');
    UiHelper::SetActiveSubpage(date('Y-m-d', $date));
    
    require("_header.php");
    
    require("_noscript-error.html");
    ?>

    <div class="wrapper container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <div class="page-header">
                    <h1>Archive <small><?php echo date('l, d.m.Y', $date); ?></small></h1>
                </div>

                <?php
                require_once("php/ImageReader.php");
                
                // Get archive images
                $images = ImageReader::GetArchiveImages($date);
                if (!is_null($images) && count($images) > 0) {
                    echo '<div id="gallery" class="gallery">';
                    
                    // Output images
                    foreach ($images as $image) {
                        echo '<img src="'        . $image['thumbnailPath'] . '"' .
                                  'data-image="' . $image['imagePath']     . '"' .
                                  'alt="'        . $image['title']         . '">' . PHP_EOL;
                                  
                    }
                    
                    echo '</div>';
                }
                else {
                    // Output message
                    echo '<p>' .
                             'There are no archived images for this day.<br />' .
                             'You can try another day by using the drop-down menu in the navigation bar.' .
                         '</p>';
                }
                ?>

                <br />
                <p>
                    Visit the <a href="index.php">homepage</a> to see a live image.        
                </p>
            </div>
        </div>
    </div>

    <?php require("_footer.php"); ?>

    <!-- Scripts -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="plugins/unitegallery/themes/grid/ug-theme-grid.js"></script>

    <script src="js/noscript.js"></script>
    <script src="js/no-fouc-end.js"></script>
    <script src="js/archive.js"></script>
</body>
</html>
