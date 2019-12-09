import sys
sys.path.append('../Day5')
from ipu import IPU

initalMemory = [int(x) for x in open('input.txt').readline().split(',')]

def run(x):
    ipu = IPU(initalMemory)
    ipu.input.append(x)
    ipu.runToEnd()
    return ipu.output.pop()

print('A: %i' % run(1))
print('B: %i' % run(2))