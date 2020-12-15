def memory_generator2(initial_numbers):
    numbers_spoken = {}
    #round_number = 1
    for i,number in enumerate(initial_numbers):
        numbers_spoken[number] = (i+1, -1)
     #   round_number += 1
        yield number

    yield 0
    last_number = 0
    numbers_spoken[0]= (len(initial_numbers) + 1, numbers_spoken[0][0])
    round_number = len(initial_numbers) + 2
    #last_number = initial_numbers[-1]

    while True:
        #print(f"The last number was {last_number}, current round is {round_number}, all the previous {numbers_spoken}")
        if last_number in numbers_spoken:
            new_number = numbers_spoken[last_number][0] - numbers_spoken[last_number][1] 

            if new_number in numbers_spoken:
                numbers_spoken[new_number] = (round_number, numbers_spoken[new_number][0])

            last_number = new_number 
        else:
            numbers_spoken[last_number] = (round_number, -1)
            last_number = 0
            numbers_spoken[0] = (round_number, numbers_spoken[0][0])
            
        #print(f"Yielding {last_number} for round {round_number}")
        yield last_number
        round_number += 1

def memory_generator(initial_numbers):
    numbers_spoken = {}
    for i,number in enumerate(initial_numbers):
        numbers_spoken[number] = (i+1, -1)
        yield number

    last_number = initial_numbers[-1]
    round_number = len(initial_numbers) + 1

    while True:
        if numbers_spoken[last_number][1] != -1:
            new_number = numbers_spoken[last_number][0] - numbers_spoken[last_number][1] 

            if new_number in numbers_spoken:
                numbers_spoken[new_number] = (round_number, numbers_spoken[new_number][0])
            else:
                numbers_spoken[new_number] = (round_number, -1)

            last_number = new_number
        else:
            numbers_spoken[0] = (round_number, numbers_spoken[0][0])
            last_number = 0

        yield last_number
        round_number += 1

def run_to(data, count):
    initial_numbers = [int(x.strip()) for x in data.split(",")]
    generator = memory_generator(initial_numbers)
    items = [next(generator) for _ in range(count)]
    return items[-1]

def part1(data):
    return run_to(data, 2020)

def part2(data):
    return run_to(data, 30000000)

tests = [
    ("15_test1", 436, part1),
    ("15_test1", 175594, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]

# 0, 3, 6, 0, 3, 3, 1, 0, 4, 0