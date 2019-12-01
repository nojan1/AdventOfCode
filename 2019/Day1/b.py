import math

def calculateFuelForMass(mass):
    return math.floor(mass / 3) - 2

def calculateTotalFuelForMass(mass):
    fuel = totalFuel = calculateFuelForMass(mass)

    while fuel > 0:
        fuel = calculateFuelForMass(fuel)

        if fuel > 0:
            totalFuel += fuel
    
    return totalFuel


moduleMasses = [calculateTotalFuelForMass(int(x)) for x in open('input.txt')]
moduleSum = sum(moduleMasses)

print('Sum is %i' % moduleSum)