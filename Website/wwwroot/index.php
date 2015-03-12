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
    <link rel="stylesheet" href="css/main.css">

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
    
    SetActivePage('index');    
    require("_header.php")
    ?>

    <div class="container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <h1>Live Image</h1>
                <p>
                    This is a live panoramic image. It was last refreshed at [x] ([x] ago).
                </p>
                <p>
                    <img class="fullscreen" src="http://placehold.it/1280x720" />
                </p>
                <p>
                    Visit the <a href="archive.php">archive</a> to see older images.                   
                </p>
            </div>
        </div>

        <?php require("_footer.php") ?>
    </div>

    <!-- Scripts -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

    <script src="js/main.js"></script>
</body>
</html>
