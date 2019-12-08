WIDTH = 25
HEIGHT = 6
PIXELS = WIDTH * HEIGHT

def countDigit(arr, digit):
    return len(list(filter(lambda x: x == digit, arr)))

data = [int(x) for x in open('input.txt').readline().strip()]
layers = [data[i:i+PIXELS] for i in range(0, len(data), PIXELS)]

def a():
    layersWithCount = list(map(lambda x: (countDigit(x, 0), x), layers))
    lowestLayer = min(layersWithCount)

    numberOfOnes = countDigit(lowestLayer[1], 1)
    numberOfTwos = countDigit(lowestLayer[1], 2)

    return numberOfOnes * numberOfTwos
    
def b():
    image = [2 for _ in range(PIXELS)]
    
    for i in range(PIXELS):
        for layer in layers[::-1]:
            if layer[i] != 2:
                image[i] = layer[i]

    return image

    
print('A: %i' % a())

image = b()
for i in range(0, len(image), WIDTH):
    print(''.join(['#' if x == 1 else ' ' for x in image[i:i+WIDTH]]))