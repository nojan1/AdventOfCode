import math

asteroids = [[c == '#' for c in l.strip()] for l in open('input.txt')]

def coordinateOkey(x, y):
    return x >= 0 and x < len(asteroids[0]) and y >= 0 and y < len(asteroids)

def getLOSBlockers(fromX, fromY):
    blockers = []
    allAsteroidsByAngle = {}
    blockedAngles = set()
    maxDistance = max(len(asteroids[0]), len(asteroids))

    def tryCoordinate(x,y):
        if coordinateOkey(x, y) and asteroids[y][x]:
            angle = math.atan2(x - fromX, y - fromY)

            if not angle in blockedAngles:
                blockedAngles.add(angle)

                if (x,y) not in blockers:
                    blockers.append((x,y))
                
            angleDeg = math.degrees(angle)
            if not angleDeg in allAsteroidsByAngle:
                allAsteroidsByAngle[angleDeg] = []

            allAsteroidsByAngle[angleDeg].append((x,y))
            
    for distance in range(1, maxDistance + 1):
        for y in [fromY - distance, fromY + distance]:
            for x in range(fromX - distance, fromX + distance + 1):
                tryCoordinate(x, y)

        for x in [fromX - distance, fromX + distance]:
            for y in range(fromY - distance + 1, fromY + distance):
                tryCoordinate(x, y)

    return (blockers, allAsteroidsByAngle)

def a():
    def countSeen(fromX, fromY):
        blockers = getLOSBlockers(fromX, fromY)[0]
        return len(blockers)

    def getCounts():
        for y in range(len(asteroids)):
            for x in range(len(asteroids[0])):
                if asteroids[y][x]:
                    yield ((x,y), countSeen(x, y))

    return max(getCounts(), key=lambda v: v[1])

def b(stationX, stationY):
    def fixAngles(anglesRaw):
        sortedAngles = sorted(anglesRaw)
        sortedAngles.reverse()

        slicePoint = 0
        while sortedAngles[slicePoint] > 180:
            slicePoint -= 1

        return sortedAngles[slicePoint:] + sortedAngles[0:slicePoint]
        

    def find200thDestroyedAsteroid():
        allAsteroidsSeenByAngle = getLOSBlockers(stationX, stationY)[1]
        asteroidsDestroyed = 0

        angles = fixAngles(allAsteroidsSeenByAngle.keys())

        while True:
            for angle in angles:
                if len(allAsteroidsSeenByAngle[angle]) > 0:
                    asteroidCoordinates = allAsteroidsSeenByAngle[angle][0]
                    del allAsteroidsSeenByAngle[angle][0]

                    asteroidsDestroyed += 1 
                    if asteroidsDestroyed == 200:
                        return asteroidCoordinates

    coordinates = find200thDestroyedAsteroid()
    return (coordinates[0] * 100) + coordinates[1]

baseStation = a()
print('A: %i' % baseStation[1])
print('B: %i' % b(baseStation[0][0], baseStation[0][1]))