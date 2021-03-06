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
    <title>About - Panorama Website</title>
    <meta name="description" content="About the panorama website.">
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
    UiHelper::SetActivePage('about');
    UiHelper::SetActiveSubpage(null);
    
    require("_header.php");
    ?>

    <div class="container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <h1>About</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-6">
                <h2>What</h2>

                <p>
                    A website where people can view live and archived panoramic images
                    generated from snapshots taken by a network camera.
                </p>
            </div>
            <div class="col-sm-6">
                <h2>Why</h2>
                <p>
                    The creation of this website and the program to generate the panoramic images was a school project
                    for module #152 at the vocational college <a href="http://home.gibm.ch/">GIBM</a> (Switzerland).
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <h2>Technology</h2>

                <h4>Website</h4>
                <p>
                    Frontend: A HTML5 website with CSS3 and Bootstrap<br />
                    Backend: PHP 5.4 running on Apache 2.4
                </p>
                <h4>Panorama Creator</h4>
                <p>
                    C# with .NET 4.5, Accord.NET framework
                </p>
            </div>
            <div class="col-sm-6">
                <h2>Who</h2>

                <h4>Developer</h4>
                <p>
                    Dimitri Vranken<br />
                    Email: <a href="mailto:dimitri.vranken@hotmail.ch">dimitri.vranken@hotmail.ch</a><br />
                </p>
                <h4>Expert</h4>
                <p>
                    B&eacute;atrice Duc<br />
                    Email: <a href="mailto:b.duc@gibmit.ch">b.duc@gibmit.ch</a><br />
                </p>
            </div>
        </div>
    </div>

    <?php require("_footer.php"); ?>

    <!-- Scripts -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

    <script src="plugins/elevatezoom/jquery.elevateZoom.min.js"></script>

    <script src="js/no-fouc-end.js"></script>
    <script src="js/index.js"></script>
</body>
</html>
