<?php

class UiHelper  {

    protected static $activePage = null;

    
    public static function SetActivePage($newActivePage) {
        self::$activePage = $newActivePage;
    }

    public static function CheckNavbarLinkActive($navbarLinkTitle) {
        if (!is_null(self::$activePage) && strcasecmp(self::$activePage, $navbarLinkTitle) === 0) {
            echo " active ";
        }
    }

}

?>
