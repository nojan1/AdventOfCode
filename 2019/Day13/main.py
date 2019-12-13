import sys
sys.path.append('../Day5')
from ipu import IPU

import pygame
from pygame.locals import K_LEFT, K_RIGHT, K_UP

def a(inputFile):
    initalMemory = [int(x) for x in open(inputFile).readline().split(',')]
    blocks = {}

    ipu = IPU(initalMemory)
    while not ipu.hasHalted:
        x,_ = ipu.runUntilOutput()
        y,_ = ipu.runUntilOutput()
        blockId,_ = ipu.runUntilOutput()

        blocks[(x,y)] = blockId

    return list(blocks.values()).count(2)

class Arcade(object):
    def __init__(self, inputFile):
        initalMemory = [int(x) for x in open('input.txt').readline().split(',')]
        self.ipu = IPU(initalMemory, self.getJoystick)
        self.ipu.memory[0] = 2
        self.blocks = {}
        self.score = 0
        self.ballPosition = None
        self.paddlePosition = None

        pygame.init()
        self.screen = pygame.display.set_mode((640,480))

        self.colorMap = {
            0: pygame.Color(0,0,0),
            1: pygame.Color(50,50,50),
            2: pygame.Color(0,0,100),
            3: pygame.Color(255,255,255),
            4: pygame.Color(255,0,0)
        }

        self.screen.fill(self.colorMap[0])
        pygame.display.flip()

    def getJoystick(self):
        if(self.ballPosition == None or self.paddlePosition == None):
            return 0

        if self.ballPosition[0] < self.paddlePosition[0]:
            return -1
        elif self.ballPosition[0] > self.paddlePosition[0]:
            return 1
        else:
            return 0

    def drawBlock(self, x, y, blockId):
        rectSize = 10
        coordX = x * rectSize
        coordY = y * rectSize

        self.screen.fill(self.colorMap[blockId], rect = pygame.Rect(coordX, coordY, rectSize, rectSize))

    def gameLoop(self):
        def inner():
            inInitalDraw = True
            while not self.ipu.hasHalted:
                for event in pygame.event.get():
                    if event.type == pygame.QUIT:
                        return

                x,_ = self.ipu.runUntilOutput()
                y,_ = self.ipu.runUntilOutput()
                blockId,_ = self.ipu.runUntilOutput()

                if x == -1 and y == 0:
                    self.score = blockId
                else:
                    if blockId == 4:
                        self.ballPosition = (x,y)
                    elif blockId == 3:
                        self.paddlePosition = (x,y)

                    self.drawBlock(x,y,blockId)
                    if not inInitalDraw:
                        pygame.display.flip()

                if x == 34 and y == 22:
                    inInitalDraw = False
                    pygame.display.flip()
                    
      
        inner()
        pygame.quit()


print('A: %i' % a('input.txt'))

arcade = Arcade('input.txt')
arcade.gameLoop()

print('B: %i' % arcade.score)