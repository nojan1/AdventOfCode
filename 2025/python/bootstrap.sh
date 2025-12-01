#!/bin/bash

"${AOC:=/opt/homebrew/bin/aoc}"
YEAR=2025

[ -z "$1" ] && echo "Usage: $0 <day>" && exit 1

path="./day$1"
[ -d "$path" ] && echo "Day folder $1 already exists, can't bootstrap" && exit 1

mkdir $path
cd $path
$AOC -y $YEAR -d $1 download

cat > main.py <<EOF
#
#   Advent of code $YEAR day $1
#

from itertools import *
from functools import *
import fileinput, sys

def step1(inputFile):
    # Implementation for step 1 goes here

    return ""

def step2(inputFile):
    # Implementation for step 2 goes here

    return ""

print("Running Advent of code $YEAR day $1\n")

inputFile = fileinput.input(encoding="utf-8")

step1Result = step1(inputFile)
print(f"Step1: {step1Result}")

step2Result = step2(inputFile)
print(f"Step2: {step2Result}")
EOF

echo "Successfully bootstrapped day $1 for $YEAR"
echo "To start coding:"
echo "  cd $path"
echo "  python3 main.py"
