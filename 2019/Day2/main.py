def step(address, memory):
    opcode = memory[address]
    if opcode == 99:
        return True

    x = memory[memory[address + 1]]
    y = memory[memory[address + 2]]
    resultAddress = memory[address + 3]

    if opcode == 1:
        result = x + y
        memory[resultAddress] = result
    elif opcode == 2:
        result = x * y
        memory[resultAddress] = result
    else:
        raise Exception('Unknown opcode')

    return step(address + 4, memory)

def a(memory):
    memory[1] = 12
    memory[2] = 2

    step(0, memory)
    return memory[0]

def b(memory):
    for verb in range(99):
        for noun in range(99):
            runInstanceMemory = list(memory)
            runInstanceMemory[1] = verb
            runInstanceMemory[2] = noun

            step(0, runInstanceMemory)

            if runInstanceMemory[0] == 19690720:
                return (verb, noun)

    raise Exception('No value found :(')


initalMemory = [int(x) for x in open('input.txt').readline().split(',')]

## A
print('A: %i' % a(list(initalMemory)))

## B
(verb, noun) = b(initalMemory)
answerB = (100 * verb) + noun

print('B: %i' % answerB)