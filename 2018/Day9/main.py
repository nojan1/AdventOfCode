from linkedlist import LinkedList

NUM_ELVES = 428
LAST_MARBLE_SCORE = 70825

def solve(numElves, lastMarble):
    scores = [0 for x in range(numElves)]
    marbles = LinkedList()
    currentMarble = marbles.append(0)
    currentElfIndex = 0

    for marble in range(1, lastMarble + 1):
        if marble != 0 and marble % 23 == 0:
            scores[currentElfIndex] += marble

            toDelete = currentMarble.backward(7)
            scores[currentElfIndex] += toDelete.value
            currentMarble = marbles.unlink(toDelete).next
        else:
            currentMarble = marbles.insert(marble, currentMarble.next)

        currentElfIndex = (currentElfIndex + 1) % numElves

    return sorted(scores, reverse=True)[0]

a = solve(NUM_ELVES, LAST_MARBLE_SCORE)
print('A: %i' % a)

b = solve(NUM_ELVES, LAST_MARBLE_SCORE * 100)
print('B: %i' % b)
