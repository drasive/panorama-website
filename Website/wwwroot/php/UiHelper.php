<?php

class UiHelper  {

    private static $activePage = null;
    private static $activeSubpage = null;

    
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
        
        $result = null;
        if ($difference < 60) {
            $result = $difference;
            if ($result != 1) {
                return $result . ' seconds';
            }
            else {
                return $result . ' second';
            }
        }
        else if ($difference < 60 * 60) {
            $result = round($difference / 60, 0);
            if ($result != 1) {
                return $result . ' minutes';
            }
            else {
                return $result . ' minute';
            }
        }
        else if ($difference < 60 * 60 * 24) {
            $result = round($difference / 60 / 60, 0);
            if ($result != 1) {
                return $result . ' hours';
            }
            else {
                return $result . ' hour';
            }
        }
        else if ($difference < 60 * 60 * 24 * 31) {
            $result = round($difference / 60 / 60 / 24, 0);
            if ($result != 1) {
                return $result . ' days';
            }
            else {
                return $result . ' day';
            }
        }
        else if ($difference < 60 * 60 * 24 * 365) {
            $result = round($difference / 60 / 60 / 24 / 30, 0);
            if ($result != 1) {
                return $result . ' months';
            }
            else {
                return $result . ' month';
            }
        }
        else {
            $result = round($difference / 60 / 60 / 24 / 365, 1);
            if ($result != 1) {
                return $result . ' years';
            }
            else {
                return $result . ' year';
            }
        }
    }
    
}

?>
