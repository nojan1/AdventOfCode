from collections import deque

class IPU(object):
    def __init__(self, instructions):
        self.input = deque()
        self.output = deque()
        self.address = 0
        self.instructions = {i: x for i, x in enumerate(instructions)}

    def _add(self, param1, param2, param3):
        self.instructions[param3] = param1 + param2

    def _multiply(self, param1, param2, param3):
        self.instructions[param3] = param1 * param2

    def _store(self, param1):
        self.instructions[param1] = self.input.pop()

    def _output(self, param1):
        self.output.appendleft(param1)

    def _jumpIfTrue(self, param1, param2):
        if param1 != 0:
            self.address = param2
            return True

        return False

    def _jumpIfFalse(self, param1, param2):
        if param1 == 0:
            self.address = param2
            return True

        return False

    def _isLessThan(self, param1, param2, param3):
        self.instructions[param3] = 1 if param1 < param2 else 0

    def _equals(self, param1, param2, param3):
        self.instructions[param3] = 1 if param1 == param2 else 0

    def runToEnd(self):
        opcodeLookup = {
            #Format: opcode: (numArguments, function, paramater_mode_overrides)
            1: (3, self._add, [None, None, True]),
            2: (3, self._multiply, [None, None, True]),
            3: (1, self._store, [True]),
            4: (1, self._output, [None]),
            5: (2, self._jumpIfTrue, [None, None]),
            6: (2, self._jumpIfFalse, [None, None]),
            7: (3, self._isLessThan, [None, None, True]),
            8: (3, self._equals, [None, None, True])
        }

        while self.address < len(self.instructions):
            rawOpcode = str(self.instructions[self.address]).zfill(5)
            opcode = int(rawOpcode[-2::])

            if opcode == 99:
                break
            elif not opcode in opcodeLookup:
                raise Exception('Opcode lookup failed, tried to find %i on address %i' % (opcode, self.address))

            lookedUpOpCode = opcodeLookup[opcode]

            paramMode = [x == '1' for x in rawOpcode[2::-1]]

            #Load paramMode overrides
            for i, _ in enumerate(paramMode):
                if i < len(lookedUpOpCode[2]) and lookedUpOpCode[2][i] != None:
                    paramMode[i] = lookedUpOpCode[2][i]

            params = [self.instructions[self.address + 1 + i] if paramMode[i] else self.instructions[self.instructions[self.address + 1 + i]] for i in range(lookedUpOpCode[0])]

            if not lookedUpOpCode[1](*params):
                self.address += lookedUpOpCode[0] + 1