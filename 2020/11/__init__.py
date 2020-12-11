from copy import deepcopy

def count_adjacent_occupied(grid, x, y):
    def is_occupied(x, y):
        return y < len(grid) and y >= 0 and x < len(grid[0]) and x >= 0 and grid[y][x] == "#"

    positions = [
        (x - 1, y - 1),
        (x, y - 1),
        (x + 1, y - 1),
        (x - 1, y),
        (x + 1, y),
        (x - 1, y + 1),
        (x, y + 1),
        (x + 1, y + 1),
    ]

    return sum(1 if is_occupied(x2,y2) else 0 for (x2,y2) in positions)

def count_visible_occupied(grid, x, y):
    def can_see_occupied(xDiff, yDiff):
        currentX = x
        currentY = y

        while True:
            currentX += xDiff
            currentY += yDiff

            if currentY < 0 or currentY >= len(grid) or currentX < 0 or currentX >= len(grid[0]):
                return False 
            elif grid[currentY][currentX] == "#":
                return True
            elif grid[currentY][currentX] == "L":
                return False

    diffs = [
        (-1, -1),
        (0, -1),
        (+1, -1),
        (-1, 0),
        (+1, 0),
        (-1, +1),
        (0, +1),
        (+1, +1),
    ]

    return sum(1 if can_see_occupied(xDiff,yDiff) else 0 for (xDiff,yDiff) in diffs)

def count_occupied(grid):
    count = 0
    for y in range(len(grid)):
        for x in range(len(grid[0])):
            if grid[y][x] == "#":
                count += 1

    return count

def mutate_grid(grid, use_los):
    grid_copy = deepcopy(grid)
    has_changed = False

    for y in range(len(grid)):
        for x in range(len(grid[0])):
            if use_los:
                num_occupied = count_visible_occupied(grid, x, y)
                target_number = 5
            else:
                num_occupied = count_adjacent_occupied(grid, x, y)
                target_number = 4

            if grid[y][x] == "L" and num_occupied == 0:
                grid_copy[y][x] = "#"
                has_changed = True
            elif grid[y][x] == "#" and num_occupied >= target_number:
                grid_copy[y][x] = "L"
                has_changed = True

    return (grid_copy, has_changed)

def wait_until_settle(grid, use_los=False):
    while True:
        grid, has_changed = mutate_grid(grid, use_los)
        if not has_changed:
            return grid

def parse_grid(data):
    return [list(l) for l in data.split("\n")]

def part1(data):
    grid = parse_grid(data)
    grid = wait_until_settle(grid)
    return count_occupied(grid)

def part2(data):
    grid = parse_grid(data)
    grid = wait_until_settle(grid, True)
    return count_occupied(grid)

tests = [
    ("11_test1", 37, part1),
    ("11_test1", 26, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
