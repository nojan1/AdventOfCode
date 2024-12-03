#
#   Advent of code 2024 day 3
#

from itertools import *
from functools import *
import fileinput, re


def step1(data):
    muls = re.findall(r"mul\((\d+),(\d+)\)", data)
    return reduce(lambda acc, cur: acc + (int(cur[0]) * int(cur[1])), muls, 0)


def step2(data):
    total = 0
    buffer = ""
    mulEnable = True

    for d in data:
        buffer += d

        if d == ")":
            if buffer.endswith("do()"):
                mulEnable = True
            elif buffer.endswith("don't()"):
                mulEnable = False
            elif mulEnable:
                matched = re.search(r"mul\((\d+),(\d+)\)", buffer)
                if matched:
                    total += int(matched.group(1)) * int(matched.group(2))

            buffer = ""

    return total


print("Running Advent of code 2024 day 3\n")

inputFile = fileinput.input(encoding="utf-8")
data = reduce(lambda acc, cur: acc + cur, inputFile, "")

# $step1Result = step1("example_input")
# assert($step1Result == "", "Step 1 returned $step1Result which was incorrect!\n")

# $step2Result = step2("example_input")
# assert($step2Result == "", "Step 2 returned $step2Result which was incorrect!\n")

step1Result = step1(data)
print(f"Step1: {step1Result}")

step2Result = step2(data)
print(f"Step2: {step2Result}")
