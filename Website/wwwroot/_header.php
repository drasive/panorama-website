<?php 
require_once('php/UiHelper.php');
require_once('php/ConfigurationReader.php');
?>

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
                <li class="<?php UiHelper::CheckNavbarLinkActive('index'); ?>"><a href="index.php">Live Image</a></li>
                <li class="<?php UiHelper::CheckNavbarLinkActive('archive'); ?> dropdown">
                    <a href="archive.php">Archive <span class="caret"></span></a>
                    <ul class="dropdown-menu" role="menu">
                        <?php
                        $currentDay = strtotime(date('Y-m-d'));
                        $archiveDuration = ConfigurationReader::getArchiveDuration();
                        for ($dayIndex = 0; $dayIndex < $archiveDuration; $dayIndex++) {
                            // Output link
                            echo '<li><a href="archive.php?date=' . date('Y-m-d', $currentDay) . '">' . date('D, d.m.Y', $currentDay) . '</a></li>';
                            
                            // Output divider between weeks
                            if (date('N', $currentDay) == 1 && $dayIndex < $archiveDuration - 1) {
                                echo '<li class="divider"></li>';
                            }
                            
                            // Reduce current day by one
                            $currentDay -= 60 * 60 * 24;
                        }
                        ?>
                    </ul>
                </li>
                <li class="<?php UiHelper::CheckNavbarLinkActive('about'); ?>"><a href="about.php">About</a></li>
            </ul>
        </div>
    </div>
</nav>
