import sys, random
sys.path.append('../Day5')
from ipu import IPU

from astar import AStarWithAvailableLocationList
from visualizer import Visualizer
    
START_POSITION = (30,30)

def walkGrid(visualizer, breakOnOxygen):
    initalMemory = [int(x) for x in open('input.txt').readline().split(',')]
    ipu = IPU(initalMemory)
    directions = [1,4,2,3]
    currentDirectionIndex = 0

    directionChange = [(0,-1), (1,0), (0,1), (-1,0)]
    position = START_POSITION
    grid={}

    visualizer.updateGridPos(position[0], position[1], 4)

    while True:
        ipu.input.append(directions[currentDirectionIndex])
        result, hasHalted = ipu.runUntilOutput()

        newPosition = (
            position[0] + directionChange[currentDirectionIndex][0],
            position[1] + directionChange[currentDirectionIndex][1]
        )
        
        if(newPosition != START_POSITION):
            visualizer.updateGridPos(newPosition[0], newPosition[1], result, False)

        if result == 0:
            grid[position] = '#'
            currentDirectionIndex = (currentDirectionIndex + 1) % len(directions)
        elif result == 1 or result == 2:
            if(position != START_POSITION):
                visualizer.updateGridPos(position[0], position[1], 1, False)

            grid[position] = '.'

            if result == 2 and breakOnOxygen:
                grid[newPosition] = 'O'
                return (newPosition, grid)
            else: 
                grid[newPosition] = '.'

            position = newPosition
            currentDirectionIndex = currentDirectionIndex - 1 
            if currentDirectionIndex < 0:
                currentDirectionIndex = len(directions) - 1
                
        if newPosition == START_POSITION:
            return (position, grid)

        if not visualizer.processEvents():
            raise Exception('Stopped')


def a(visualizer):
    position, grid = walkGrid(visualizer, True)
    visualizer.update()
    nonWallPositions = [c for c in grid.keys() if grid[c] != '#']

    astar = AStarWithAvailableLocationList(nonWallPositions)
    distance, path = astar.findPath(START_POSITION, position, False)
    print('A: %i ' % distance)

    for node in path[1:-1]:
        visualizer.updateGridPos(node.position[0], node.position[1], 5, True)

    return position

def b(visualizer, oxygenPosition):
    position, grid = walkGrid(visualizer, False)

    nonWallPositions = [c for c in grid.keys() if grid[c] != '#']

    astar = AStarWithAvailableLocationList(nonWallPositions)
    _, closedList = astar.findPath(oxygenPosition, (-1,-1), False)

    maxG = max(closedList, key=lambda v: v.g).g
    print('B: %i' % maxG)

    currentG = 0
    while currentG <= maxG:
        for node in closedList:
            percentage = node.g / maxG
            color = (0,255 * percentage,0)
            if node.g <= currentG:
                visualizer.updateGridPosWithColor(node.position[0], node.position[1], color, False)

        currentG += 1
        visualizer.update()


visualizer = Visualizer((800,800), {
    0: (80,80,80),
    1: (255,255,255),
    2: (255,0,0),
    4: (0,0,255),
    5: (0,200,0)
})

visualizer.waitForKeypress()
oxygenPosition = a(visualizer)

visualizer.waitForKeypress()
visualizer.clear()

b(visualizer, oxygenPosition)

visualizer.update()
visualizer.waitTillEnd()