<?php
/*
    Advent of code 2023 day 10
*/

function getValidChars($self, $direction) {
    switch($direction) {
        case "UP":
            switch($self){
                case "|":
                case "L":
                    return array("|", "7", "F", "S");
                default:
                    return array();
            }
        case "RIGHT":
            switch($self){
                case "-":
                case "L":
                case "F":
                    return array("-", "J", "7", "S");
                default:
                    return array();
            }
        case "DOWN":
            switch($self){
                case "|":
                case "7":
                    return array("|", "7", "F", "S");
                case "F":
                    return array("|", "J", "S");
                default:
                    return array();
            }
        case "LEFT":
            switch($self){
                case "-":
                case "J":
                case "7":
                    return array("-", "F", "L", "S");
                default:
                    return array();
            }
    }
}

function isValidChar($x, $y, $charRows, $self, $direction) {
    if($x < 0 || $y < 0 || $x > count($charRows[0]) - 1 || $y > count($charRows) - 1) return false;
    $validChars = getValidChars($self, $direction);
    return in_array($charRows[$y][$x], $validChars);
}

function isValid($x, $y, $charRows) {
    $self = $charRows[$y][$x];
    if($self == ".") return false;
    if($self == "S") return true;

    $validDirections = array(
        isValidChar($x - 1, $y, $charRows, $self, "LEFT"),
        isValidChar($x, $y - 1, $charRows, $self, "UP"),
        isValidChar($x + 1, $y, $charRows, $self, "RIGHT"),
        isValidChar($x, $y + 1, $charRows, $self, "DOWN")
    );

    return count(array_filter($validDirections, fn($x) => $x));
}

function findStart($grid){
     for($y = 0; $y < count($grid); $y++){
        for($x = 0; $x < count($grid[0]); $x++) {
            if($grid[$y][$x] == "S") return array("X" => $x, "Y" => $y);
        }
    }
}

function parse($inputFile) {
    $charRows = array_map("str_split", array_map("trim", file($inputFile)));
    $grid = array_map(
        fn($y, $row) => array_map(
            fn($x, $char) => isValid($x, $y, $charRows) ? $char : ".",
                array_keys($row), array_values($row))
    , array_keys($charRows), array_values($charRows));

    // for($y = 0; $y < count($grid); $y++){
    //     for($x = 0; $x < count($grid[0]); $x++) {
    //         echo $grid[$y][$x];
    //     }
    //     echo PHP_EOL;
    // }

    return array($grid, findStart($grid));
}

function follow($x, $y, $grid, $direction, $visited){
    if($x < 0 || $y < 0 || $x > count($grid[0]) - 1 || $y > count($grid) || $grid[$y][$x] == ".") return 0;

    $steps = 0;
    while(true){
        $key = "X:" . $x . ",Y:". $y;
        if(in_array($key, $visited)) return count($visited) / 2;

        $visited[] = $key;
        switch($grid[$y][$x]) {
            case "L":
                if($direction == "DOWN") $direction = "RIGHT";
                else $direction = "UP";
                break;
            case "J":
                if($direction == "DOWN") $direction = "LEFT";
                else $direction = "UP";
                break;
            case "7":
                if($direction == "RIGHT") $direction = "DOWN";
                else $direction = "LEFT";
                break;
            case "F":
                if($direction == "LEFT") $direction = "DOWN";
                else $direction = "RIGHT";
                break;
        }

        switch($direction){
            case "UP":
                $y--;
                break;
            case "RIGHT":
                $x++;
                break;
            case "DOWN":
                $y++;
                break;
            case "LEFT":
                $x--;
                break;
        }
    }
}

function step1($inputFile) {
    list($grid, $start) = parse($inputFile);
    
    $paths = array(
        follow($start["X"], $start["Y"] - 1, $grid, "UP", array("X:" . $start["X"] . ",Y:" . $start["Y"])),
        follow($start["X"] + 1, $start["Y"], $grid, "RIGHT", array("X:" . $start["X"] . ",Y:" . $start["Y"])),
        follow($start["X"], $start["Y"] + 1, $grid, "DOWN", array("X:" . $start["X"] . ",Y:" . $start["Y"])),
        follow($start["X"] - 1, $start["Y"], $grid, "LEFT", array("X:" . $start["X"] . ",Y:" . $start["Y"]))
    );

    return array_reduce($paths, "max", 0);
}

function probeRegion($x, $y, $grid, $size, $visited = array()) {
    if($x < 0 || $y < 0 || $x > count($grid[0]) - 1 || $y > count($grid) - 1 || $grid[$y][$x] != ".") return $size - 1;
    
    $key = "X:" . $x . ",Y:". $y;
    if(in_array($key, $visited)) return $size - 1;
    $visited[] = $key;

    $size = probeRegion($x, $y - 1, $grid, $size + 1, $visited);
    $size = probeRegion($x + 1, $y, $grid, $size + 1, $visited);
    $size = probeRegion($x, $y + 1, $grid, $size + 1, $visited);
    $size = probeRegion($x - 1, $y, $grid, $size + 1, $visited);

    return $size;
}

function step2($inputFile) {
    list($grid) = parse($inputFile);
    
    $regions = array();
    for($y = 0; $y < count($grid); $y++){
        for($x = 0; $x < count($grid[0]); $x++) {
            if($grid[$y][$x] == ".") {
                $regions[] = probeRegion($x, $y, $grid, 1, array());
            }
        }
    }

    print_r($regions);

    return array_reduce($regions, "max", 0);
}

echo "Running Advent of code 2023 day 10\n";

$step1Result = step1("example_input");
assert($step1Result == 8, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input2");
assert($step2Result == 8, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
