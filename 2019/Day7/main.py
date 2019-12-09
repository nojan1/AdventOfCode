from itertools import permutations
import sys
sys.path.append('../Day5')
from ipu import IPU

initalMemory = [int(x) for x in open('input.txt').readline().split(',')]

def run(phaseSetting, feedback):
    ipu = IPU(initalMemory)
    ipu.input.append(feedback)
    ipu.input.append(phaseSetting)
    ipu.runToEnd()
    return ipu.output.popleft()

def a():
    maxOutput = 0

    for permutation in permutations(range(5), 5):
        output = 0
        for x in permutation:
            output = run(x, output)

        if output > maxOutput:
            maxOutput = output

    return maxOutput

def b():
    maxOutput = 0

    for permutation in permutations(range(5, 10), 5):
        amplifiers = [IPU(initalMemory) for _ in range(5)]

        for i,amp in enumerate(amplifiers):
            amp.input.append(permutation[i])

        output = 0
        ampStepped = True
        while ampStepped:
            ampStepped = False

            for amp in amplifiers:
                if amp.hasHalted:
                    continue

                amp.input.appendleft(output)
                (newOutput, hasHalted) = amp.runUntilOutput()

                if(not hasHalted):
                    output = newOutput

                ampStepped = True

        if output > maxOutput:
            maxOutput = output

    return maxOutput

print('A: %i' % a())
print('B: %i' % b())