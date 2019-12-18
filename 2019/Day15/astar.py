
class Node(object):
    def __init__(self, parent, position):
        self.position = position
        self.parent = parent
        self.g = 0
        self.h = 0

    def f(self):
        return self.g + self.h

    def __eq__(self, other):
        if other == None:
            return False

        return self.position == other.position

class AStarBase(object):
    def positionOkey(self, position):
        return True

    def findPath(self, fromPosition, toPosition, mayMoveHorizontal = True):
        openList = [Node(None, fromPosition)]
        closedList = []

        def addChild(currentNode, diff):
            childPosition = (
                currentNode.position[0] + diff[0],
                currentNode.position[1] + diff[1]
            )
            
            return Node(currentNode, childPosition) if self.positionOkey(childPosition) else None

        diffs = [(0,-1),(-1,0),(1,0),(0,1)]
        if mayMoveHorizontal:
            diffs = diffs + [(-1,-1),(1,-1),(-1,1),(1,1)]

        while len(openList) > 0:
            currentNode = min(openList, key=lambda v: v.f())
            openList.remove(currentNode)
            closedList.append(currentNode)

            if currentNode.position == toPosition:
                returnPath = []
            
                while currentNode is not None:
                    returnPath.append(currentNode)
                    currentNode = currentNode.parent

                return (returnPath[0].g, returnPath[::-1])

            for diff in diffs:
                child = addChild(currentNode, diff)
                if child == None or child in closedList:
                    continue

                child.g = currentNode.g + 1
                child.h = ((child.position[0] - toPosition[0]) ** 2) + ((child.position[1] - toPosition[1]) ** 2)

                if child in openList:
                    existingChild = next(filter(lambda n: n == child, openList))
                    if child.g > existingChild.g:
                        continue

                openList.append(child)

        return (-1, closedList)
            

class AStarWithAvailableLocationList(AStarBase):
    def __init__(self, availablePositions):
        super().__init__()
        self.availablePositions = availablePositions

    def positionOkey(self, position):
        return position in self.availablePositions
              

    
