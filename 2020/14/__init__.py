import re
from functools import reduce

def parse_input(data):
    current_mask = None
    mem_sets = []
    res = []

    for line in data.split("\n"):
        if "mask" in line:
            if current_mask != None:
                res.append((current_mask, mem_sets))
            
            current_mask = line[7:]
            mem_sets = []
        else:
            match = re.match(r"mem\[(\d+)\] = (\d+)", line)
            mem_sets.append((int(match.group(1)), int(match.group(2))))

    res.append((current_mask, mem_sets))
    return res

def part1(data):
    def run_program(blocks):
        def apply_value(mask, mem, addr, value):
            value_string = list(format(value, "036b"))

            for i,d in enumerate(mask):
                if d != 'X':
                    value_string[i] = d

            value = int("".join(value_string), 2)
            mem[addr] = value

        mem = {}
        for mask, ops in blocks:
            for addr,value in ops:
                apply_value(mask, mem, addr, value)

        return mem

    program_blocks = parse_input(data)
    mem = run_program(program_blocks)
    return sum(value for i,value in mem.items())

def part2(data):
    def run_program(blocks):
        def write_value(mask, mem, addr, value):
            def mutate_address(addr_string):
                floating_indicies = []
                for i,c in enumerate(mask):
                    if c == "1":
                        addr_string[i] = "1"
                    elif c == "X":
                        floating_indicies.append(i)

                yield addr_string

                floating_values = [False for _ in floating_indicies]
                for floating_count in range(2**len(floating_indicies)):
                    float_count_string = format(floating_count, f"0{len(floating_indicies)}b")
                    for i,global_i in enumerate(floating_indicies):
                        addr_string[global_i] = float_count_string[i]

                    yield addr_string

            addr_string = list(format(addr, "036b"))
            for mutated_addr_string in mutate_address(addr_string):
                addr = int("".join(mutated_addr_string), 2)
                mem[addr] = value

        mem = {}
        for mask, ops in blocks:
            for addr,value in ops:
                write_value(mask, mem, addr, value)
        return mem

    program_blocks = parse_input(data)
    mem = run_program(program_blocks)
    return sum(value for i,value in mem.items())

tests = [
    ("14_test1", 165, part1),
    ("14_test2", 208, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
