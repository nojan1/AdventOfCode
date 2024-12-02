#
#   Advent of code 2024 day 2
#

from itertools import *
from functools import *
import fileinput, sys


class Report(object):
    def __init__(self, levels):
        self.levels = levels
        self.slope = None

        for i in range(1, len(levels)):
            curSlope = 1 if levels[i - 1] < levels[i] else -1
            if self.slope == None:
                self.slope = curSlope

            if curSlope != self.slope:
                self.valid = False
                return

            change = abs(levels[i - 1] - levels[i])
            if change < 1 or change > 3:
                self.valid = False
                return

        self.valid = True

    def __repr__(self) -> str:
        return f"Levels: {", ".join([str(x) for x in self.levels])} Slope: {self.slope}, Valid: {self.valid}"


def step1(rawReports):
    valids = [x for x in map(Report, rawReports) if x.valid]
    return len(valids)


def step2(rawReports):
    validReports = 0

    for rawReport in rawReports:
        if Report(rawReport).valid:
            validReports += 1
            continue

        for i in range(0, len(rawReport)):
            trimmedReport = list(rawReport)
            del trimmedReport[i]

            if Report(trimmedReport).valid:
                validReports += 1
                break

    return validReports


print("Running Advent of code 2024 day 2\n")

inputFile = fileinput.input(encoding="utf-8")
rawReports = list(map(lambda x: [int(l) for l in x.split(" ") if x != ""], inputFile))

step1Result = step1(rawReports)
print(f"Step1: {step1Result}")

step2Result = step2(rawReports)
print(f"Step2: {step2Result}")
