<?php
/*
    Advent of code 2023 day 4
*/

function parseInput($inputFile) {
    return array_reduce(file($inputFile), function($acc, $line){
        $parts = explode(":", trim($line));
        $halfs = explode(" | ", trim($parts[1]));

        $numbers = array_map(function($half){
            return array_reduce(preg_split("#\s+#", $half), function($acc, $g){
                $acc[] = intval(trim($g));
                return $acc;
            }, array());
        }, $halfs);

        $acc[substr($parts[0], 5)] = $numbers;
        return $acc;
    }, array());
}

function step1($inputFile) {
    $cards = parseInput($inputFile);

    $scores = array_map(function($card) use ($cards){
        $score = 0;
        foreach($card[1] as $number) {
            if(in_array($number, $card[0])) {
                $score = $score == 0 ? 1 : $score * 2;
            }
        }

        return $score;
    }, array_values($cards));

    return array_sum($scores);
}

function processCard($cards, $cardNumber, $currentCount) {
    $numbersThatWon = array_filter($cards[$cardNumber][1], function($number) use ($cards, $cardNumber) {
        return in_array($number, $cards[$cardNumber][0]);
    });

    // echo "Card number $cardNumber won on " . count($numbersThatWon) . " numbers\n";

    for($i = 1; $i <= count($numbersThatWon); $i++) {
        if(!array_key_exists($cardNumber + $i, $cards)) continue;
        $currentCount = processCard($cards, $cardNumber + $i, $currentCount + 1);
    }

    return $currentCount;
}

function step2($inputFile) {
    $cards = parseInput($inputFile);
    $currentCount = 0;

    foreach($cards as $cardNumber => $card) {
        $currentCount = processCard($cards, $cardNumber, $currentCount + 1);
    }

    return $currentCount;
}

echo "Running Advent of code 2023 day 4\n";

$step1Result = step1("example_input");
assert($step1Result == 13, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 30, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
if($step2 <= 8123119) { echo "To low!", PHP_EOL; }