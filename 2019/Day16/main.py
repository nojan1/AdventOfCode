from functools import reduce
from itertools import accumulate
from numpy import fft

def buildPattern(i):
    basePattern = [0, 1, 0, -1]
    firstDigit = True

    while True:
        for digit in basePattern:
            for _ in range(i + 1):
                if firstDigit:
                    firstDigit = False
                    continue

                yield digit

def doPhase(inputData):
    for i,n in enumerate(inputData):
        pattern = buildPattern(i)
        result = reduce(lambda acc,cur: acc + (cur * next(pattern)), inputData, 0)

        yield int(str(result)[-1])

def a(data):
    for _ in range(100):
        data = list(doPhase(data))
    
    return ''.join([str(n) for n in result[0:8]])

def b(data):
    offset = int(''.join([str(x) for x in data[0:7]]))

    realData = data * 10000
    data = realData[offset:][::-1]

    for _ in range(100):
        data = list(accumulate(data, lambda a,b: (a+b)%10))

    return ''.join([str(x) for x in data[::-1][0:8]])


data = [int(x) for x in open('input.txt').readline().strip()]

print('A: %s' % a(list(data)))
print('B: %s' % b(data))