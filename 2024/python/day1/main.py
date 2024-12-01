#
#    Advent of code 2024 day 1
#

from itertools import *
from functools import *
import fileinput, sys, re


def step1(left, right):
    rejoined = zip(sorted(left), sorted(right))
    return reduce(lambda acc, cur: acc + abs(cur[0] - cur[1]), rejoined, 0)


def step2(left, right):
    def findSimilarCount(num):
        return reduce(lambda acc, r: acc + (1 if r == num else 0), right, 0)

    return reduce(lambda acc, cur: acc + findSimilarCount(cur) * cur, left, 0)


print("Running Advent of code 2024 day 1\n")

inputFile = fileinput.input(encoding="utf-8")

groups = map(
    lambda x: tuple(int(v.strip()) for v in re.split(r"\s+", x) if v != ""),
    inputFile,
)
left, right = zip(*groups)

step1Result = step1(left, right)
print(f"Step1: {step1Result}")

step2Result = step2(left, right)
print(f"Step2: {step2Result}")
