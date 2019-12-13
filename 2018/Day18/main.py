import copy, time
from functools import reduce

def getAdjacentValues(state, centerX, centerY):
    for y in range(centerY - 1, centerY + 2):
        for x in range(centerX - 1, centerX + 2):
            if x == centerX and y == centerY:
                continue

            if x >= 0 and y >= 0 and x < len(state[0]) and y < len(state):
                yield state[y][x]


def calculateResourceValue(numMinutes):
    state = [[x for x in line.strip()] for line in open('input.txt')]

    startTime = time.process_time()
    for minute in range(numMinutes):
        if minute > 0 and minute % 1000 == 0:
            timeTaken = time.process_time() - startTime
            print('Currently at %i minutes, %f seconds expired' % (minute, timeTaken))

        newState = copy.deepcopy(state)

        for y, line in enumerate(state):
            for x, acre in enumerate(line):
                adjacentValues = list(getAdjacentValues(state, x, y))

                adjacentTrees = adjacentValues.count('|')
                adjacentLumberyards = adjacentValues.count('#')

                if acre == '.':
                    if adjacentTrees >= 3:
                        newState[y][x] = '|'

                elif acre == '|':
                    if adjacentValues.count('#') >= 3:
                        newState[y][x] = '#'

                else:
                    if adjacentLumberyards < 1 or adjacentTrees < 1:
                        newState[y][x] = '.'

        state = newState

    allStates = reduce(lambda acc,cur: acc + cur, state, [])
    numWooded = allStates.count('|')
    numLumberyard = allStates.count('#')

    return numWooded * numLumberyard

print('A: %i' % calculateResourceValue(10))
print('B: %i' % calculateResourceValue(1000000000))