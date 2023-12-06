<?php
/*
    Advent of code 2023 day 6
*/

function tryPresstime($pressTime, $raceTime, $distance) {
    $remainingTime = $raceTime - $pressTime;
    $distanceTraveled = $remainingTime * $pressTime;
    return $distanceTraveled > $distance;
}

function simulate($times, $distances) {
    $counts = array_map(function($i) use ($times, $distances) {
        $winCount = 0;

        for($pressTime = 0; $pressTime < $times[$i]; $pressTime++) {
            if(tryPresstime($pressTime, $times[$i], $distances[$i])) $winCount++;
        }

        return $winCount;
    }, array_keys($times));

    return array_reduce(array_filter($counts, fn($x) => $x > 0), fn($acc, $cur) => $acc * $cur, 1);
}

function step1($inputFile) {
    $lines = file($inputFile);
    $times = array_map("intval", preg_split("#\s+#", trim(explode(":", trim($lines[0]))[1])));
    $distances = array_map("intval", preg_split("#\s+#", trim(explode(":", trim($lines[1]))[1])));

    return simulate($times, $distances);
}

function step2($inputFile) {
    $lines = file($inputFile);
    $time = intval(preg_replace("#\s+#", "", trim(explode(":", trim($lines[0]))[1])));
    $distance = intval(preg_replace("#\s+#", "", trim(explode(":", trim($lines[1]))[1])));

    return simulate(array($time), array($distance));
}

echo "Running Advent of code 2023 day 6\n";

$step1Result = step1("example_input");
assert($step1Result == 288, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 71503, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
