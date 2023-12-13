<?php
/*
    Advent of code 2023 day 11
*/

function parse($inputFile, $expandAmount) {
    $grid = array_map("str_split", array_map("trim", file($inputFile)));
    $galaxies = array();

    $emptyRows = array();
    $emptyCols = array();

    for ($y=0; $y < count($grid); $y++) { 
        $isRowEmpty = true;
        for ($x=0; $x < count($grid[0]); $x++) { 
            if($grid[$y][$x] == "#") {
                $galaxies[] = array($x, $y);
                $isRowEmpty = false;
            }
        }

        if($isRowEmpty) {
            $emptyRows[] = $y;
        }
    }

    for ($x=0; $x < count($grid[0]); $x++) { 
        $isColumnEmpty = true;
        for ($y=0; $y < count($grid); $y++) { 
            if($grid[$y][$x] == "#") {
                $isColumnEmpty = false;
            }
        }

        if($isColumnEmpty) {
            $emptyCols[] = $x;
        }
    }

    foreach ($galaxies as &$galaxy) {
        $columnGaps = array_filter($emptyCols, fn($x) => $x < $galaxy[0]);
        $rowGaps = array_filter($emptyRows, fn($y) => $y < $galaxy[1]);

        $galaxy[0] += count($columnGaps) * $expandAmount;
        $galaxy[1] += count($rowGaps) * $expandAmount;
    }

    return $galaxies;
}

function getDistance( $galaxies) {
    $distances = [];

    for($i = 0; $i < count($galaxies); $i++) {
        for($y = $i + 1; $y < count($galaxies); $y++) {
            $distances[] = abs($galaxies[$i][0] - $galaxies[$y][0]) + abs($galaxies[$i][1] - $galaxies[$y][1]);
        }
    }

    return array_sum($distances);
}

function step1($inputFile) {
    $galaxies = parse($inputFile, 1);
    return getDistance($galaxies);
}

function step2($inputFile, $expandAmount) {
    $galaxies = parse($inputFile, $expandAmount - 1);
    return getDistance($galaxies);
}

echo "Running Advent of code 2023 day 11\n";

$step1Result = step1("example_input");
assert($step1Result == 374, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input", 100);
assert($step2Result == 8410, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input", 1000000);
echo "Step2: {$step2}\n";
