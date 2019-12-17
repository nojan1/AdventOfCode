import pygame

class Visualizer(object):
    def __init__(self, size, colorMap, sizeMultiplier = 10):
        pygame.init()
        self.colorMap = colorMap
        self.sizeMultiplier = sizeMultiplier
        self.screen =  pygame.display.set_mode(size)

    def updateGridPos(self, x, y, value, redraw = True):
        color = pygame.Color(self.colorMap[value]) if value in self.colorMap else pygame.Color.white
        self.__updateGrid(x, y, color, redraw)

    def updateGridPosWithColor(self, x, y, color, redraw = True):
        self.__updateGrid(x,y,color,redraw)

    def __updateGrid(self, x, y, color, redraw):
        xCoord = x * self.sizeMultiplier
        yCoord = y * self.sizeMultiplier

        self.screen.fill(color, pygame.Rect(xCoord, yCoord, self.sizeMultiplier, self.sizeMultiplier))

        if redraw:
            pygame.display.flip()

    def processEvents(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return False

        return True

    def waitForKeypress(self):
        while True:
            for event in pygame.event.get():
                if event.type == pygame.KEYDOWN:
                    return

            pygame.time.wait(10)        

    def clear(self):
        self.screen.fill((0,0,0))
        pygame.display.flip()

    def update(self):
        pygame.display.flip()

    def waitTillEnd(self):
        while self.processEvents():
            pygame.time.wait(10)