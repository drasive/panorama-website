<?php

class UiHelper  {

    protected static $activePage = null;
    protected static $activeSubpage = null;

    
    public static function SetActivePage($newActivePage) {
        self::$activePage = $newActivePage;
    }

    public static function CheckPageLinkActive($pageLinkTitle) {
        if (!is_null(self::$activePage) && strcasecmp(self::$activePage, $pageLinkTitle) === 0) {
            return " active ";
        }
    }
    
    public static function SetActiveSubpage($newActiveSubpage) {
        self::$activeSubpage = $newActiveSubpage;
    }

    public static function CheckSubpageLinkActive($subpageLinkTitle) {
        if (!is_null(self::$activeSubpage) && strcasecmp(self::$activeSubpage, $subpageLinkTitle) === 0) {
            return " active ";
        }
    }

}

?>
