import sys
sys.path.append('../Day5')
from ipu import IPU
from functools import reduce

def getBeamStatusAt(memory, x, y):
    ipu = IPU(list(memory))
    ipu.input.appendleft(x)
    ipu.input.appendleft(y)

    output, _ = ipu.runUntilOutput()
    return output

def a(memory):
    val = 0
    for y in range(50):
        for x in range(50):
            val += getBeamStatusAt(memory, x, y)

    return val

def b(memory):
    #Good start values determined by looking at output from a
    y = 5 
    x = 3

    while True:
        y += 1
        while getBeamStatusAt(memory, x, y) == 0:
            x += 1

        topRightX = x + 99
        topRightY = y - 99
        if topRightY < 0:
            continue

        topRightStatus = getBeamStatusAt(memory, topRightX, topRightY)
        if topRightStatus == 1:
            return (x * 10000) + topRightY

initalMemory = [int(x) for x in open('input.txt').readline().split(',')]
print('A: %i' % a(initalMemory))
print('B: %i' % b(initalMemory))