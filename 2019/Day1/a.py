import math

def calculateFuel(mass):
    return math.floor(mass / 3) - 2

moduleMasses = [calculateFuel(int(x)) for x in open('input.txt')]
moduleSum = sum(moduleMasses)

print('Sum is %i' % moduleSum)