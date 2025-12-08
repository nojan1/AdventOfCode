#
#   Advent of code 2025 day 1
#

from itertools import *
from functools import reduce
import fileinput

def step1(data):
    dial = 50
    zeroCount = 0
    for d in data:
        dial = (dial + d) % 100

        if dial == 0:
            zeroCount += 1
     
    return zeroCount

def step2(data):
    zeroTicks = 0
    dial = 50

    # for d in data:
    #     distanceLeft = dial if d < 0 else 99 - dial - 1
    #     dial = (dial + d) % 100

    #     if abs(d) > distanceLeft:
    #         zeroTicks += 1

    #         extraRotations = int((abs(d) - distanceLeft) / 99)
    #         if extraRotations > 0:
    #             zeroTicks += extraRotations - 1
        
    #         print(f"The dial is rotated {d} to point at {dial}; during this rotation points at 0 {"once" if extraRotations == 0 else 1 + extraRotations}")
    #     else:
    #         print(f"The dial is rotated {d} to point at {dial}")
        

    for d in data:
        while abs(d) > 0:
            distanceLeft = dial if d < 0 else 99 - dial - 1
            dial = (dial + d) % 100
            print(d)
            if abs(d) > distanceLeft:
                zeroTicks += 1

            if distanceLeft > abs(d):
                break
            
            d -= distanceLeft if d > 0 else -distanceLeft
        

    return zeroTicks

print("Running Advent of code 2025 day 1\n")

inputFile = fileinput.input(encoding="utf-8")
data = [int(line[1:]) if line[0] == "R" else int(line[1:]) * -1 for line in inputFile]

step1Result = step1(data)
print(f"Step1: {step1Result}")

step2Result = step2(data)

print(f"Step2: {step2Result}")

if step2Result >= 6773:
    print("Too higg!!!")
elif step2Result <= 5953:
    print("To low")
