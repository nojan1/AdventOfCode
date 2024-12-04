#
#   Advent of code 2024 day 4
#

from itertools import *
from functools import *
import fileinput, sys


def searchWord(word, start, direction):
    foundIndex = 0
    current = start

    while True:
        current = (current[0] + direction[0], current[1] + direction[1])

        if (
            current[0] < 0
            or current[0] > len(grid[0]) - 1
            or current[1] < 0
            or current[1] > len(grid) - 1
        ):
            return False

        if grid[current[1]][current[0]] == word[foundIndex + 1]:
            foundIndex += 1
            if foundIndex == len(word) - 1:
                return True
        else:
            return False


def step1(grid):
    word = "XMAS"
    counts = 0
    for y in range(len(grid)):
        for x in range(len(grid[y])):
            if grid[y][x] == word[0]:
                checks = [
                    searchWord(word, (x, y), (0, -1)),
                    searchWord(word, (x, y), (1, 0)),
                    searchWord(word, (x, y), (0, 1)),
                    searchWord(word, (x, y), (-1, 0)),
                    searchWord(word, (x, y), (1, -1)),
                    searchWord(word, (x, y), (1, 1)),
                    searchWord(word, (x, y), (-1, 1)),
                    searchWord(word, (x, y), (-1, -1)),
                ]

                numSuccess = len([i for i in checks if i])
                if numSuccess > 0:
                    counts += numSuccess

    return counts


def step2(grid):
    counts = 0

    for y in range(1, len(grid) - 1):
        for x in range(1, len(grid[y]) - 1):

            if (
                grid[y][x] == "A"
                and (
                    searchWord("-MAS", (x - 2, y - 2), (1, 1))
                    or searchWord("-MAS", (x + 2, y + 2), (-1, -1))
                )
                and (
                    searchWord("-MAS", (x + 2, y - 2), (-1, 1))
                    or searchWord("-MAS", (x - 2, y + 2), (1, -1))
                )
            ):
                counts += 1

    return counts


print("Running Advent of code 2024 day 4\n")

inputFile = fileinput.input(encoding="utf-8")
grid = [x.strip() for x in inputFile]

step1Result = step1(grid)
print(f"Step1: {step1Result}")

step2Result = step2(grid)
print(f"Step2: {step2Result}")
