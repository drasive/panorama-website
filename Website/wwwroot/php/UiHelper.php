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

    
    public static function GetTimeDifferenceString($timestamp1, $timestamp2) {
        $difference = abs($timestamp1 - $timestamp2);
        
        if ($difference < 60) {
            return round($difference / 60, 0) . ' seconds';
        }
        else if ($difference < 60 * 60) {
            return round($difference / 60, 0) . ' minutes';
        }
        else if ($difference < 60 * 60 * 24) {
            return round($difference / 60 / 60, 0) . ' hours';
        }
        else if ($difference < 60 * 60 * 24 * 31) {
            return round($difference / 60 / 60 / 24, 0) . ' days';
        }
        else if ($difference < 60 * 60 * 24 * 365) {
            return round($difference / 60 / 60 / 24 / 30, 0) . ' months';
        }
        else {
            return round($difference / 60 / 60 / 24 / 365, 1) . ' years';
        }
    }
    
}

?>
