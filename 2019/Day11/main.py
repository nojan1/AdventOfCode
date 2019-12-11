import sys
sys.path.append('../Day5')
from ipu import IPU

initalMemory = [int(x) for x in open('input.txt').readline().split(',')]

def doPaint(initalColor):
    turns = [(0, -1), (1, 0), (0, 1), (-1, 0)]
    position = (0, 0)
    direction = 0
    panels = {}
    uniquePaintEvents = set()

    panels[position] = initalColor

    ipu = IPU(initalMemory)
    while not ipu.hasHalted:
        currentPanel = panels[position] if position in panels else 0
        ipu.input.append(currentPanel)

        paint, _ = ipu.runUntilOutput()
        directionChange, _ = ipu.runUntilOutput()

        panels[position] = paint
        uniquePaintEvents.add(position)

        direction += -1 if directionChange == 0 else 1

        if direction < 0:
            direction = len(turns) - 1
        elif direction > len(turns) - 1:
            direction = 0

        position = (position[0] + turns[direction][0],
                    position[1] + turns[direction][1])

    return (uniquePaintEvents, panels)


uniquePaintEvents, _ = doPaint(0)
print('A: %i' % len(uniquePaintEvents))

_, panels = doPaint(1)
positions = panels.keys()

maxX = max(positions, key=lambda c: c[0])[0]
maxY = max(positions, key=lambda c: c[1])[1]

print('B:')
for y in range(0, maxY + 1):
    line = ''
    for x in range(0, maxX + 1):
        if (x,y) in panels:
            line += '#' if panels[(x,y)] == 1 else ' '
        else:
            line += ' '

    print(line)
