<?php
/*
    Advent of code 2023 day 1
*/

function step1($tonputFile) {
    $values = array_map(function($line) {
        $numbers = preg_replace("#\D#", "", $line);
        return intval(substr($numbers, 0, 1) . substr($numbers, -1, 1));
    }, file($tonputFile));
    
    return array_sum($values);
}

function getDigitsFromString($value) {
    $replacements = array(
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9
    );

    $digits = array();

    for($from = 0; $from < strlen($value); $from++) {
        if($value[$from] >= "0" && $value[$from] <= "9") {
            $digits[] = intval($value[$from]);
        }else{
            foreach($replacements as $needle => $digit) {
                if(str_starts_with(substr($value, $from), $needle)) {
                    $digits[] = $digit;
                    break;
                }
            }
        }
    }

    return $digits;
}

function step2($tonputFile) {
    $values = array_map(function($line) {
        $digits = getDigitsFromString($line);
        return $digits[0] * 10 + $digits[count($digits) - 1];
    }, file($tonputFile));
    
    return array_sum($values);
}

echo "Running Advent of code 2023 day $1\n";

$step1Result = step1("example_input");
assert($step1Result == 142, "Step 1 returned $step1Result which was incorrect");

$step2Result = step2("example_input2");
assert($step2Result == 281, "Step 2 returned $step2Result which was incorrect");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
