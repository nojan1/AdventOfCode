<?php
/*
    Advent of code 2023 day 3
*/

function isSymbol($grid, $x, $y) {
    return $grid[$y][$x] != "." && $grid[$y][$x] != "\n" && !is_numeric($grid[$y][$x]);
}

function isAdjacent($grid, $xStart, $yStart, $width) {
    for($x = $xStart - 1; $x <= $xStart + $width; $x++){
        if($x < 0 || $x > strlen($grid[0])) continue;

        if($yStart - 1 > 0 && isSymbol($grid, $x, $yStart - 1)) return true;
        if($yStart + 1 < count($grid) && isSymbol($grid, $x, $yStart + 1)) return true;
    }

    if($xStart - 1 > 0 && isSymbol($grid, $xStart - 1, $yStart)) return true;
    if($xStart + $width < strlen($grid[0]) && isSymbol($grid, $xStart + $width, $yStart)) return true;

    return false;
}

function step1($inputFile) {
    // $grid = array_map("trim", file($inputFile));
    $grid = file($inputFile);   
    print_r($grid);
    
    $partNumbers = array();
    
    for($y = 0; $y < count($grid); $y++){
        $numberBuffer = "";

        for($x = 0; $x < strlen($grid[$y]); $x++) {
            if(!is_numeric($grid[$y][$x])) {
                if($numberBuffer != "") {
                    if(isAdjacent($grid, $x - strlen($numberBuffer), $y, strlen($numberBuffer))){
                        $partNumbers[] = intval($numberBuffer);
                    }
                    $numberBuffer = "";
                }
            } else {
                $numberBuffer .= $grid[$y][$x];
            }
        }

        if($numberBuffer != "" && isAdjacent($grid, strlen($grid[$y]) - strlen($numberBuffer), $y, strlen($numberBuffer))){
            echo "Derp";
            $partNumbers[] = intval($numberBuffer);
        }
    }

    print_r($partNumbers);

    return array_sum($partNumbers);
}

function step2($inputFile) {
    //Implementation for step 2 goes here

    return "";
}

echo "Running Advent of code 2023 day 3\n";

$step1Result = step1("example_input");
assert($step1Result == 4361, "Step 1 returned $step1Result which was incorrect!\n");
// assert($step1Result == 925, "Step 1 returned $step1Result which was incorrect!\n");

//$step2Result = step2("example_input");
//assert($step2Result == "", "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
