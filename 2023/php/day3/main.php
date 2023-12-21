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

        if($yStart - 1 > 0 && isSymbol($grid, $x, $yStart - 1)) return array("x" => $x, "y" => $yStart - 1);
        if($yStart + 1 < count($grid) && isSymbol($grid, $x, $yStart + 1)) return array("x" => $x, "y" => $yStart + 1);;
    }

    if($xStart - 1 > 0 && isSymbol($grid, $xStart - 1, $yStart)) return array("x" => $xStart - 1, "y" => $yStart);
    if($xStart + $width < strlen($grid[0]) && isSymbol($grid, $xStart + $width, $yStart)) return array("x" => $xStart + $width, "y" => $yStart);

    return false;
}

function findPartnumbers($grid) {
    $partNumbers = array();
    
    for($y = 0; $y < count($grid); $y++){
        $numberBuffer = "";

        for($x = 0; $x < strlen($grid[$y]); $x++) {
            if(!is_numeric($grid[$y][$x])) {
                if($numberBuffer != "") {
                    $symbolLocation = isAdjacent($grid, $x - strlen($numberBuffer), $y, strlen($numberBuffer));
                    if($symbolLocation != false){
                        $partNumbers[] = array("num" => intval($numberBuffer), "symbol" => $symbolLocation);
                    }
                    $numberBuffer = "";
                }
            } else {
                $numberBuffer .= $grid[$y][$x];
            }
        }

        $symbolLocation = isAdjacent($grid, strlen($grid[$y]) - strlen($numberBuffer), $y, strlen($numberBuffer));
        if($numberBuffer != "" && $symbolLocation != false){
            $partNumbers[] = array("num" => intval($numberBuffer), "symbol" => $symbolLocation);
        }
    }

    return $partNumbers;
}

function step1($inputFile) {
    $grid = file($inputFile);   
    $partNumbers = findPartnumbers($grid);
    return array_sum(array_map(fn($x) => $x["num"], $partNumbers));
}

function step2($inputFile) {
    $grid = file($inputFile);   
    $partNumbers = findPartnumbers($grid);

    $gearRatio = 0;

    $taken = array();
    foreach ($partNumbers as $id => $part) {
        foreach ($partNumbers as $id2 => $part2) {
            $symbol = $grid[$part["symbol"]["y"]][$part["symbol"]["x"]];
            if($id != $id2 && $symbol == "*" && !in_array($id, $taken) && !in_array($id2, $taken) && $part["symbol"]["x"] == $part2["symbol"]["x"] && $part["symbol"]["y"] == $part2["symbol"]["y"]) {
                $taken[] = $id;
                $taken[] = $id2;

                // echo "Gear $id * $id2, ", $part["num"] , " * ", $part2["num"], PHP_EOL;
                $gearRatio += $part["num"] * $part2["num"];
            }
        }
    }

    return $gearRatio;
}

echo "Running Advent of code 2023 day 3\n";

$step1Result = step1("example_input");
assert($step1Result == 4361, "Step 1 returned $step1Result which was incorrect!\n");
// assert($step1Result == 925, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 467835, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
