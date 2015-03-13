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
</head>
<body>
    <?php
    require_once("php/ConfigurationReader.php");
    
    // Get date
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
        
        if (ConfigurationReader::getDebugMode() == true) {
            echo 'Debug: "date" HTTP parameter not existing or invalid, ' . 
            'using the default value "' . date('Y-m-d', $defaultDate) . '"' . 
            '<br />' . PHP_EOL;
        }
    }
    
    if (ConfigurationReader::getDebugMode() == true) {
        echo 'Debug: "date" parameter = "' . date('Y-m-d', $date) . '"' .
        '<br />' . PHP_EOL;
    }
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

                <div id="gallery" class="gallery">
                    <?php
                    function GetArchiveImages() {
                        return array(
                            array(
                                'imagePath'     => 'http://placehold.it/300x150',
                                'thumbnailPath' => 'http://placehold.it/30x15',
                                'title'         => 'title',
                                'description'   => 'desc'
                            ),
                            array(
                                'imagePath'     => 'http://placehold.it/1000x1000',
                                'thumbnailPath' => 'http://placehold.it/2000x2000',
                                'title'         => 'title',
                                'description'   => 'desc'
                            ),
                            array(
                                'imagePath'     => 'http://placehold.it/1280x720',
                                'thumbnailPath' => 'http://placehold.it/480x360',
                                'title'         => 'title',
                                'description'   => 'desc'
                            )
                        );
                    }
                    
                    
                    $imageFiles = GetArchiveImages();
                    
                    foreach ($imageFiles as $imageFile) {
                        echo '<img src="'              . $imageFile['thumbnailPath']  . '"' .
                        'alt="'              . $imageFile['title']          . '"' .
                        'data-image="'       . $imageFile['imagePath']      . '"' .
                        'date-description="' . $imageFile['description']    . '">' . PHP_EOL;
                    }             
                    ?>
                </div>

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
    <script src="js/archive.js"></script>
</body>
</html>
