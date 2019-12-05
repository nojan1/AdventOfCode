from ipu import IPU

initalMemory = [int(x) for x in open('input.txt').readline().split(',')]

def a():
    ipu = IPU(initalMemory)
    ipu.input.append(1)
    ipu.runToEnd()
    return ipu.output.popleft()

def b():
    ipu = IPU(initalMemory)
    ipu.input.append(5)
    ipu.runToEnd()
    return ipu.output.popleft()

print('A: %i' % a())
print('B: %i' % b())
