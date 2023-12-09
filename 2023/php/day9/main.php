<?php
/*
    Advent of code 2023 day 9
*/


function parse($inputFile) {
    $lines = array_map("trim", file($inputFile));
    return array_map(fn($x) => array_map("intval", explode(" ", $x)), $lines);
}

function processSequence($values, $previousValues) {

    $isZeros = true;
    $newValues = array();

    for($i = 1; $i < count($values); $i++) {
        $diff = $values[$i] - $values[$i - 1];
        if($diff != 0 && $isZeros) $isZeros = false;
        $newValues[] = $diff;
    }

    $previousValues[] = $newValues;
    if(!$isZeros) return processSequence($newValues, $previousValues);
    return $previousValues;
}

function step1($inputFile) {
    $valueList = parse($inputFile);
    $predictions = array_map(function($values) {
        $processed = processSequence($values, array($values));
        
        for($i = count($processed) - 1; $i > 0; $i--) {
            $processed[$i - 1][] = $processed[$i][count($processed[$i]) - 1] + $processed[$i - 1][count($processed[$i - 1]) - 1];
        }

        return $processed[0][count($processed[0]) - 1];
    }, $valueList);


    return array_sum($predictions);
}

function step2($inputFile) {
    $valueList = parse($inputFile);
    $predictions = array_map(function($values) {
        $processed = processSequence($values, array($values));

        $last = 0;
        for($i = count($processed) - 1; $i >= 0; $i--) {
            $last = $processed[$i][0] - $last; 
        }

        return $last;
    }, $valueList);

    return array_sum($predictions);
}

echo "Running Advent of code 2023 day 8\n";

$step1Result = step1("example_input");
assert($step1Result == 114, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 2, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
