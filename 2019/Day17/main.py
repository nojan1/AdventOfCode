from functools import reduce
import sys
sys.path.append('../Day5')
from ipu import IPU

sys.path.append('../Day15')
from visualizer import Visualizer

def isIntersection(grid, pos):
    def positionIsIntersection(x,y):
        return (x,y) in grid and grid[(x,y)] == '#'

    return positionIsIntersection(pos[0] + 0, pos[1] - 1) and positionIsIntersection(pos[0] - 1, pos[1] + 0) and positionIsIntersection(pos[0] + 1, pos[1] + 0) and positionIsIntersection(pos[0] + 0, pos[1] + 1)

def getInitalGrid(memory):
    ipu = IPU(list(initalMemory))

    grid = {}
    x = y = 0
    robotPos = None
    line = ''
    while not ipu.hasHalted:
        output,_ = ipu.runUntilOutput()
        c = chr(output)

        if c == '\n':
            y += 1
            x = 0

            print(line)
            line = ''
        else:
            if c == '^':
                robotPos = (x,y)

            line += c
            grid[(x,y)] = c
            x += 1

    print(line)
    return grid, robotPos

def a(grid):
    intersections = [p for p in grid.keys() if grid[p] == '#' and isIntersection(grid, p)]
    alignmentParameters = list(map(lambda pos: pos[0] * pos[1], intersections))
    return sum(alignmentParameters)

def b(memory, visualizer):
    # Manual solve ftw
    # PATH: L10 R10 L10 L10 R10 R12 L12 L10 R10 L10 L10 R10 R12 L12 R12 L12 R6 R12 L12 R6 R10 R12 L12 R10 L10 L10 R10 R12 L12 R12 L12 R6
    # ORDER: A B A B C C B A B C
    # RULES:
    # A: L10 R10 L10 L10
    # B: R10 R12 L12
    # C: R12 L12 R6
    
    ipu = IPU(memory)
    ipu.memory[0] = 2

    def addAsAsciiInput(ascii):
        for c in ascii:
            ipu.input.appendleft(ord(c))

        ipu.input.appendleft(10)

    addAsAsciiInput('A,B,A,B,C,C,B,A,B,C')
    addAsAsciiInput('L,10,R,10,L,10,L,10')
    addAsAsciiInput('R,10,R,12,L,12')
    addAsAsciiInput('R,12,L,12,R,6')

    addAsAsciiInput('y')

    x = y = 0
    while not ipu.hasHalted:
        output,_ = ipu.runUntilOutput()

        if(output < 255):
            c = chr(output)
            
            if c == '\n':
                y += 1

                if x == 0:
                    y = 0
                    visualizer.update()
                    visualizer.processEvents()

                x = 0
            elif c in ['.', '#', '^', 'v', '<', '>']:
                visualizer.updateGridPos(x, y, output, False)
                x += 1
            
        else: 
            return output

    return -1



initalMemory = [int(x) for x in open('input.txt').readline().split(',')]
grid, playerPos = getInitalGrid(initalMemory)

print('A: %i' % a(grid))

visualizer = Visualizer((800,800), {
        ord('#'): (200,200,200),
        ord('^'): (0,0,255),
        ord('>'): (0,0,255),
        ord('v'): (0,0,255),
        ord('<'): (0,0,255)
    })

print('B: %i' % b(initalMemory, visualizer))

visualizer.waitTillEnd()