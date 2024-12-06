#
#   Advent of code 2024 day 6
#

from itertools import *
from functools import *
import fileinput, sys

DIRECTIONS = [(0, -1), (1, 0), (0, 1), (-1, 0)]


def parseInput(inputFile):
    guardPosition = (0, 0)
    grid = []
    for y, line in enumerate(inputFile):
        row = []
        for x, c in enumerate(line.strip()):
            if c == "^":
                guardPosition = (x, y)
                row.append(".")
            else:
                row.append(c)

        grid.append(row)

    return (grid, guardPosition)


def walk(grid, guardPosition):
    backlog = []
    uniqueVisited = set([guardPosition])
    direction = DIRECTIONS[0]
    while True:
        lookAt = (guardPosition[0] + direction[0], guardPosition[1] + direction[1])

        if (
            lookAt[0] < 0
            or lookAt[0] > len(grid[0]) - 1
            or lookAt[1] < 0
            or lookAt[1] > len(grid) - 1
        ):
            break

        if grid[lookAt[1]][lookAt[0]] != ".":
            curIndex = DIRECTIONS.index(direction)
            direction = DIRECTIONS[(curIndex + 1) % len(DIRECTIONS)]
            continue

        guardPosition = lookAt
        uniqueVisited.add(guardPosition)
        backlog.append(guardPosition)

        if len(backlog) / 2 == len(set(backlog)):
            return uniqueVisited, True

    return uniqueVisited, False


def step1(grid, guardPosition):
    (uniqueVisited, _) = walk(grid, guardPosition)
    return len(uniqueVisited)


def step2(grid, guardPosition):
    successfulObstructionCount = 0

    for y in range(len(grid)):
        for x in range(len(grid[y])):
            if (x, y) == guardPosition or grid[y][x] != ".":
                continue

            backup = grid[y][x]
            grid[y][x] = "O"

            # print(f"Introducing obstruction at {x},{y}")
            (_, didLoop) = walk(grid, guardPosition)
            if didLoop:
                successfulObstructionCount += 1

            grid[y][x] = backup

    return successfulObstructionCount


print("Running Advent of code 2024 day 6\n")

inputFile = fileinput.input(encoding="utf-8")
grid, guardPosition = parseInput(inputFile)

step1Result = step1(grid, guardPosition)
print(f"Step1: {step1Result}")

step2Result = step2(grid, guardPosition)
print(f"Step2: {step2Result}")
