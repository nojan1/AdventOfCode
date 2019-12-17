from collections import deque

class IPU(object):
    def __init__(self, instructions, inputFunction = None):
        self.input = deque()
        self.output = deque()
        self.address = 0
        self.memory = {i: x for i, x in enumerate(instructions)}
        self.hasHalted = False
        self.relativeBase = 0
        self.inputFunction = inputFunction

    def _add(self, param1, param2, param3):
        self.writeMemory(param3, self.getParamValue(param1) + self.getParamValue(param2))

    def _multiply(self, param1, param2, param3):
        self.writeMemory(param3, self.getParamValue(param1) * self.getParamValue(param2))

    def _store(self, param1):
        if self.inputFunction != None and len(self.input) == 0:
            self.writeMemory(param1, self.inputFunction())
        else:
            self.writeMemory(param1, self.input.pop())

    def _output(self, param1):
        self.output.appendleft(self.getParamValue(param1))

    def _jumpIfTrue(self, param1, param2):
        if self.getParamValue(param1) != 0:
            self.address = self.getParamValue(param2)
            return True

        return False

    def _jumpIfFalse(self, param1, param2):
        if self.getParamValue(param1) == 0:
            self.address = self.getParamValue(param2)
            return True

        return False

    def _isLessThan(self, param1, param2, param3):
        self.writeMemory(param3, 1 if self.getParamValue(param1) < self.getParamValue(param2) else 0)

    def _equals(self, param1, param2, param3):
        self.writeMemory(param3, 1 if self.getParamValue(param1) == self.getParamValue(param2) else 0)

    def _setRelativeBase(self, param1):
        self.relativeBase += self.getParamValue(param1)

    def getParamValue(self, param):
        if param[1] == '0':
            return self.memory[param[0]] if param[0] in self.memory else 0
        elif param[1] == '1':
            return param[0]
        else:
            address = self.relativeBase + param[0]
            return self.memory[address] if address in self.memory else 0

    def writeMemory(self, writeParam, value):
        if writeParam[1] == '0':
            self.memory[writeParam[0]] = value
        elif writeParam[1] == '2':
            self.memory[writeParam[0] + self.relativeBase] = value
        else:
            raise Exception('Unsupported write mode %s at address %i in opcode %i' % (writeParam[1], self.address, self.memory[self.address]))

    def runStep(self):
        opcodeLookup = {
            1: (3, self._add),
            2: (3, self._multiply),
            3: (1, self._store),
            4: (1, self._output),
            5: (2, self._jumpIfTrue),
            6: (2, self._jumpIfFalse),
            7: (3, self._isLessThan),
            8: (3, self._equals),
            9: (1, self._setRelativeBase)
        }

        if self.address > len(self.memory) - 1:
            self.hasHalted = True
            return False

        rawOpcode = str(self.memory[self.address]).zfill(5)
        opcode = int(rawOpcode[-2::])

        if opcode == 99:
            self.hasHalted = True
            return False
        elif not opcode in opcodeLookup:
            raise Exception('Opcode lookup failed, tried to find %i on address %i' % (opcode, self.address))

        lookedUpOpCode = opcodeLookup[opcode]

        paramMode = list(rawOpcode[2::-1])
        params = [(self.memory[self.address + 1 + i], paramMode[i]) for i in range(lookedUpOpCode[0])]

        if not lookedUpOpCode[1](*params):
            self.address += lookedUpOpCode[0] + 1
            
        return True

    def runToEnd(self):
        while self.runStep():
            pass

    def runUntilOutput(self):
        outputBufferLengthBefore = len(self.output)

        while True:
            stepStatus = self.runStep()

            if(not stepStatus):
                return (0, True)
            elif len(self.output) != outputBufferLengthBefore:
                return (self.output.popleft(), False)