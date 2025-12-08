#
#   Advent of code 2025 day 4
#

from itertools import *
from functools import *
import fileinput, sys

def getAccessible(grid):
    def check(x, y):
        global grid

        if x < 0 or y < 0 or x > len(grid[0]) - 1 or y > len(grid) - 1:
            return False

        return grid[y][x]
    

    accessible = []
    for x in range(len(grid[0])):
        for y in range(len(grid)):
            if not grid[y][x]:
                continue
            
            count = (1 if check(x - 1, y - 1) else 0) + \
                    (1 if check(x - 0, y - 1) else 0) + \
                    (1 if check(x + 1, y - 1) else 0) + \
                    (1 if check(x - 1, y - 0) else 0) + \
                    (1 if check(x + 1, y - 0) else 0) + \
                    (1 if check(x - 1, y + 1) else 0) + \
                    (1 if check(x - 0, y + 1) else 0) + \
                    (1 if check(x + 1, y + 1) else 0)

            if count < 4:
                accessible.append((x,y))

    return accessible

def step1(grid):
    return len(getAccessible(grid))

def step2(grid):
    removalTotal = 0

    while True:
        accessible = getAccessible(grid)
        if len(accessible) == 0:
            break
        
        removalTotal += len(accessible)
        for x,y in accessible:
            grid[y][x] = False

    return removalTotal

print("Running Advent of code 2025 day 4\n")

inputFile = fileinput.input(encoding="utf-8")
grid = [[x == "@" for x in y.strip()] for y in inputFile]

step1Result = step1(grid)
print(f"Step1: {step1Result}")

step2Result = step2(grid)
print(f"Step2: {step2Result}")
