orbitMap = dict((lambda x: (x[1], x[0]))(l.strip().split(")")) for l in open('input.txt'))

def getPathToCOM(initalPoint):
    path = [orbitMap[initalPoint]]
    while path[-1] != 'COM':
        path.append(orbitMap[path[-1]])
    
    return path

def removeCommon(mePath, santaPath):
    while mePath[0] == santaPath[0]:
        del mePath[0]
        del santaPath[0]

mePath = getPathToCOM('YOU')
santaPath = getPathToCOM('SAN')

mePath.reverse()
santaPath.reverse()

removeCommon(mePath, santaPath)

b = len(mePath) + len(santaPath)
print('B: %i' % b)