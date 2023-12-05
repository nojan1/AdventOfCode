<?php
/*
    Advent of code 2023 day 5
*/

function parse($inputFile) {
    $rows = array_map("trim", file($inputFile));
    $data = array();
    $typeBuffer = "";

    foreach($rows as $row){
        if(str_starts_with($row, "seeds")) {
            $data["seeds"] = array_map("intval", explode(" ", explode(": ", $row)[1]));
        }else if(str_contains($row, ":")){
            $typeBuffer = trim(explode(" ", $row)[0]);
            $data[$typeBuffer] = array();
        }else {
            $numbers = explode(" ", trim($row));

            if(count($numbers) == 3) {
                $dst = intval($numbers[0]);
                $src = intval($numbers[1]);
                $len = intval($numbers[2]);

                $data[$typeBuffer][] = array(
                    "dst" => $dst,
                    "src" => $src,
                    "len" => $len
                );
            }
        }
    }

    return $data;
}

function lookup($data, $type, $value) {
    foreach($data[$type] as $ranges) {
        if($value >= $ranges["src"] && $value <= $ranges["src"] + $ranges["len"]) {
            $offset = $value - $ranges["src"];
            return $ranges["dst"] + $offset;
        }
    }

    return $value;
}

function step1($inputFile) {
    $data = parse($inputFile);
    $locations = array_map(function($seed) use ($data){
        $soil = lookup($data, "seed-to-soil", $seed);
        $fertilizer = lookup($data, "soil-to-fertilizer", $soil);
        $water = lookup($data, "fertilizer-to-water", $fertilizer);
        $light = lookup($data, "water-to-light", $water);
        $temperature = lookup($data, "light-to-temperature", $light);
        $humidity = lookup($data, "temperature-to-humidity", $temperature);
        $location = lookup($data, "humidity-to-location", $humidity);

        return $location;
    }, $data["seeds"]);

    $min = array_reduce($locations, "min", PHP_INT_MAX);
    return $min;
}

function step2($inputFile) {
    $data = parse($inputFile);
    $lowestLocation = PHP_INT_MAX;

    for($i = 0; $i < count($data["seeds"]); $i += 2) {
        $from = $data["seeds"][$i];
        $to = $data["seeds"][$i] + $data["seeds"][$i + 1];

        for($seed = $from; $seed < $to; $seed++) {
            $soil = lookup($data, "seed-to-soil", $seed);
            $fertilizer = lookup($data, "soil-to-fertilizer", $soil);
            $water = lookup($data, "fertilizer-to-water", $fertilizer);
            $light = lookup($data, "water-to-light", $water);
            $temperature = lookup($data, "light-to-temperature", $light);
            $humidity = lookup($data, "temperature-to-humidity", $temperature);
            $location = lookup($data, "humidity-to-location", $humidity);

            $lowestLocation = min($location, $lowestLocation);
        }
    }

    return $lowestLocation;
}

echo "Running Advent of code 2023 day 5\n";

$step1Result = step1("example_input");
assert($step1Result == 35, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 46, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
