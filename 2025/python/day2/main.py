#
#   Advent of code 2025 day 2
#

import fileinput, re

def match(ranges, regex):
    result = 0
    for curRange in ranges:
        for val in range(curRange[0], curRange[1] + 1):
            if re.match(regex, str(val)) is not None:
                result += val

    return result

print("Running Advent of code 2025 day 2\n")

inputFile = fileinput.input(encoding="utf-8")
ranges = [(int(x.split("-")[0]), int(x.split("-")[1])) for x in inputFile.readline().split(",")]

# step1Result = step1(ranges)
step1Result = match(ranges, r"^(\d+)\1$")
print(f"Step1: {step1Result}")

step2Result = match(ranges, r"^(\d+)\1+$")
print(f"Step2: {step2Result}")
