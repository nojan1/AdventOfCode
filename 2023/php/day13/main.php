<?php
/*
    Advent of code 2023 day 13
*/

function rotate($buffer) {
    $newBuffer = [];
    for($x = 0; $x < strlen($buffer[0]); $x++) {
        $s = "";
        for($y = count($buffer) - 1; $y >= 0; $y--) {
            $s .= $buffer[$y][$x];
        } 

        $newBuffer[] = $s;
    }

    return $newBuffer;
}

function parse($inputFile) {
    $lines = array_map("trim", file($inputFile));
    $patterns = array();

    $i = 0;
    while($i < count($lines) - 1) {
        $buffer = [];
        while($i < count($lines) && $lines[$i] != "") {
            $buffer[] = $lines[$i++];
        } 

        $patterns[] = array(
            rotate($buffer),
            $buffer
        );

        $i++;
    }

    return $patterns;
}

function isValidMirror($projection, $start) {
    $i = -1;
    while(true) {
        $i++;
        $sample1 = $start - $i;
        $sample2 = $start + 1 + $i;

        if($sample1 < 0 || $sample2 > count($projection) - 1) return true;
        if($projection[$sample1] != $projection[$sample2]) return false;
    } 
}

function findMirrorIndex($projection) {
    for($i = 0; $i < count($projection) - 1; $i++) {
        if($projection[$i] == $projection[$i + 1] && isValidMirror($projection, $i)) {
            return $i;
        }
    }

    return -1;
}

function findMirrorPoint($pattern, $fixSmudge) {
    foreach($pattern as $key => $projection) {
        for($i = 0; $i < count($projection) - 1; $i++) {
            $mirrorIndex = findMirrorIndex($projection);
            if($mirrorIndex != -1 && !$fixSmudge) return ($mirrorIndex + 1) * max(1,$key * 100);

            for($row = 0; $row < count($projection); $row++) {
                for($col = 0; $col < strlen($projection[$row]); $col++) {
                    $projectionCopy = $projection;
                    $projectionCopy[$row][$col] = $projectionCopy[$row][$col] == "." ? "#" : ".";
                 
                    for($z = 0; $z < count($projectionCopy); $z++) {
                        $newMirrorIndex = findMirrorIndex($projectionCopy);
                        if($newMirrorIndex != -1 && $newMirrorIndex != $mirrorIndex) return ($newMirrorIndex + 1) * max(1,$key * 100);
                    }
                }
            }
        }
    }

    return 0;
}

function step1($inputFile) {
    $patterns = parse($inputFile);
    $mirrorPoints = array_map(fn($pattern) => findMirrorPoint($pattern, false), $patterns);
    return array_sum($mirrorPoints);
}

function step2($inputFile) {
    $patterns = parse($inputFile);
    $mirrorPoints = array_map(fn($pattern) => findMirrorPoint($pattern, true), $patterns);
    return array_sum($mirrorPoints);
}

echo "Running Advent of code 2023 day 13\n";

$step1Result = step1("example_input");
assert($step1Result == 405, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 400, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";

if($step2 <= 23846) echo "To low", PHP_EOL;