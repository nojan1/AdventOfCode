def buildLineCoordinates(line):
    changeMap = {
        'U': (0, +1),
        'L': (-1, 0),
        'D': (0, -1),
        'R': (+1, 0)
    }

    coordinates = []
    instructions = line.strip().split(',')
    for instruction in instructions:
        direction = instruction[0]
        distance = int(instruction[1::])

        for _ in range(distance):
            lastCoord = coordinates[-1] if len(coordinates) > 0 else (0,0)
            coordinates.append(
                (lastCoord[0] + changeMap[direction][0], lastCoord[1] + changeMap[direction][1])
            )

    return coordinates

def findMatchingCoordinates(wires):
    seen = set(wires[0])

    for x in wires[1]:
        if x in seen:
            yield x

def getDistance(coordinate):
    return abs(coordinate[0]) + abs(coordinate[1])

def getStepsToCrossing(wire, crossing):
    for i,x in enumerate(wire):
   
        if x == crossing:
            return i+1

    raise Exception('Crossing not found')

wires = [ buildLineCoordinates(l) for l in open('input.txt').readlines() ]
crossings = list(findMatchingCoordinates(wires))
distances = list(map(getDistance, crossings))

minDistance = min(distances)
print('A: %i' % minDistance)

stepSums = list(map(lambda crossing: getStepsToCrossing(wires[0], crossing) + getStepsToCrossing(wires[1], crossing), crossings))
minStepSum = min(stepSums)
print('B: %i' % minStepSum)