from copy import deepcopy

class Computer(object):
    def __init__(self, instructions):
        self.instructions = instructions

        self.acc = 0
        self.pc = 0
        self.has_terminated = False
        self.repeated_pc = False
        self.pc_history = set()

    def run_cycle(self):
        global pc, acc
        instruction, value = self.instructions[self.pc]

        if instruction == "acc":
            self.acc += value
        elif instruction == "jmp":
            self.pc += value - 1

        self.pc += 1
        self.has_terminated = self.pc >= len(self.instructions)
        self.repeated_pc = self.pc in self.pc_history

        self.pc_history.add(self.pc)

        return (self.acc, self.pc, self.repeated_pc, self.has_terminated)

def parse_instructions(data):
    return [(x.split(" ")[0].strip(), int(x.split(" ")[1].strip())) for x in data.split("\n")]

def part1(data):
    instructions = parse_instructions(data)
    computer = Computer(instructions)
    while True:
        acc, pc, repeated, terminated = computer.run_cycle()
        if repeated:
            return acc

    return None

def part2(data):
    instructions = parse_instructions(data)

    for index_to_change, instruction in enumerate(instructions):
        if instruction[0] == "acc":
            continue

        local_instructions = deepcopy(instructions)

        if local_instructions[index_to_change][0] == "jmp":
            local_instructions[index_to_change] = ("nop", 0)
        else:
            local_instructions[index_to_change] = ("jmp", local_instructions[index_to_change][1])

        computer = Computer(local_instructions)
        while not computer.has_terminated and not computer.repeated_pc:
            computer.run_cycle()

        if computer.has_terminated:
            return computer.acc

    return None

tests = [
    ("08_test1", 5, part1),
    ("08_test1", 8, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
