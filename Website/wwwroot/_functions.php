<?php

$activePage = null;

function SetActivePage($newActivePage) {
    global $activePage;
    
    $activePage = $newActivePage;
}

function CheckNavbarLinkActive($navbarLinkTitle) {
    global $activePage;
    
    if (!is_null($activePage) && $activePage === $navbarLinkTitle) {
        echo " active ";
    }
}

?>
