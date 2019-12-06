from functools import reduce

orbitMap = dict((lambda x: (x[1], x[0]))(l.strip().split(")")) for l in open('input.txt'))

def countOrbits(origin, count = 0):
    return count + 1 if orbitMap[origin] == 'COM' else countOrbits(orbitMap[origin], count + 1)

a = reduce(lambda acc, cur: acc + countOrbits(cur), orbitMap.keys(), 0)
print('A: %i' % a)



