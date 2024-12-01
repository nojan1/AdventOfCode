<?php
/*
    Advent of code 2023 day 14
*/

function parse($inputFile) {
    return array_map("str_split", array_map("trim", file($inputFile)));
}

function roll_rocks(&$data, $xScanDir, $yScanDir, $xLookDir, $yLookDir) {
    $didRoll = false;

    $yStart = $yScanDir > 0 ? 0 : count($data) - 1; 
    $xStart = $xScanDir > 0 ? 0 : count($data[0]) - 1; 
    $yStop = $yScanDir > 0 ? count($data) - 1 : 0;
    $xStop = $xScanDir > 0 ? count($data[0]) - 1 : 0;

    for ($y=$yStart; $y != $yStop + $yScanDir; $y += $yScanDir) { 
        if($y < 0 || $y > count($data) - 1) continue;

        for ($x=$xStart; $x != $xStop + $xScanDir; $x += $xScanDir) { 
            if($x < 0 || $x > count($data[0]) - 1) continue;

            if($y + $yLookDir < 0 || $y + $yLookDir > count($data) - 1) continue;
            if($x + $xLookDir < 0 || $x + $xLookDir > count($data[0]) - 1) continue;

            if($data[$y][$x] == "O" && $data[$y + $yLookDir][$x + $xLookDir] == ".") {
                $didRoll = true;
                $data[$y][$x] = ".";
                $data[$y + $yLookDir][$x + $xLookDir] = "O";
            }
        }
    }

    return $didRoll;
}

function calculate_load($data) {
    $load = 0;
    for ($y=0; $y < count($data); $y++) { 
        for ($x=0; $x < count($data[0]); $x++) { 
            if($data[$y][$x] == "O") {
                $load += abs($y - count($data));
            }
        }
    }

    return $load;;
}

function render_grid($data) {
    $s = "";

    for ($y=0; $y < count($data); $y++) { 
        for ($x=0; $x < count($data[0]); $x++) { 
            $s .= $data[$y][$x];
        }

        $s .= PHP_EOL;
    }

    return $s;
}

function step1($inputFile) {
    $data = parse($inputFile);
    while(roll_rocks($data, 1, -1, 0, -1)) { }
    return calculate_load($data);
}

function step2($inputFile) {
    $data = parse($inputFile);

    echo render_grid($data), PHP_EOL, PHP_EOL;


    $backlog = array();
    // $target = 1000000000;
    $target = 1;
    $lookForRepeat = true;
    for($i = 0; $i < $target; $i++) {
        roll_rocks($data, 1, 1, 0, -1);
        roll_rocks($data, -1, 1, -1, 0);
        // echo render_grid($data), PHP_EOL;
        // return 0;
        roll_rocks($data, 1, -1, 0, 1);
        roll_rocks($data, 1, -1, 1, 0);
        echo render_grid($data), PHP_EOL;
        return 0;
        // $state = render_grid($data);
        // if($lookForRepeat && in_array($state, $backlog)) {
        //     $lookForRepeat = false;
        //     echo "Encountered same state at $i", PHP_EOL;
            
        //     $target = $target  $i;
        //     echo "New target ", $target, PHP_EOL;
        // }

        // $backlog[] = $state;

        // print_grid($data);
    }

    echo render_grid($data), PHP_EOL;

    return calculate_load($data);
}

echo "Running Advent of code 2023 day 14\n";

$step1Result = step1("example_input");
assert($step1Result == 136, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 64, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
