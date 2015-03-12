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
    <!--[if lt IE 8]>
        <p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
    <![endif]-->

    <?php
    require("_functions.php");
    
    SetActivePage('archive');    
    require("_header.php");
    
    include("_noscript_error.html");
    ?>

    <?php
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
    $maximumDate = strtotime(date('Y-m-d'));
    // TODO: Use config amount of days
    $minimumDate = $maximumDate - ((60 * 60 * 24 * 14) - 1);
    if ($date == null || $date == false || $date == '' || $date < $minimumDate || $date > $maximumDate) {
        // Date couldn't be obtained or is invalid        
        // TODO: Log warning if debug
        $date = strtotime(date('Y-m-d'));
    }
    
    // TODO: Log date info if debug
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

    <?php require("_footer.php") ?>

    <!-- Scripts -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="plugins/unitegallery/themes/grid/ug-theme-grid.js"></script>

    <script src="js/noscript.js"></script>
    <script src="js/archive.js"></script>
</body>
</html>
