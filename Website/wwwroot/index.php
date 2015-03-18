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
    
    // TODO: _
    require_once("php/Common.php");
    Common::getLogger()->info('Test');
    
    ?>

    <div class="container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <h1>Live Image</h1>

                <?php
                require_once("php/ImageReader.php");
                
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
