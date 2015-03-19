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
    <title>Live Image - Panorama Website</title>
    <meta name="description" content="Have a look at a live panoramic image.">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Styles -->
    <link rel="stylesheet" href="css/main.css">

    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap-theme.min.css">

    <!-- Favicon -->
    <link rel="apple-touch-icon" sizes="57x57" href="images/favicon/apple-touch-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="images/favicon/apple-touch-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="images/favicon/apple-touch-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="images/favicon/apple-touch-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="images/favicon/apple-touch-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="images/favicon/apple-touch-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="images/favicon/apple-touch-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="images/favicon/apple-touch-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="images/favicon/apple-touch-icon-180x180.png">
    <link rel="icon" type="image/png" href="images/favicon/favicon-32x32.png" sizes="32x32">
    <link rel="icon" type="image/png" href="images/favicon/android-chrome-192x192.png" sizes="192x192">
    <link rel="icon" type="image/png" href="images/favicon/favicon-96x96.png" sizes="96x96">
    <link rel="icon" type="image/png" href="images/favicon/favicon-16x16.png" sizes="16x16">
    <link rel="manifest" href="images/favicon/manifest.json">
    <link rel="shortcut icon" href="images/favicon/favicon.ico">
    <meta name="msapplication-TileColor" content="#00aba9">
    <meta name="msapplication-TileImage" content="images/favicon/mstile-144x144.png">
    <meta name="msapplication-config" content="images/favicon/browserconfig.xml">
    <meta name="theme-color" content="#ffffff">

    <!-- Scripts -->
    <script src="bower_components/modernizr/modernizr.js"></script>
    <script src="bower_components/respond/dest/respond.min.js"></script>

    <script src="js/no-fouc-start.js"></script>
</head>
<body>
    <?php
    require("_browserupgrade.php");

    require_once("php/UiHelper.php");
    UiHelper::SetActivePage('index');
    UiHelper::SetActiveSubpage(null);

    require("_header.php");

    require("_noscript-warning.html");
    ?>

    <div class="container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <h1>Live Image</h1>

                <?php
                require_once("php/ImageReader.php");
                
                // Get last image
                $image = ImageReader::GetLastImage();
                if (!is_null($image)) {
                    // Output info
                    $creationDateFormatted = date('l, H:i:s d.m.Y', $image['creationDate']);
                    $ageFormatted = UiHelper::GetTimeDifferenceString(time(), $image['creationDate']);
                    echo '<p>' . 
                             'This is a live panoramic image. ' . 
                             'It was last refreshed on ' . $creationDateFormatted . ' (' . $ageFormatted . ' ago).' .
                         '</p>';
                    
                    // Output image
                    // Path replacement required for the zoom plugin to work correctly
                    $imagePathFormatted = str_replace('\\', '/', $image['imagePath']);
                    $thumbnailPathFormatted = str_replace('\\', '/', $image['thumbnailPath']);
                    
                    echo '<p>' . 
                             '<img class="image-fullscreen image-zoom"' .
                                   'src="'             . $thumbnailPathFormatted . '" ' .
                                   'data-zoom-image="' . $imagePathFormatted     . '" />' .
                         '</p>';
                }
                else {
                    // Output message
                    echo '<p>' . 
                             'Currently, there is no live panoramic image available.' .
                         '</p>';
                }
                ?>

                <p>
                    Visit the <a href="archive.php">archive</a> to see older images.                   
                </p>
            </div>
        </div>
    </div>

    <?php require("_footer.php"); ?>

    <!-- Scripts -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

    <script src="plugins/elevatezoom/jquery.elevateZoom.min.js"></script>

    <script src="js/noscript.js"></script>
    <script src="js/no-fouc-end.js"></script>
    <script src="js/index.js"></script>
</body>
</html>
