<nav class="navbar navbar-default navbar-fixed-top" role="navigation">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="index.php">Panorama Website</a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="<?php CheckNavbarLinkActive('index'); ?>"><a href="index.php">Live Image</a></li>
                <li class="<?php CheckNavbarLinkActive('archive'); ?> dropdown">
                    <a href="archive.php">Archive <span class="caret"></span></a>
                    <ul class="dropdown-menu" role="menu">
                        <?php
                            // TODO: List last 14 (configurable) days. List days without images?
                        ?>

                        <li><a href="#">Action</a></li>
                        <li><a href="#">Another action</a></li>
                        <li><a href="#">Something else here</a></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">Nav header</li>
                        <li><a href="#">Separated link</a></li>
                        <li><a href="#">One more separated link</a></li>
                    </ul>
                </li>
                <li class="<?php CheckNavbarLinkActive('about'); ?>"><a href="about.php">About</a></li>
            </ul>
        </div>
    </div>
</nav>
